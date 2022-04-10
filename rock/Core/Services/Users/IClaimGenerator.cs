using System.Collections.Generic;
using rock.Framework.Autofac;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;
using rock.Core.Domains.Users;

namespace rock.Core.Services.Users
{
  public interface IClaimGenerator : IScopedDependency
  {
    Claim[] Create(User user, string securityStamp, Claim[] extraClaims = null);
  }
}