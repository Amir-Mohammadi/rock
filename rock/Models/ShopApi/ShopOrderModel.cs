using System;
using rock.Models.ProductApi;
namespace rock.Models.ShopApi
{
  public class ShopOrderModel
  {
    public int Id { get; set; }
    public int ProductId  { get; set; }
    public ProductImageModel PreviewProductImage { get; set; }
    public string ProductName { get; set; }
    public string ProductCategoryName { get; set; }
    public string BrandName { get; set; }
    public string ColorName { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string Phone { get; set; }
    public byte[] RowVersion { get; set; }
  }
}