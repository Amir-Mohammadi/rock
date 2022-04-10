using rock.Models.CommonApi;

namespace rock.Models.CustomerApi
{
  public class ShoppingCouponModel
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public double Value { get; set; }
    public CouponValidateStatus Status { get; set; }
  }
}