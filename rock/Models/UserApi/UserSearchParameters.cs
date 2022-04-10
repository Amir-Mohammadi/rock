using Microsoft.AspNetCore.Mvc;
using rock.Core.Domains.Users;
using rock.Filters;
namespace rock.Models.UserApi
{
  public class UserSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "city_id")]
    public int? CityId { get; set; }
    [FromQuery(Name = "roles")]
    public UserRole? Roles { get; set; }
    [FromQuery(Name = "phone")]
    public string Phone { get; set; }
    [FromQuery(Name = "email")]
    public string Email { get; set; }
  }
}