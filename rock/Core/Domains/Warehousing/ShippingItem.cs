using System;
using rock.Core.Domains.Orders;
using rock.Framework.Models;

namespace rock.Core.Domains.Warehousing
{
  public class ShippingItem : IEntity
  {
    public int Id { get; set; }
    public int ShippingId { get; set; }
    public int OrderItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Shipping Shipping { get; set; }
    public virtual OrderItem OrderItem { get; set; }
  }
}
