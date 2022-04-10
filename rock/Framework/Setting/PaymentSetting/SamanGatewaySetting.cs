namespace rock.Framework.Setting.PaymentSetting
{
  public class SamanGatewaySetting
  {
    public string TerminalId { get; set; }
    public string PayUrl { get; set; }
    public string VerifyTransactionUrl { get; set; }
    public string ReverseTransactionUrl { get; set; }
    public string CallBackUrl { get; set; }
  }
}