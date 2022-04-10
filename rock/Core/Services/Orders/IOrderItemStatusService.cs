using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Orders;
using rock.Framework.Autofac;

namespace rock.Core.Services.Orders
{
  public interface IOrderItemStatusService : IScopedDependency
  {
    Task InsertOrderItemStatus(OrderItemStatus orderItemStatus, CancellationToken cancellationToken);
    Task UpdateOrderItemStatus(OrderItemStatus orderItemStatus, CancellationToken cancellationToken);
    Task DeleteOrderItemStatus(OrderItemStatus orderItemStatus, CancellationToken cancellationToken);
    Task<OrderItemStatus> GetOrderItemById(int Id, CancellationToken cancellationToken);
  }
}