using rock.Core.Domains.Users;
using rock.Models.UserApi;

namespace rock.Core.Services.Users
{
  public interface IAttemptResponse
  {
    bool Success { get; set; }
    User User { get; set; }
    AuthenticateLoginType AuthenticationLoginType { get; set; }
    AttemptReaction Reaction { get; set; }
    AttemptType AttemptType { get; set; }
    string CodeReceiverContactInfo { get; set; }

  }
}