using System;
using System.Collections.Generic;
using rock.Core.Domains.Financial;
using rock.Framework.Models;

namespace rock.Core.Domains.Commons
{
  public class Currency : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public double Ratio { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
