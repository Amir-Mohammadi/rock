using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
namespace rock.Core.Services.Shops
{
  public class ShopStuffService : IShopStuffService
  {
    #region Fields
    private readonly IRepository<ShopStuff> shopStuffRepository;
    #endregion
    #region Constactor
    public ShopStuffService(IRepository<ShopStuff> shopStuffRepository)
    {
      this.shopStuffRepository = shopStuffRepository;
    }
    #endregion
    #region ShopStuff
    public IQueryable<ShopStuff> GetShopStuffs(int? shopId, int? stuffId, int? categoryId, int? brandId, string q,
                                               bool showArchive = true, IInclude<ShopStuff> include = null)
    {
      var query = shopStuffRepository.GetQuery(include: include);
      if (shopId != null)
        query = query.Where(x => x.ShopId == shopId);
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (categoryId != null)
        query = query.Where(x => x.Stuff.ProductCategoryId == categoryId);
      if (brandId != null)
        query = query.Where(x => x.Stuff.BrandId == brandId);
      if (showArchive == false)
        query = query.Where(x => x.DeletedAt == null);

      if (!string.IsNullOrEmpty(q))
      {
        query = query.Where(
          x => x.Stuff.Name.Contains(q) ||
               x.Stuff.Brand.Name.Contains(q) ||
               x.Stuff.ProductCategory.Name.Contains(q)
        );
      }
      return query;
    }
    public async Task<ShopStuff> GetShopStuff(int shopId, int stuffId, CancellationToken cancellationToken,
                                              IInclude<ShopStuff> include = null)
    {
      var shopStuff = await shopStuffRepository.GetAsync(predicate: x => x.StuffId == stuffId && x.ShopId == shopId,
                                                         cancellationToken: cancellationToken,
                                                         include: include);
      return shopStuff;
    }
    public async Task<ShopStuff> InsertShopStuff(ShopStuff shopStuff, Shop shop, Product stuff,
                                                 CancellationToken cancellationToken)
    {
      shopStuff.Shop = shop;
      shopStuff.Stuff = stuff;
      await shopStuffRepository.AddAsync(entity: shopStuff,
                                         cancellationToken: cancellationToken);
      return shopStuff;
    }
    public async Task DeleteShopStuff(ShopStuff shopStuff, CancellationToken cancellationToken)
    {
      await shopStuffRepository.DeleteAsync(entity: shopStuff,
                                            cancellationToken: cancellationToken);
    }
    public async Task RemoveShopStuff(ShopStuff shopStuff, CancellationToken cancellationToken)
    {
      shopStuff.DeletedAt = DateTime.UtcNow;
      await shopStuffRepository.UpdateAsync(entity: shopStuff,
                                            cancellationToken: cancellationToken);
    }
    public async Task RestoreShopStuff(ShopStuff shopStuff, CancellationToken cancellationToken)
    {
      shopStuff.DeletedAt = null;
      await shopStuffRepository.UpdateAsync(entity: shopStuff,
                                            cancellationToken: cancellationToken);
    }
    #endregion
  }
}