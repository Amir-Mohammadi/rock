using System;
using Microsoft.AspNetCore.Mvc;

namespace rock.Models.ShopApi
{
  public class ShopOrderStatisticsSearchParameter
  {
    [FromQuery(Name = "from_date_time")]
    public DateTime FromDateTime { get; set; }

    [FromQuery(Name = "to_date_time")]
    public DateTime ToDateTime { get; set; }
  }
}