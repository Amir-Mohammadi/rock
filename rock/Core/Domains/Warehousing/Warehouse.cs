using rock.Framework.Models;

namespace rock.Core.Domains.Warehousing
{
  public class Warehouse : IEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }
    
    public byte[] RowVersion { get; set; }
  }
}