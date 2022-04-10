using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.ShopApi
{
  public class StuffSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "stuff_name")]
    public string StuffName { get; set; }
    [FromQuery(Name = "brand_id")]
    public int? BrandId { get; set; }
    [FromQuery(Name = "category_id")]
    public int? CategoryId { get; set; }

  }
}