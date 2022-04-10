using System;
using System.Collections.Generic;
using rock.Framework.Models;
using rock.Core.Domains.Catalogs;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Files;
using rock.Core.Domains.Threads;
using rock.Core.Domains.Shops;
namespace rock.Core.Domains.Products
{
  public class Product : IEntity, IRemovable, ISeoFriendly
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public string BriefDescription { get; set; }
    public int ProductCategoryId { get; set; }
    public int CatalogMemoryId { get; set; }
    public int BrandId { get; set; }
    public int ThreadId { get; set; }
    public int? DefaultColorId { get; set; }
    public int? PreviewProductImageId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductColor DefaultProductColor { get; set; }
    public ProductImage PreviewProductImage { get; set; }
    public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    public virtual CatalogMemory CatalogMemory { get; set; }
    public virtual ProductCategory ProductCategory { get; set; }
    public virtual ProductBrochure ProductBrochure { get; set; }
    public virtual ICollection<ProductImage> ProductImages { get; set; }
    public virtual ICollection<ProductColor> ProductColors { get; set; }
    public virtual ICollection<ShopStuffPrice> ShopStuffPrices { get; set; }
    public virtual ICollection<ShopStuff> ShopStuffs { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual Thread Thread { get; set; }
    public virtual ProductShippingInfo ProductShippingInfo { get; set; }
  }
}