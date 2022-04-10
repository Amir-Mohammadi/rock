using System;

namespace rock.Models.PaymentApi
{
  public class BriefPaymentGatewayModel
  {
    public string Gateway { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public Guid ImageId { get; set; }
    public byte[] ImageRowVersion { get; set; }
  }
}