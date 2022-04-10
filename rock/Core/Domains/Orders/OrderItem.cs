using System.Collections.Generic;
using rock.Framework.Models;
using rock.Core.Domains.Shops;
using System;

namespace rock.Core.Domains.Orders
{
  public class OrderItem : IEntity
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Nullable<int> LatestOrderItemStatusId { get; set; }
    public int? TransportId { get; set; }
    public int? ShopId { get; set; } //each item can be sell by a specific shop
    public int CartItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Order Order { get; set; }
    public virtual Shop Shop { get; set; }
    public virtual CartItem CartItem { get; set; }
    public virtual Transport Transport { get; set; }
    public virtual ICollection<OrderItemStatus> Statuses { get; set; }
    public virtual OrderItemStatus LatestStatus { get; set; }
  }
}