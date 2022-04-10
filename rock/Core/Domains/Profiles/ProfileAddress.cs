using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Framework.Models;

namespace rock.Core.Domains.Profiles
{
  public class ProfileAddress : IEntity, IHasDescription, IRemovable
  {
    public int Id { get; set; }
    public string Phone { get; set; }
    public string Description { get; set; }
    public string PostalCode { get; set; }
    public string AddressOwnerName { get; set; }
    public int ProfileId { get; set; }
    public int CityId { get; set; }
    public bool IsDefault { get; set; }
    public virtual City City { get; set; }
    public virtual Profile Profile { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public ICollection<Cart> Carts { get; set; }
  }
}
