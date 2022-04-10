using System;
using System.Collections.Generic;
using rock.Core.Domains.Documents;
using rock.Framework.Models;
namespace rock.Core.Domains.Warehousing
{
  public class Shipping : IEntity, IHasDescription
  {
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Document Document { get; set; }
    public virtual ICollection<ShippingItem> Items { get; set; }
  }
}