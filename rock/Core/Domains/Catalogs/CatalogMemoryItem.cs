using rock.Framework.Models;
namespace rock.Core.Domains.Catalogs
{
  public class CatalogMemoryItem : IEntity
  {
    public int Id { get; set; }
    public int CatalogMemoryId { get; set; }
    public int CatalogItemId { get; set; }
    public string Value { get; set; }
    public string ExtraKey { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CatalogMemory CatalogMemory { get; set; }
    public virtual CatalogItem CatalogItem { get; set; }
  }
}