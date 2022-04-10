using System;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Framework.Models;
namespace rock.Core.Domains.Orders
{
  public class CartItem : IEntity
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Nullable<int> ShopStuffPriceId { get; set; }
    public int ProductPriceId { get; set; }
    public int CartId { get; set; }
    public int ColorId { get; set; }
    public int Amount { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Product Product { get; set; }
    public ShopStuffPrice StuffPrice { get; set; }
    public ProductPrice ProductPrice { get; set; }
    public virtual Cart Cart { get; set; }
    public virtual Color Color { get; set; }
    public virtual ProductColor ProductColor { get; set; }
  }
}