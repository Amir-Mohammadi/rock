using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Framework.Autofac;
namespace rock.Core.Services.Shops
{
  public interface IShopStuffService : IScopedDependency
  {
    IQueryable<ShopStuff> GetShopStuffs(int? shopId = null, int? brandId = null, int? stuffId = null, int? categoryId = null, string q = null, bool showArchive = true, IInclude<ShopStuff> include = null);
    Task<ShopStuff> GetShopStuff(int shopId, int stuffId, CancellationToken cancellationToken, IInclude<ShopStuff> include = null);
    Task<ShopStuff> InsertShopStuff(ShopStuff shopStuff, Shop shop, Product stuff, CancellationToken cancellationToken);
    Task DeleteShopStuff(ShopStuff shopStuff, CancellationToken cancellationToken);
    Task RemoveShopStuff(ShopStuff shopStuff, CancellationToken cancellationToken);
    Task RestoreShopStuff(ShopStuff shopStuff, CancellationToken cancellationToken);
  }
}