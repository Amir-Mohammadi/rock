using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace rock.Models.FinancialApi
{
  public class DocumentSearchParameters
  {
    [FromQuery(Name = "user_id")]
    public int UserId { get; set; }

    [FromQuery(Name = "create_time")]
    public DateTime CreateTime { get; set; }

    [FromQuery(Name = "form_id")]
    public int FormId { get; set; }
  }
}
