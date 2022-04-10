using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using rock.Filters;

namespace rock.Models.OrderApi
{
  public class OrderSearchParameters : PagedListFilter
  {
    [FromQuery(Name="create_at")]
    public DateTime CreateAt { get; set; }
    [FromQuery(Name="updated_at")]
    public DateTime UpdateAt { get; set; }
  }
}
