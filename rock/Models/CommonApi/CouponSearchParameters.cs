using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using rock.Filters;

namespace rock.Models.CommonApi
{
  public class CouponSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "name")]
    public string Name { get; set; }

  }
}