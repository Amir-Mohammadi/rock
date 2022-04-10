using System;

namespace rock.Models.ShopApi
{
  public class ShopInProgressOrderModel
  {
    public int OrderItemId { get; set; }
    public string ProductName { get; set; }
    public Guid ProductImageId { get; set; }
    public byte[] ProductImageRowVersion { get; set; }
    public string BrandName { get; set; }
    public string ColorName { get; set; }
    public int ColorCode { get; set; }
    public int ProductCount { get; set; }
    public string CustomerName { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Address { get; set; }

  }
}