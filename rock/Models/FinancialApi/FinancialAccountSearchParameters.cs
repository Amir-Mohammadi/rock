using System;
using rock.Filters;

namespace rock.Models.FinancialApi
{
  public class FinancialAccountSearchParameters : PagedListFilter
  {
    public int? CurrencyId { get; set; }
    public int? BankId { get; set; }

  }
}
