using System;
using rock.Core.Domains.Products;
namespace rock.Models.ProductApi
{
  public class ProductModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public ProductColorModel DefaultProductColor { get; set; }
    public ProductImageModel PreviewProductImage { get; set; }
    public byte[] RowVersion { get; set; }
    public string BriefDescription { get;  set; }
    public string BrandName { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
  }
}