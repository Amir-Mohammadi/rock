using System;
using System.Collections.Generic;
using rock.Core.Domains.Documents;
using rock.Core.Domains.Payment;
using rock.Framework.Models;
namespace rock.Core.Domains.Orders
{
  public class Order : IEntity, ITimestamp
  {

    public int Id { get; set; }
    public int? DocumentId { get; set; }
    public int CartId { get; set; }
    public Nullable<int> LatestOrderStatusId { get; set; }
    public Nullable<int> CouponId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime Expiration { get; set; }
    public int Tax { get; set; }
    public int TotalAmount { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Cart Cart { get; set; }
    public virtual Document Document { get; set; }
    public virtual Coupon Coupon { get; set; }
    public virtual ICollection<OrderItem> Items { get; set; }
    public virtual ICollection<Transport> Transports { get; set; }
    public virtual ICollection<OrderPayment> OrderPayments { get; set; }
    public virtual ICollection<OrderStatus> OrderStatuses { get; set; }
    public virtual OrderStatus LatestOrderStatus { get; set; }
  }
}