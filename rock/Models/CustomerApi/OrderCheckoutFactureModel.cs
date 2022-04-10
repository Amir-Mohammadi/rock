namespace rock.Models.CustomerApi
{
  public class OrderCheckoutFactureModel
  {
    public double TotalPrice { get; set; }
    public double DiscountPrice { get; set; }
    public double ShippingPrice { get; set; }
    public double TaxPrice { get; set; }
    public double TotalPayPrice
    {
      get
      {
        return TotalPrice + ShippingPrice + TaxPrice - DiscountPrice;
      }
    }
  }
}