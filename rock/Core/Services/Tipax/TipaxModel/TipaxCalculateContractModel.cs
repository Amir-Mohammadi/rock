using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax.TipaxModel
{
  public class TipaxCalculateContractModel
  {
    public CalculateContractDispatchModel[] Dispatchs { get; set; }
    public double Price { get; set; }
    public int SenderCityID { get; set; }
    public int ReceiverCityID { get; set; }
  }
}
