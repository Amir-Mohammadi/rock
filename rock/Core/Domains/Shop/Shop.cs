using System;
using System.Collections.Generic;
using rock.Core.Domains.Warehousing;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Users;
using rock.Framework.Models;
using rock.Core.Domains.Commons;

namespace rock.Core.Domains.Shops
{
  public class Shop : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int OwnerId { get; set; }
    public int CityId { get; set; }
    public int WarehouseId { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ShopStuff> ShopStuffs { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual City City { get; set; }
    public virtual User Owner { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual ShopProfile Profile { get; set; }

  }
}