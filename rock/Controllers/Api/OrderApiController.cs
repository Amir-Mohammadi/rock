using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Common;
using rock.Core.Domains.Orders;
using rock.Core.Services.Orders;
using rock.Factories;
using rock.Models.OrderApi;


namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [Authorize]
  public class OrderApiController : BaseController
  {
    private readonly ICartService cartService;
    private readonly IOrderService orderService;
    private readonly IOrderFactory factory;
    public OrderApiController(IOrderService orderService,
                              ICartService cartService,
                              IOrderFactory factory)
    {
      this.orderService = orderService;
      this.factory = factory;
      this.cartService = cartService;
    }

    [HttpGet("orders")]
    public async Task<IPagedList<GetOrderModel>> GetPagedOrders([FromQuery] OrderSearchParameters parameters, CancellationToken cancellationToken)
    {
      var result = await this.factory.PrepareOrderListModel(parameters: parameters, cancellationToken: cancellationToken);
      return result;
    }

    [HttpPost("orders")]
    public async Task<OrderModel> CreateOrder(CancellationToken cancellationToken)
    {
      var cart = await cartService.GetOrCreateCart(cancellationToken: cancellationToken);
      var order = this.mapOrder(cart: cart);
      // order = await orderService.InsertOrder(order: order, cartItems: cart.Items, cancellationToken: cancellationToken);
      var orderModel = this.mapOrderModel(order: order);
      return orderModel;
    }

    [HttpGet("orders/{orderId}")]
    public Task<GetOrderModel> GetOrder([FromRoute] int orderId, CancellationToken cancellationToken)
    {
      return this.factory.PrepareOrderModel(orderId, cancellationToken);
    }

    [HttpGet("orders/{orderId}/detail")]
    public Task<OrderDetailModel> GetOrderDetail([FromRoute] int orderId, CancellationToken cancellationToken)
    {
      return this.factory.PrepareOrderDetailModel(orderId, cancellationToken);
    }

    [HttpGet("orders/{orderId}/items")]
    public Task<IList<OrderItemModel>> GetOrderItem([FromRoute] int orderId, CancellationToken cancellationToken)
    {
      return this.factory.PrepareOrderItemModel(orderId, cancellationToken);
    }

    [HttpPost("orders/{orderId}/statuses")]
    public void CreateOrderStatus([FromRoute] int orderId, OrderStatusModel status)
    {
      // var order = this.orderService.GetOrderById(orderId);
      // var orderItem = this.orderService.GetOrderItemById(status.OrderItemId);
      // var orderItemStatus = new OrderItemStatus();
      // orderItemStatus.Type = status.Type;
      // this.orderService.InsertOrderItemStatus(order, orderItem, orderItemStatus);
    }

    #region  Privates
    private Order mapOrder(Cart cart)
    {
      var order = new Order();
      order.CartId = cart.Id;
      order.CreatedAt = DateTime.UtcNow;
      return order;
    }

    private OrderModel mapOrderModel(Order order)
    {
      var orderModel = new OrderModel();
      orderModel.Id = order.Id;
      orderModel.RowVersion = order.RowVersion;
      return orderModel;
    }
    #endregion
  }
}