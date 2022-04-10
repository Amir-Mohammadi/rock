using System;
using rock.Core.Domains.Users;
using rock.Framework.Models;

namespace rock.Core.Domains.Orders
{
  public class OrderStatus : IEntity
  {
    public int Id { get; set; }
    public OrderStatusType OrderStatusType { get; set; }
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual Order Order { get; set; }
    public virtual User User { get; set; }
  }
}