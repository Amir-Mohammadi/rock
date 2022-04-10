using System.Collections.Generic;
using rock.Framework.Models;
namespace rock.Core.Domains.Commons
{
  public class Province : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int AreaCode { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<City> Cities { get; set; }
  }
}