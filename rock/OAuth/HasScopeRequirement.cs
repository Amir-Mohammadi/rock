using System;
using Microsoft.AspNetCore.Authorization;
using rock.Core.Domains.Users;
namespace rock.OAuth
{
  public class HasScopeRequirement : IAuthorizationRequirement
  {
    public string Issuer { get; }
    public string Scope { get; }

    public HasScopeRequirement(string scope, string issuer)
    {
      Scope = scope ?? throw new ArgumentNullException(nameof(scope));
      Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
    }
  }
}
