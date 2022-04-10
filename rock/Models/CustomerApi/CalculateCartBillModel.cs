using Microsoft.AspNetCore.Mvc;

namespace rock.Models.CustomerApi
{
  public class CalculateCartBillModel
  {
    [FromQuery(Name = "coupon_code")]
    public string CouponCode { get; set; }
  }
}