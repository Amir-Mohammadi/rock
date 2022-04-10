using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Framework.Models;
namespace rock.Core.Domains.Orders
{
  public class Transport : IEntity, ITimestamp, IHasDescription
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int FromCityId { get; set; }
    public int ToCityId { get; set; }
    public int Cost { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Description { get; set; }
    public string TrackingCode { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Order Order { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual City FromCity { get; set; }
    public virtual City ToCity { get; set; }
  }
}