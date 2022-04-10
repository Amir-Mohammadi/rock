using System;
using rock.Core.Domains.Products;
using rock.Models.CommonApi;

namespace rock.Models.ProductApi
{
  public class ProductPriceModel
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public CityModel City { get; set; }
    public ColorModel Color { get; set; }
    public double Price { get; set; }
    public double MaxPrice { get; set; }
    public double MinPrice { get; set; }
    public double Discount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}