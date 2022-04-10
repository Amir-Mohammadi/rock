using rock.Core.Domains.Payment;

namespace rock.Models.CustomerApi
{
  public class OrderPaymentStatusModel
  {
    public int OrderId { get; set; }
    public int OrderPaymentId { get; set; }
    public PaymentStatus Status { get; set; }
  }
}