using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Services.Common;

namespace rock.Core.Services.Orders
{
  public class OrderStatusService : IOrderStatusService
  {
    private IRepository<OrderStatus> orderStatusRepository;
    private readonly IWorkContext workContext;
    public OrderStatusService(IRepository<OrderStatus> orderStatusRepository, IWorkContext workContext)
    {
      this.workContext = workContext;
      this.orderStatusRepository = orderStatusRepository;
    }

    public async Task InsertOrderStatus(OrderStatus orderStatus, CancellationToken cancellationToken)
    {
      orderStatus.CreatedAt = DateTime.UtcNow;
      orderStatus.UserId = workContext.GetCurrentUserId();
      await orderStatusRepository.AddAsync(entity: orderStatus, cancellationToken: cancellationToken);
    }

    public async Task UpdateOrderStatus(OrderStatus orderStatus, CancellationToken cancellationToken)
    {
      await orderStatusRepository.UpdateAsync(entity: orderStatus, cancellationToken: cancellationToken);
    }
  }
}