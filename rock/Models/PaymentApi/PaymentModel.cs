using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Services.Payment;

namespace rock.Models.PaymentApi
{
  public class PaymentModel
  {
    public string Gateway { get; set; }
    public string Kind { get; set; }
    public Dictionary<string, string> ExtraParams { get; set; }
  }
}
