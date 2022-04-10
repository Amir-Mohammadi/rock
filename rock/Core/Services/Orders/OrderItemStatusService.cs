using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Orders;

namespace rock.Core.Services.Orders
{
  public class OrderItemStatusService : IOrderItemStatusService
  {
    private IRepository<OrderItemStatus> orderItemStatusRepository;
    public OrderItemStatusService(IRepository<OrderItemStatus> orderItemStatusRepository)
    {
      this.orderItemStatusRepository = orderItemStatusRepository;
    }
    public async Task DeleteOrderItemStatus(OrderItemStatus orderItemStatus, CancellationToken cancellationToken)
    {
      orderItemStatus.DeletedAt = DateTime.UtcNow;
      await orderItemStatusRepository.UpdateAsync(entity: orderItemStatus, cancellationToken: cancellationToken);
    }

    public async Task<OrderItemStatus> GetOrderItemById(int id, CancellationToken cancellationToken)
    {
      return await orderItemStatusRepository.GetAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task InsertOrderItemStatus(OrderItemStatus orderItemStatus, CancellationToken cancellationToken)
    {
      orderItemStatus.CreatedAt = DateTime.UtcNow;
      await orderItemStatusRepository.AddAsync(entity: orderItemStatus, cancellationToken: cancellationToken);
    }

    public async Task UpdateOrderItemStatus(OrderItemStatus orderItemStatus, CancellationToken cancellationToken)
    {
      await orderItemStatusRepository.UpdateAsync(entity: orderItemStatus, cancellationToken: cancellationToken);
    }
  }
}