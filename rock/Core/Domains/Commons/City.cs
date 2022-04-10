using System;
using rock.Framework.Models;

namespace rock.Core.Domains.Commons
{
  public class City : IEntity
  {
    public int Id { get; set; }
    public int ProvinceId { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Province Province { get; set; }
  }
}
