using rock.Framework.Models;
using rock.Core.Domains.Commons;
using System;

namespace rock.Core.Domains.Shops
{
  public class ShopProfile : IEntity, ITimestamp
  {
    public int Id { get; set; }
    public int ShopId { get; set; }
    public string PostalCode { get; set; }
    public string Telephone { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public virtual Shop Shop { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}