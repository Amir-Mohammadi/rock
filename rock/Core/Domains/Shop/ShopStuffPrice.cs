using System;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Products;
using rock.Framework.Models;
namespace rock.Core.Domains.Shops
{
  public class ShopStuffPrice : IEntity, IRemovable
  {
    public int Id { get; set; }
    public double Price { get; set; }
    public int ColorId { get; set; }
    public int ShopId { get; set; }
    public int StuffId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Product Stuff { get; set; }
    public virtual Shop Shop { get; set; }
    public virtual ShopStuff ShopStuff { get; set; }
    public virtual Color Color { get; set; }
    public virtual ProductColor StuffColor { get; set; }
  }
}