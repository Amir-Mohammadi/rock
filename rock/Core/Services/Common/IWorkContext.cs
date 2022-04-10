using System;
using rock.Framework.Autofac;
namespace rock.Core.Services.Common
{
  public interface IWorkContext : IScopedDependency
  {
    // Task<User> GetCurrentUser(CancellationToken cancellationToken);
    bool IsAuthenticated();
    int GetCurrentUserId();
    int GetCurrentCityId();
    DateTime GetUserTokenExpiration();
    string GetUserToken();
  }
}