using System;
using rock.Framework.Models;
namespace rock.Core.Domains.Orders
{
  public class OrderItemStatus : IEntity, IRemovable, IHasDescription
  {
    public int Id { get; set; }
    public OrderItemStatusType Type { get; set; }
    public int OrderItemId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual OrderItem OrderItem { get; set; }
  }
}