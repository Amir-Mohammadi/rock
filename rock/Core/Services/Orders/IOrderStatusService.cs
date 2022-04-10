using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Orders;
using rock.Framework.Autofac;

namespace rock.Core.Services.Orders
{
  public interface IOrderStatusService : IScopedDependency
  {
    Task InsertOrderStatus(OrderStatus orderStatus, CancellationToken cancellationToken);
    Task UpdateOrderStatus(OrderStatus orderStatus, CancellationToken cancellationToken);
  }
}