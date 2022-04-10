
using System.Collections.Generic;
using rock.Framework.Autofac;
using rock.Core.Domains.Users;
using System.Threading;
using System.Threading.Tasks;
using System;
using rock.Models.UserApi;
using rock.OAuth;

namespace rock.Core.Services.Users
{
  public interface IAuthService : IScopedDependency
  {

    Task<VerifyModel> Authenticate(string loginId, CancellationToken cancellationToken);
    Task<LoginResult> VerifyAuthenticate(string password, string verificationToken, AuthenticateType authenticateType, CancellationToken cancellationToken);
    Task<LoginResult> Login(AuthGatewayType authGatewayType, string email, string password, CancellationToken cancellationToken);
    Task Logout();
    Task ChangePassword(string oldPassword, string newPassword, CancellationToken cancellationToken);
    Task<string> CreateToken(User user, CancellationToken cancellationToken);


  }
}