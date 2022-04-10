using rock.Core.Domains.Commons;
using rock.Framework.Models;
namespace rock.Core.Domains.Products
{
  public class ProductColor : IEntity
  {
    public int ColorId { get; set; }
    public int ProductId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Color Color { get; set; }
    public virtual Product Product { get; set; }
  }
}