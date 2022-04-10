using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Models.ProductApi;
namespace rock.Models.ShopApi
{
  public class ShopStuffModel
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public string BrandName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public bool Status { get; set; }
    public int? VariationNumber { get; set; }
    public ProductImageModel PreviewProductImage { get; set; }
  }
}