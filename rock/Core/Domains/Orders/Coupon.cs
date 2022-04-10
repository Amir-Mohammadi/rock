using System;
using System.Collections.Generic;
using rock.Core.Domains.Payment;
using rock.Framework.Models;

namespace rock.Core.Domains.Orders
{
  public class Coupon : IEntity, ITimestamp
  {
    public Coupon()
    {
      MaxQuantitiesPerUser = 1;
    }
    public int Id { get; set; }
    public string CouponCode { get; set; }
    public int? MaxQuantities { get; set; }
    public int MaxQuantitiesPerUser { get; set; }
    public double Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime ExpiryDate { get; set; }
    public byte[] RowVersion { get; set; }
    public bool Active { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
  }
}