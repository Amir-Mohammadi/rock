using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;
namespace rock.Core.Services.Users
{
  public interface ITokenGenerator : IScopedDependency
  {
    Task<string> Create(Claim[] claims, CancellationToken cancellationToken, long? lifetime = null);
    Task<string> CreateVerifyToken(string context);
  }
}