using System;
using System.Collections.Generic;
using rock.Core.Domains.Files;
using rock.Core.Domains.Products;
using rock.Core.Domains.Profiles;
using rock.Framework.Models;
namespace rock.Core.Domains.Commons
{
  public class Brand : IEntity, IHasImage, ISeoFriendly, IHasDescription
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public string Description { get; set; }
    public int? ProfileId { get; set; }
    public Guid ImageId { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Profile Profile { get; set; }
    public virtual File Image { get; set; }
    public virtual ICollection<ProductCategoryBrand> ProductCategoryBrands { get; set; }
  }
}