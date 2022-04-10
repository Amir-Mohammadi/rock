using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.CommonApi
{
  public class TransportationSearchParameters : PagedListFilter
  {
    public int? FromCityId { get; set; }
    public int? ToCityId { get; set; }
  }
}