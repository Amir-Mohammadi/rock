using System;

namespace rock.Models.CustomerApi
{
  public class OrderCheckoutCargoModel
  {
    public string ProductName { get; set; }
    public string ProductColorName { get; set; }
    public string ProductBrandName { get; set; }
    public DateTime? ShippingDateTime { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public int amount { get; set; }
  }
}