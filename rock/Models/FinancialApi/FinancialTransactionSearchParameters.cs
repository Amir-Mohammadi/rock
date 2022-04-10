using System;
using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.FinancialApi
{
  public class FinancialTransactionSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "bank_id")]
    public int BankId { get; set; }

    [FromQuery(Name = "title")]
    public string Title { get; set; }
  }
}
