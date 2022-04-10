using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Products;
using rock.Core.Domains.Profiles;
using rock.Core.Services.Tipax;

namespace rock.Core.Services.Orders
{
  public class TransportService : ITransportService
  {
    private readonly ITipaxService tipaxService;
    private readonly IOrderService orderService;
    public TransportService(ITipaxService tipaxService, IOrderService orderService)
    {
      this.tipaxService = tipaxService;
      this.orderService = orderService;
    }
    public async Task<int> CalculateShippingPrice(List<CartItem> cartItems, ProfileAddress profileAddress, TransportType transportType, CancellationToken cancellationToken)
    {
      int shippingCost = 0;
      switch (transportType)
      {
        case TransportType.Tipax:
          shippingCost = await tipaxService.CalculateContractPrice(cartItems: cartItems, profileAddress: profileAddress, cancellationToken: cancellationToken);
          break;
        default:
          shippingCost = 0;
          break;
      };

      return shippingCost;
    }

    public async Task RegisterShipping(int orderId, TransportType transportType, CancellationToken cancellationToken)
    {
      var order = await orderService.GetOrderById(id: orderId, cancellationToken: cancellationToken,
                 include: new Include<Order>(query =>
                 {
                   query = query.Include(x => x.Items).ThenInclude(x => x.Shop).ThenInclude(x => x.Owner);
                   query = query.Include(x => x.Cart.ProfileAddress);
                   query = query.Include(x => x.Cart.ProfileAddress).ThenInclude(x => x.City);
                   query = query.Include(x => x.Items);
                   query = query.Include(x => x.Items).ThenInclude(x => x.CartItem.Product).ThenInclude(x => x.ProductShippingInfo);
                   query = query.Include(x => x.Items).ThenInclude(x => x.CartItem.ProductPrice);
                   query = query.Include(x => x.Items).ThenInclude(x => x.CartItem.ProductPrice).ThenInclude(x => x.City);
                   return query;
                 }));

      var availableOrders = order.Items.Where(x => x.ShopId != null);
    }
    public async Task<int> CalculateTipaxShippingPirce(Product product, ProfileAddress profileAddress, CancellationToken cancellationToken)
    {
      return await Task.FromResult(10000);
    }

    public async Task<TransportResult> RegisterTipaxShipping(Product product, ProfileAddress profileAddress, CancellationToken cancellationToken)
    {
      var transportResult = new TransportResult();
      transportResult.TotalCost = 10000;
      transportResult.TrackingCode = "test_tracking_code";
      return await Task.FromResult(transportResult);

    }
  }
}