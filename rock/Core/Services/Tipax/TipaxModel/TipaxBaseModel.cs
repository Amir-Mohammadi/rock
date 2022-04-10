using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax.TipaxModel
{
  public class TipaxBaseModel
  {
    public string SystemToken { get; set; }
    public string UserToken { get; set; }
  }

  public class TipaxBaseModel<T> : TipaxBaseModel
  {
    public T Item { get; set; }
  }
}
