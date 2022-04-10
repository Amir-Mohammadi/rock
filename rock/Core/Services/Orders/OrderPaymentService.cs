using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Controllers;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Payment;
using rock.Models.CommonApi;
using rock.Models.CustomerApi;
using rock.Core.Services.Common;
using rock.Core.Errors;
using Microsoft.Extensions.Options;
using rock.Framework.Setting;

namespace rock.Core.Services.Orders
{
  public class OrderPaymentService : IOrderPaymentService
  {
    private IRepository<OrderPayment> orderPaymentRepository { get; set; }
    private readonly IOrderService orderService;
    private readonly IWorkContext workContext;
    private readonly IErrorFactory errorFactory;
    private IOptionsSnapshot<SiteSettings> siteSettings;
    public OrderPaymentService(
      IRepository<OrderPayment> orderPaymentRepository,
      IWorkContext workContext,
      IErrorFactory errorFactory,
      IOrderService orderService,
      IOptionsSnapshot<SiteSettings> siteSettings)
    {
      this.orderPaymentRepository = orderPaymentRepository;
      this.workContext = workContext;
      this.errorFactory = errorFactory;
      this.orderService = orderService;
      this.siteSettings = siteSettings;
    }

    #region  OrderPayment
    public async Task InsertOrderPayment(OrderPayment orderPayment, CancellationToken cancellationToken)
    {
      orderPayment.CreatedAt = DateTime.UtcNow;
      await orderPaymentRepository.AddAsync(entity: orderPayment, cancellationToken: cancellationToken);
    }
    public async Task UpdateOrderPayment(OrderPayment orderPayment, CancellationToken cancellationToken)
    {
      await orderPaymentRepository.UpdateAsync(entity: orderPayment, cancellationToken: cancellationToken);
    }

    public async Task<double> CalculateMinimumOrderPayment(int orderId, CancellationToken cancellationToken)
    {

      #region  Get Order And Related Part
      var order = await orderService.GetOrderById(id: orderId,
                                                  cancellationToken: cancellationToken,
                                                  new Include<Order>(query =>
                                                  {
                                                    query = query.Include(x => x.Items);
                                                    query = query.Include(x => x.Items).ThenInclude(x => x.LatestStatus);
                                                    query = query.Include(x => x.Items).ThenInclude(x => x.CartItem).ThenInclude(x => x.ProductPrice);
                                                    query = query.Include(x => x.Items).ThenInclude(x => x.Transport);
                                                    query = query.Include(x => x.Transports);
                                                    return query;
                                                  }));
      #endregion

      #region  Calculate Delivered Item Price And Transport
      var deliveredItems = order.Items.Where(x => x.LatestStatus.Type == OrderItemStatusType.Delivered);

      var sumDeliveredItemPrice = deliveredItems.Sum(x => (x.CartItem.ProductPrice.Price
                                                           - (x.CartItem.ProductPrice.Price * x.CartItem.ProductPrice.Discount)
                                                           / 100) * x.CartItem.Amount);


      double deliveredTransportPrice = 0;
      foreach (var item in order.Transports)
      {
        var orderItem = deliveredItems.FirstOrDefault(x => x.TransportId == item.Id);
        if (orderItem != null)
        {
          deliveredTransportPrice += orderItem.Transport.Cost;
        }
      }
      #endregion

      #region  Calculate Not Delivered Item Price And Transport
      var notDeliveredItems = order.Items.Where(x => x.LatestStatus.Type != OrderItemStatusType.Delivered);

      var sumNotDeliveredItemPrice = notDeliveredItems.Sum(x => (x.CartItem.ProductPrice.Price
                                                                - (x.CartItem.ProductPrice.Price * x.CartItem.ProductPrice.Discount)
                                                                / 100) * x.CartItem.Amount);

      double notDeliveredTransportPrice = 0;
      foreach (var item in order.Transports)
      {
        var orderItem = notDeliveredItems.FirstOrDefault(x => x.TransportId == item.Id);
        if (orderItem != null)
        {
          notDeliveredTransportPrice += orderItem.Transport.Cost;
        }
      }
      #endregion

      #region  Result
      return sumDeliveredItemPrice
             + deliveredTransportPrice
             + ((sumNotDeliveredItemPrice * siteSettings.Value.OrderSettings.MinimumPaymentPercentage) / 100)
             + (notDeliveredTransportPrice * 2);
      #endregion
    }
    public async Task<OrderPayment> GetOrderPaymentById(int id, CancellationToken cancellationToken, Include<OrderPayment> include = null)
    {
      return await orderPaymentRepository.GetAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken, include: include);
    }
    #endregion

    #region CheckSetPayment
    public async Task CheckOrderPaymentVisit(int orderPaymentId, CancellationToken cancellationToken)
    {
      int userId = workContext.GetCurrentUserId();
      var orderPayment = await orderPaymentRepository.GetAsync(predicate: x => x.Id == orderPaymentId &&
                                                                          x.Visited == false &&
                                                                          x.Order.Cart.UserId == userId,
                                                                          cancellationToken: cancellationToken);
      if (orderPayment == null)
      {
        throw errorFactory.InvalidPaymentPreview();
      }
    }

    public async Task SetOrderPaymentVisit(int orderPaymentId, CancellationToken cancellationToken)
    {
      int userId = workContext.GetCurrentUserId();
      var orderPayment = await orderPaymentRepository.GetAsync(predicate: x => x.Id == orderPaymentId &&
                                                                          x.Visited == false &&
                                                                          x.Order.Cart.UserId == userId,
                                                                          cancellationToken: cancellationToken);

      if (orderPayment == null)
      {
        throw errorFactory.InvalidPaymentPreview();
      }
      orderPayment.Visited = true;
      await orderPaymentRepository.UpdateAsync(entity: orderPayment, cancellationToken: cancellationToken);
    }

    #endregion

  }
}