using System;

namespace rock.Models.PaymentApi
{
  public class PaymentGatewayModel
  {
    public string Gateway { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public Guid ImageId { get; set; }
    public int FinancialAccountId { get; set; }
    public string FinancialAccountTitle { get; set; }
    public string BankName { get; set; }
    public string FistName { get; set; }
    public string LastName { get; set; }
    public byte[] ImageRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}