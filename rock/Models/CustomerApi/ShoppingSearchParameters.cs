using System;
using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.CustomerApi
{
  public class ShoppingSearchParameters : PagedListFilter
  {

    [FromQuery(Name = "tracking_id")]
    public int TrackingId { get; set; }
    [FromQuery(Name = "purchased_at")]
    public DateTime PurchasedAt { get; set; }
    [FromQuery(Name = "status")]
    public ShoppingStatus Status { get; set; }
  }
}
