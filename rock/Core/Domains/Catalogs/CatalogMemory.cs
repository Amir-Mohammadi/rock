using System;
using System.Collections.Generic;
using rock.Framework.Models;

namespace rock.Core.Domains.Catalogs
{
  public class CatalogMemory : IEntity
  {
    public int Id { get; set; }
    public int CatalogId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Catalog Catalog { get; set; }
    public virtual ICollection<CatalogMemoryItem> Items { get; set; }

  }
}