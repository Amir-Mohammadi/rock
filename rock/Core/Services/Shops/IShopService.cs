using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Core.Domains.Users;
using rock.Framework.Autofac;
namespace rock.Core.Services.Shops
{
  public interface IShopService : IScopedDependency
  {
    Task UpdateShop(Shop shop, ShopProfile shopProfile, CancellationToken cancellationToken);
    Task ActivateShop(Shop shop, CancellationToken cancellationToken);
    Task DeactivateShop(Shop shop, CancellationToken cancellationToken);
    Task<Shop> GetShopById(int id, CancellationToken cancellationToken, IInclude<Shop> include = null);
    IQueryable<Shop> GetShops(int? cityId = null, IInclude<Shop> include = null);
    Task<Shop> GetCurrentUserShop(CancellationToken cancellationToken, IInclude<Shop> include = null);
    Task<Shop> CreateShop(Shop shop, User owner, ShopProfile shopProfile, CancellationToken cancellationToken);
    Task IncreaseShopInventory(Shop shop, Product stuff, Color color, int amount, CancellationToken cancellationToken);
    Task DecreaseShopInventory(Shop shop, Product stuff, Color color, int amount, CancellationToken cancellationToken);
  }
}