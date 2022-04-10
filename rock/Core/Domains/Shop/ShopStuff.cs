using System;
using System.Collections.Generic;
using rock.Core.Domains.Products;
using rock.Framework.Models;
namespace rock.Core.Domains.Shops
{
  public class ShopStuff : IEntity, IRemovable
  {
    public int StuffId { get; set; }
    public int ShopId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Product Stuff { get; set; }
    public virtual Shop Shop { get; set; }
    public virtual ICollection<ShopStuffPrice> ShopStuffPrices { get; set; }
  }
}