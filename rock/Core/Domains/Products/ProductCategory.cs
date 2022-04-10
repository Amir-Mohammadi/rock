using rock.Framework.Models;
using System;
using System.Collections.Generic;
using rock.Core.Domains.Catalogs;
namespace rock.Core.Domains.Products
{
  public class ProductCategory : IEntity, ITimestamp, IRemovable, ISeoFriendly
  {
    public int Id { get; set; }
    public int? ParentId { get; set; } // if there is no parent, then this is a root node
    public int CatalogId { get; set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public string Explanation { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsPublished { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Catalog Catalog { get; set; }
    public virtual ProductCategory Parent { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<ProductCategory> Children { get; set; }
    public virtual ICollection<ProductCategoryBrand> ProductCategoryBrands { get; set; }
  }
}