using System.Data.Common;
using rock.Framework.Models;

namespace rock.Core.Domains.Commons
{
  public class Static : IEntity
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public StaticType StaticType { get; set; }
  }
}