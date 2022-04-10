using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.PaymentApi
{
  public class PaymentGatewaySearchParameters : PagedListFilter
  {
    [FromQuery(Name = "gateway")]
    public string Gateway { get; set; }
  }
}