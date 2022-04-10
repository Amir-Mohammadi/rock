using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Services.Common;
using rock.Models.CommonApi;

namespace rock.Core.Services.Orders
{
  public class OrderService : IOrderService
  {
    #region Fields
    private IRepository<Order> orderRepository;
    private IRepository<Transport> transportRepository;
    private readonly IWorkContext workContext;
    #endregion
    #region Constractor
    public OrderService(IRepository<Order> orderRepository,
                        IRepository<Transport> transportRepository,
                        IWorkContext workContext)
    {
      this.orderRepository = orderRepository;
      this.transportRepository = transportRepository;
      this.workContext = workContext;
    }
    #endregion
    #region  Order
    public async Task InsertOrder(Order order, CancellationToken cancellationToken)
    {
      order.CreatedAt = DateTime.UtcNow;
      await orderRepository.AddAsync(entity: order, cancellationToken: cancellationToken);
    }

    public IQueryable<Order> GetOrders(IInclude<Order> include = null)
    {
      var query = orderRepository.GetQuery(include);
      return query;
    }


    public async Task<Order> GetOrderById(int id, CancellationToken cancellationToken, IInclude<Order> include = null)
    {

      var order = await this.orderRepository.GetAsync(predicate: x => x.Id == id,
                                                      cancellationToken: cancellationToken,
                                                      include: include);
      return order;

    }

    public async Task UpdateOrder(Order order, CancellationToken cancellationToken)
    {
      await orderRepository.UpdateAsync(entity: order, cancellationToken: cancellationToken);
    }
    #endregion

    #region  Transport
    public async Task<Transport> InsertTransport(Transport transport, CancellationToken cancellationToken)
    {

      transport.CreatedAt = DateTime.UtcNow;
      await transportRepository.AddAsync(entity: transport, cancellationToken: cancellationToken);
      return transport;
    }

    public async Task UpdateTransport(Transport transport, CancellationToken cancellationToken)
    {
      transport.UpdatedAt = DateTime.UtcNow;
      await transportRepository.UpdateAsync(entity: transport, cancellationToken: cancellationToken);
    }

    public async Task<Transport> GetTransportById(int transportId, CancellationToken cancellationToken, Include<Transport> include = null)
    {
      return await transportRepository.GetAsync(predicate: x => x.Id == transportId, cancellationToken: cancellationToken, include: include);
    }

    public async Task<int> GetCouponUsed(CancellationToken cancellationToken, int couponId, int? userId = null)
    {
      var couponUsed = orderRepository.GetQuery(include: null).Where(predicate: x => (
         x.CouponId == couponId &&
         x.Cart.CartStatus == Domains.Orders.CartStatus.Permanent &&
         x.Expiration >= DateTime.UtcNow));

      if (userId != null)
      {
        couponUsed = couponUsed.Where(x => x.Cart.UserId == userId.Value);
      }
      return await couponUsed.CountAsync(cancellationToken: cancellationToken);
    }

    public async Task<CouponValidateStatus> DefineCouponStatus(Coupon coupon, CancellationToken cancellationToken)
    {

      var status = CouponValidateStatus.Available;

      var couponUsedCount = await GetCouponUsed(couponId: coupon.Id,
                                                userId: workContext.GetCurrentUserId(),
                                                cancellationToken: cancellationToken);

      if (!coupon.Active)
      {
        status = CouponValidateStatus.Deactive;
      }
      if (coupon.ExpiryDate <= DateTime.UtcNow)
      {
        status = CouponValidateStatus.Expired;
      }
      if (coupon.MaxQuantities != null)
      {
        var usage = await GetCouponUsed(couponId: coupon.Id,
                                        userId: null,
                                        cancellationToken: cancellationToken);

        if (coupon.MaxQuantities == usage)
        {
          status = CouponValidateStatus.Finished;
        }

      }
      if (couponUsedCount > coupon.MaxQuantitiesPerUser)
      {
        status = CouponValidateStatus.Used;
      }
      return status;
    }


    #endregion
  }
}