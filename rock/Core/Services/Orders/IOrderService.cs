using System;
using System.Collections.Generic;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Core.Domains.Orders;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Models.CommonApi;

namespace rock.Core.Services.Orders
{
  public interface IOrderService : IScopedDependency
  {
    #region  Order
    Task InsertOrder(Order order, CancellationToken cancellationToken);
    IQueryable<Order> GetOrders(IInclude<Order> include = null);
    Task<Order> GetOrderById(int id, CancellationToken cancellationToken, IInclude<Order> include = null);
    Task UpdateOrder(Order order, CancellationToken cancellationToken);
    // Task DeleteOrder(Order entity, CancellationToken cancellationToken);
    // void InsertOrderItemStatus(Order order, OrderItem orderItem, OrderItemStatus entity);
    // void UpdateOrderItemStatus(OrderItemStatus entity, byte[] rowVersion);
    // void DeleteOrderItemStatus(OrderItemStatus entity);
    // OrderItemStatus GetOrderItemStatusById(int Id);
    // void InsertOrderItem(OrderItem entity);
    // void UpdateOrderItem(OrderItem entity, byte[] rowVersion);
    // void DeleteOrderItem(OrderItem entity);
    // OrderItem GetOrderItemById(int Id);
    #endregion

    #region  Transport
    Task<Transport> InsertTransport(Transport transport, CancellationToken cancellationToken);
    #endregion

    #region  Coupon
    Task<int> GetCouponUsed(CancellationToken cancellationToken, int couponId, int? userId = null);
    Task<CouponValidateStatus> DefineCouponStatus(Coupon coupon, CancellationToken cancellationToken);
    #endregion
  }
}