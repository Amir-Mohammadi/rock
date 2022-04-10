using System.Collections.Generic;
using rock.Framework.Models;
namespace rock.Core.Domains.Catalogs
{
  public class CatalogItem : IEntity
  {
    public int Id { get; set; }
    public CatalogItemType Type { get; set; }
    public int Order { get; set; }
    public bool IsMain { get; set; }
    public bool HasMultiple { get; set; }
    public string Value { get; set; }
    public bool ShowInFilter { get; set; }
    public int CatalogId { get; set; }
    public int? ReferenceId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CatalogItem Reference { get; set; }
    public virtual Catalog Catalog { get; set; }
    public virtual ICollection<CatalogItem> Children { get; set; }
    public virtual ICollection<CatalogMemoryItem> CatalogMemoryItems { get; set; }
  }
}