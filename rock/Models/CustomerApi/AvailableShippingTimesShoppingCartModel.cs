using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace rock.Models.CustomerApi
{
  public class AvailableShippingTimesShoppingCartModel
  {
    public ICollection<DateTime> AvailableShippingTimes { get; set; }
  }
}
