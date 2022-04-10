using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Users;
using rock.Framework.Models;
namespace rock.Models.OrderApi
{
  public class OrderCartModel 
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? CouponId { get; set; }
    public int? ProfileAddressId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public User User { get; set; }
    public ProfileAddress ProfileAddress { get; set; }
    public ICollection<OrderCartItemModel> Items { get; set; }
  }
}