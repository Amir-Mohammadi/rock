using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using rock.Core.Data;
using rock.Core.Domains.Users;
using rock.Core.Errors;
using rock.Framework.Setting;
namespace rock.Core.Services.Common
{
  public class WorkContext : IWorkContext
  {
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IErrorFactory errors;
    private readonly IRepository<User> userRepository;
    private readonly IOptionsSnapshot<SiteSettings> siteSetting;
    public WorkContext(
      IHttpContextAccessor httpContextAccessor,
      IErrorFactory errors,
      IRepository<User> userRepository,
      IOptionsSnapshot<SiteSettings> siteSetting)
    {
      this.httpContextAccessor = httpContextAccessor;
      this.errors = errors;
      this.userRepository = userRepository;
      this.siteSetting = siteSetting;
    }
    private Claim getCurrentUserIdClaim()
    {
      return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "user-id");
    }
    public int GetCurrentCityId()
    {
      var defaultLocation = new DefaultLocation
      {
        CityId = siteSetting.Value.DefaultLocation.CityId,
        CityName = siteSetting.Value.DefaultLocation.CityName
      };
      var location = httpContextAccessor.HttpContext.Request.Cookies["default-location"];
      if (!string.IsNullOrEmpty(location) && !string.IsNullOrWhiteSpace(location))
      {
        defaultLocation = JsonConvert.DeserializeObject<DefaultLocation>(location);
      }
      else
      {
        httpContextAccessor.HttpContext.Response.Cookies.Append("default-location", JsonConvert.SerializeObject(defaultLocation));
      }
      return defaultLocation.CityId;
    }
    public async Task<User> GetCurrentUser(CancellationToken cancellationToken)
    {
      var userId = GetCurrentUserId();
      var user = await userRepository.GetAsync(x => x.Id == userId, cancellationToken);
      return user;
    }
    public int GetCurrentUserId()
    {
      var userIdClaim = getCurrentUserIdClaim();
      if (userIdClaim == null)
      {
        throw errors.AccessDenied();
      }
      try
      {
        return int.Parse(userIdClaim.Value);
      }
      catch (Exception)
      {
        throw errors.InvalidToken();
      }
    }
    public bool IsAuthenticated()
    {
      return getCurrentUserIdClaim()?.Value != null;
    }
    public DateTime GetUserTokenExpiration()
    {
      var exp = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "exp");
      DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
      return dtDateTime.AddSeconds(double.Parse(exp.Value));
    }
    public string GetUserToken()
    {
      string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
      return token.Substring(7, token.Length - 7);
    }
  }
}