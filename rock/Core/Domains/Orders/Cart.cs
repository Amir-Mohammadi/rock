using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Users;
using rock.Framework.Models;
namespace rock.Core.Domains.Orders
{
  public class Cart : IEntity, ITimestamp, IRemovable
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? ProfileAddressId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public CartStatus CartStatus { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual ProfileAddress ProfileAddress { get; set; }
    public virtual ICollection<CartItem> Items { get; set; }
    public virtual Order Order { get; set; }
  }
}