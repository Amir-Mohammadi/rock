using System;
using Microsoft.AspNetCore.Mvc;

namespace rock.Models.FinancialApi
{
    public class PurchaseSearchParamters
    {
      [FromQuery(Name="user_id")]
      public int UserId { get; set; }

    }
}
