using System;
using System.Collections.Generic;
using rock.Framework.Models;

namespace rock.Core.Domains.Catalogs
{
  public class Catalog : IEntity
  {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual ICollection<CatalogItem> Items { get; set; }
    public virtual ICollection<CatalogMemory> CatalogMemories { get; set; }
  }
}