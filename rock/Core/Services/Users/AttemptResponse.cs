using rock.Core.Domains.Users;
using rock.Models.UserApi;

namespace rock.Core.Services.Users
{
  public class AttemptResponse : IAttemptResponse
  {

    public bool Success { get; set; }

    public User User { get; set; }
    public AuthenticateLoginType AuthenticationLoginType { get; set; }
    public AttemptReaction Reaction { get; set; }
    public string CodeReceiverContactInfo { get; set; }
    public AttemptType AttemptType { get; set; }


  }
}




