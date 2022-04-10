using System;
using rock.Models.CommonApi;
namespace rock.Models.ProductApi
{
  public class ProductCategoryBrandModel
  {
    public int ProductCategoryId { get; internal set; }
    public int BrandId { get; set; }
    public BrandModel Brand { get; internal set; }
  }
}