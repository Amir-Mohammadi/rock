
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using rock.Core.Domains.Users;
using rock.Framework.Setting;
namespace rock.OAuth
{
  public static class OAuthServiceExtention
  {
    private static void registerScope(this AuthorizationOptions options, string name, string issuer)
    {
      options.AddPolicy(name, policy =>
        {
          policy.Requirements.Add(new HasScopeRequirement(name, issuer));
        });
    }
    public static void AddOAuthAuthorization(this IServiceCollection services, TokenSettings token)
    {
      services.AddAuthorization(options =>
      {
        options.registerScope(Scopes.ROCK_PROFILE_READ, token.Issuer);
        options.registerScope(Scopes.ROCK_PROFILE_MANGE, token.Issuer);
        options.registerScope(Scopes.ROCK_USER_READ, token.Issuer);
        options.registerScope(Scopes.ROCK_USER_MANGE, token.Issuer);
      });
      services.AddSingleton<IAuthorizationHandler, HasScopeAuthorizationHandler>();
    }
  }
}