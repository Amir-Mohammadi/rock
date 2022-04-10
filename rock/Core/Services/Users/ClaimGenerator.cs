using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;
using rock.Core.Data;
using rock.Core.Domains.Users;
using Microsoft.EntityFrameworkCore;
using rock.OAuth;
using rock.Core.Extensions;
using System.Linq;
namespace rock.Core.Services.Users
{
  public class ClaimGenerator : IClaimGenerator
  {
    private IRepository<User> userRepository;
    public ClaimGenerator(IRepository<User> userRepository)
    {
      this.userRepository = userRepository;
    }
    public Claim[] Create(User user, string securityStamp, Claim[] extraClaims = null)
    {
      var claims = new List<Claim>();
      if (extraClaims != null)
      {
        foreach (var extraClaim in extraClaims)
        {
          claims.Add(extraClaim);
        }
      }
      claims.Add(new Claim("user-id", user.Id.ToString()));
      claims.Add(new Claim("security-stamp", securityStamp));
      var scopeList = from map in Scopes.Map
                       from role in map.Value
                       where role == user.Role
                       select map.Key;                       
      claims.Add(new Claim("scope", string.Join(" ", scopeList)));
      return claims.ToArray();
    }
  }
}