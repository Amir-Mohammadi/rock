
using System.Collections.Generic;
using rock.Framework.Autofac;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Users;
using rock.Models.UserApi;

namespace rock.Core.Services.Users
{
  public interface IAuthenticator : IScopedDependency
  {
    Task<IAttemptResponse> AuthenticateAttempt(string loginId, CancellationToken cancellationToken);
    Task SendCode(string context, CancellationToken cancellationToken);
    AuthenticateLoginType ValidateLoginId(string loginId);
    Task<User> VerifyPhone(string phone, string password, AuthenticateType authenticateType, CancellationToken cancellationToken);
    Task<User> VerifyEmail(string email, string password, CancellationToken cancellationToken);
  }
}