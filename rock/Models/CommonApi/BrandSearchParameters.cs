using Microsoft.AspNetCore.Mvc;
using rock.Filters;
using rock.Models.ProductApi;

namespace rock.Models.CommonApi
{
  public class BrandSearchParameters : PagedListFilter
  {
    [FromQuery]
    public int ProductCategoryId { get; set; }
  }
}