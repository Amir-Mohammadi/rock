using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using rock.Filters;

namespace rock.Models.CustomerApi
{
  public class CustomerDocumentsSearchParameters : PagedListFilter
  {

    [FromQuery(Name = "from")]
    public DateTime FromDate { get; set; }

    [FromQuery(Name = "to")]
    public DateTime ToDate { get; set; }

  }
}
