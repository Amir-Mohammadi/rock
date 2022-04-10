using System;
using System.Threading.Tasks;
using rock.Core.Domains.Users;
using rock.Framework.Autofac;

namespace rock.Core.Services.Users
{
  public interface ITokenManagerService : IScopedDependency
  {
    Task<bool> SetLoginUserSecurityStamp(string userId, string securityStamp);
    Task<bool> IsActiveToken();
    Task<bool> CheckUserSecurityStamp(string userId, string hashValue);
    Task<bool> DeactiveToken(string token, DateTime expireDate);
    string GenerateSecurityStamp(User user);

    string ValidateVerificationToken(string verificationToken);
  }
}