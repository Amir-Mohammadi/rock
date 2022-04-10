using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.ShopApi
{
  public class ShopFinancialTransactionsSearchParameter : PagedListFilter
  {
    [FromQuery(Name = "transaction_date")]
    public DateTime? TransactionDate { get; set; }
  }
}