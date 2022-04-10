using rock.Framework.Models;

namespace rock.Core.Domains.Commons
{
  public class Color : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Code { get; set; }
    public byte[] RowVersion { get; set; }
  }
}