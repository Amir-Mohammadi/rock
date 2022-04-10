namespace rock.Models.UserApi
{
  public class VerifyModel
  {
    public string Token { get; set; }
    public AuthenticateType AuthenticateType { get; set; }
    public AuthenticateLoginType AuthenticateLoginType { get; set; }
    public bool IsOptionalAuthenticateType { get; set; }
  }
}