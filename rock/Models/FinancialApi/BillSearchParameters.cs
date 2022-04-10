using System;
using Microsoft.AspNetCore.Mvc;

namespace rock.Models.FinancialApi
{
  public class BillSearchParameters
  {
    [FromQuery(Name = "document_id")]
    public int DocumentId { get; set; }

  }
}
