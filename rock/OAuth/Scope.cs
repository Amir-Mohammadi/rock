using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using rock.Core.Domains.Users;
namespace rock.OAuth
{
  public class Scopes
  {
    public const string ROCK_PROFILE_READ = "rock.profile.read";
    public const string ROCK_PROFILE_MANGE = "rock.profile.manage";
    public const string ROCK_USER_READ = "rock.user.read";
    public const string ROCK_USER_MANGE = "rock.user.manage";
    public static readonly Dictionary<string, UserRole[]> Map = new()
    {
      [Scopes.ROCK_PROFILE_READ] = new UserRole[] { UserRole.Customer, UserRole.Admin },
      [Scopes.ROCK_PROFILE_MANGE] = new UserRole[] { UserRole.Customer },
      [Scopes.ROCK_USER_READ] = new UserRole[] { UserRole.Customer },
      [Scopes.ROCK_USER_MANGE] = new UserRole[] { UserRole.Customer }
    };
  }
}