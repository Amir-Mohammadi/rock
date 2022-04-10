namespace rock.Models.CustomerApi
{
  public class CartBillModel
  {
    public int TotalPrice { get; set; }
    public int TotalShippingPrice { get; set; }
    public int TotalDiscountPrice { get; set; }
    public int TotalTaxPrice { get; set; }
    public int TotalPayPrice
    {
      get
      {
        return TotalPrice + TotalShippingPrice + TotalTaxPrice - TotalDiscountPrice;
      }
    }
  }
}