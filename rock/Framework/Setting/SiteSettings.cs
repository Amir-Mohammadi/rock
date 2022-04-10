namespace rock.Framework.Setting
{
  public class SiteSettings
  {
    public TokenSettings TokenSettings { get; set; }
    public VerifyTokenSettings VerifyTokenSettings { get; set; }
    public GoogleReCaptchaSettings GoogleReCaptchaSetting { get; set; }
    public TaxSettings TaxSettings { get; set; }
    public DefaultLocation DefaultLocation { get; set; }
    public SMSServiceInfo SMSServiceInfo { get; set; }
    public OrderSettings OrderSettings { get; set; }
    public string Version { get; set; }
    public string CacheControl { get; set; }
    public string WithOrigins { get; set; }

  }
}