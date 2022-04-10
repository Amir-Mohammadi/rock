using rock.Framework.Models;
using rock.Core.Domains.Commons;
namespace rock.Core.Domains.Products
{
  public class ProductPrice : IEntity
  {
    public int Id { get; set; }
    public int CityId { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public double Price { get; set; }
    public double MaxPrice { get; set; }
    public double MinPrice { get; set; }
    public double Discount { get; set; }
    public bool IsPublished { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Product Product { get; set; }
    public virtual Color Color { get; set; }
    public virtual ProductColor ProductColor { get; set; }
    public virtual City City { get; set; }
  }
}