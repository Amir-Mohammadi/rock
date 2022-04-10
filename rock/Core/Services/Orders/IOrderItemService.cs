using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Framework.Autofac;
using rock.Models.ShopApi;
namespace rock.Core.Services.Orders
{
  public interface IOrderItemService : IScopedDependency
  {
    Task InsertOrderItem(OrderItem orderItem, CancellationToken cancellationToken);
    Task UpdateOrderItem(OrderItem orderItem, CancellationToken cancellationToken);
    Task DeleteOrderItem(OrderItem orderItem, CancellationToken cancellationToken);
    Task<OrderItem> GetOrderItemById(int id, CancellationToken cancellationToken, IInclude<OrderItem> include = null);
    IQueryable<OrderItem> GetOrderItems(int? orderId = null,
                                        int? userId = null,
                                        OrderItemStatusType? orderItemStatus = null,
                                        ShopOrderSearchParameters parameters = null,
                                        OrderStatusType? orderStatus = null,
                                        int? shopId = null,
                                        DateTime? fromDateTime = null,
                                        DateTime? toDateTime = null,
                                        IInclude<OrderItem> include = null);
    Task SendOrderItem(OrderItem orderItem, CancellationToken cancellationToken);
    Task PrepareOrderItem(OrderItem orderItem, CancellationToken cancellationToken);
  }
}