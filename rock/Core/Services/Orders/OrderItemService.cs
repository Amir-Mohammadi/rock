using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Models.ShopApi;
namespace rock.Core.Services.Orders
{
  public class OrderItemService : IOrderItemService
  {
    private IRepository<OrderItem> orderItemRepository;
    public OrderItemService(IRepository<OrderItem> orderItemRepository)
    {
      this.orderItemRepository = orderItemRepository;
    }
    public Task DeleteOrderItem(OrderItem orderItem, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
    public Task<OrderItem> GetOrderItemById(int id, CancellationToken cancellationToken, IInclude<OrderItem> include = null)
    {
      throw new System.NotImplementedException();
    }
    public IQueryable<OrderItem> GetOrderItems(int? orderId = null,
                                               int? userId = null,
                                               OrderItemStatusType? orderItemStatus = null,
                                               ShopOrderSearchParameters parameters = null,
                                               OrderStatusType? orderStatus = null,
                                               int? shopId = null,
                                               DateTime? fromDateTime = null,
                                               DateTime? toDateTime = null,
                                               IInclude<OrderItem> include = null)
    {
      var orderItems = this.orderItemRepository.GetQuery(include: include);
      if (orderId != null)
        orderItems = orderItems.Where(x => x.OrderId == orderId);
      if (orderItemStatus != null)
        orderItems = orderItems.Where(x => x.LatestStatus.Type == orderItemStatus);
      if (orderStatus != null)
        orderItems = orderItems.Where(x => x.Order.LatestOrderStatus.OrderStatusType == orderStatus);
      if (shopId != null)
        orderItems = orderItems.Where(x => x.ShopId == shopId);
      if (fromDateTime != null)
        orderItems = orderItems.Where(x => x.Order.CreatedAt >= fromDateTime);
      if (toDateTime != null)
        orderItems = orderItems.Where(x => x.Order.CreatedAt <= toDateTime);
      if (userId != null)
        orderItems = orderItems.Where(x => x.Order.Cart.UserId == userId);
      if (parameters?.CategoryId != null)
        orderItems = orderItems.Where(x => x.CartItem.Product.ProductCategoryId == parameters.CategoryId);
      if (parameters?.BrandId != null)
        orderItems = orderItems.Where(x => x.CartItem.Product.BrandId == parameters.BrandId);
      if (parameters?.Status != null)
        orderItems = orderItems.Where(x => x.LatestStatus.Type == parameters.Status);
      if (!string.IsNullOrEmpty(parameters?.Q))
      {
        orderItems = orderItems.Where(
          x => x.CartItem.Product.Name.Contains(parameters.Q) ||
          x.CartItem.Product.Brand.Name.Contains(parameters.Q) ||
          x.CartItem.Product.ProductCategory.Name.Contains(parameters.Q) ||
          x.CartItem.Color.Name.Contains(parameters.Q)
          );
      }
      return orderItems;
    }
    public async Task InsertOrderItem(OrderItem orderItem, CancellationToken cancellationToken)
    {
      await orderItemRepository.AddAsync(entity: orderItem, cancellationToken: cancellationToken);
    }
    public Task PrepareOrderItem(OrderItem orderItem, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
    public Task SendOrderItem(OrderItem orderItem, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
    public async Task UpdateOrderItem(OrderItem orderItem, CancellationToken cancellationToken)
    {
      await orderItemRepository.UpdateAsync(entity: orderItem, cancellationToken: cancellationToken);
    }
  }
}