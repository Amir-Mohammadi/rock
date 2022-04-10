using rock.Core.Domains.Commons;
using rock.Framework.Models;
namespace rock.Core.Domains.Products
{
  public class ProductCategoryBrand : IEntity
  {
    public int BrandId { get; set; }
    public int ProductCategoryId { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual ProductCategory ProductCategory { get; set; }
    public byte[] RowVersion { get; set; }
  }
}