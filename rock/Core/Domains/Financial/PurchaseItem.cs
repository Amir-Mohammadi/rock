using System;
using rock.Core.Domains.Orders;
using rock.Framework.Models;

namespace rock.Core.Domains.Financial
{
  public class PurchaseItem : IEntity
  {
    public int Id { get; set; }
    public int PurchaseId { get; set; }
    public int OrderItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Purchase Purchase { get; set; }
    public virtual OrderItem OrderItem { get; set; }
  }
}
