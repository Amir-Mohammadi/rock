using System;

namespace rock.Models.UserApi
{
  public class VerifyAuthenticateModel
  {
    public AuthenticateType AuthenticateType { get; set; }
    public string Password { get; set; }
    public string VerificationToken { get; set; }
  }
}
