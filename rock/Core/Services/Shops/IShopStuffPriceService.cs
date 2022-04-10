
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Framework.Autofac;
namespace rock.Core.Services.Shops
{
  public interface IShopStuffPriceService : IScopedDependency
  {
    Task<ShopStuffPrice> InsertShopStuffPrice(Shop shop, ShopStuffPrice shopStuffPrice, ShopStuff shopStuff, ProductColor stuffColor, CancellationToken cancellationToken);
    Task RemoveShopStuffPrice(ShopStuffPrice shopStuffPrice, CancellationToken cancellationToken);
    IQueryable<ShopStuffPrice> GetShopStuffPrices(int? stuffId = null, int? shopId = null, int? cityId = null, int? colorId = null, bool showArchive = true, IInclude<ShopStuffPrice> include = null);
    Task<ShopStuffPrice> GetShopStuffPriceById(int id, CancellationToken cancellationToken, IInclude<ShopStuffPrice> include = null);
    Task<ShopStuffPrice> GetShopStuffPrice(int shopId, int stuffId, int colorId, CancellationToken cancellationToken, IInclude<ShopStuffPrice> include = null);
  }
}