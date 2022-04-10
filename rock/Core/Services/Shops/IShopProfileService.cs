using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Shops;
using rock.Framework.Autofac;

namespace rock.Core.Services.Shops
{
  public interface IShopProfileService : IScopedDependency
  {
    Task<ShopProfile> CreateShopProfile(ShopProfile shopProfile, Shop shop, CancellationToken cancellationToken);
    Task<ShopProfile> UpdateShopProfile(ShopProfile shopProfile, CancellationToken cancellationToken);
    Task<ShopProfile> GetShopProfileById(int shopProfileId, CancellationToken cancellationToken, IInclude<ShopProfile> include = null);
    Task<ShopProfile> GetShopProfileByShopId(int shopId, CancellationToken cancellationToken, IInclude<ShopProfile> include = null);

  }
}