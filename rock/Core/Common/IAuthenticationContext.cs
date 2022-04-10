using rock.Core.Domains.Users;

namespace rock.Core.Common
{
  public interface IAuthenticatonConetxt
  {
    bool IsGuest();
    bool IsAuthenticated();
    User GetUser();
  }
}