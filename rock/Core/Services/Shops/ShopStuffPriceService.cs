
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Core.Errors;
namespace rock.Core.Services.Shops
{
  public class ShopStuffPriceService : IShopStuffPriceService
  {
    #region Fields
    private readonly IRepository<ShopStuffPrice> shopStuffRepository;
    private readonly IRepository<ProductPrice> productPriceRepository;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public ShopStuffPriceService(
      IRepository<ShopStuffPrice> shopStuffRepository,
      IRepository<ProductPrice> productPriceRepository,
      IErrorFactory errorFactory)
    {
      this.shopStuffRepository = shopStuffRepository;
      this.productPriceRepository = productPriceRepository;
      this.errorFactory = errorFactory;
    }
    #endregion
    #region ShopStuffPrice
    public async Task<ShopStuffPrice> InsertShopStuffPrice(Shop shop, ShopStuffPrice shopStuffPrice, ShopStuff shopStuff, ProductColor stuffColor, CancellationToken cancellationToken)
    {
      //TODO remove old price
      var ownerCityId = shop.CityId;
      var productPrice = await productPriceRepository.GetQuery().Where(
        m => m.CityId == ownerCityId &&
        m.ProductId == shopStuff.StuffId &&
        m.ColorId == stuffColor.ColorId &&
        m.IsPublished == true)
        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
      if (productPrice != null)
      {
        if (!(shopStuffPrice.Price <= productPrice.MaxPrice && shopStuffPrice.Price >= productPrice.MinPrice))
        {
          throw errorFactory.ShopStuffPriceIsOutOfRange();
        }
      }
      var currentStuffPrice = await shopStuffRepository.GetQuery().Where(
        m => m.ShopId == shop.Id &&
        m.StuffId == shopStuffPrice.StuffId &&
        m.ColorId == shopStuffPrice.ColorId)
        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
      if (currentStuffPrice != null)
      {
        currentStuffPrice.DeletedAt = DateTime.Now;
        await shopStuffRepository.UpdateAsync(currentStuffPrice, cancellationToken: cancellationToken);
      }
      shopStuffPrice.ShopStuff = shopStuff;
      shopStuffPrice.StuffColor = stuffColor;
      shopStuffPrice.CreatedAt = DateTime.UtcNow;
      await shopStuffRepository.AddAsync(entity: shopStuffPrice,
                                         cancellationToken: cancellationToken);
      return shopStuffPrice;
    }
    public async Task RemoveShopStuffPrice(ShopStuffPrice shopStuffPrice, CancellationToken cancellationToken)
    {
      shopStuffPrice.DeletedAt = DateTime.UtcNow;
      await shopStuffRepository.UpdateAsync(entity: shopStuffPrice,
                                            cancellationToken: cancellationToken);
    }
    public async Task<ShopStuffPrice> GetShopStuffPriceById(int id, CancellationToken cancellationToken, IInclude<ShopStuffPrice> include = null)
    {
      var result = await shopStuffRepository.GetAsync(predicate: x => x.Id == id,
                                                      cancellationToken: cancellationToken,
                                                      include: include);
      return result;
    }
    public async Task<ShopStuffPrice> GetShopStuffPrice(int shopId, int stuffId, int colorId, CancellationToken cancellationToken, IInclude<ShopStuffPrice> include = null)
    {
      var result = await shopStuffRepository.GetAsync(predicate: x => x.ShopId == shopId & x.StuffId == stuffId & x.ColorId == colorId & x.DeletedAt == null,
                                                      cancellationToken: cancellationToken,
                                                      include: include);
      return result;
    }
    public IQueryable<ShopStuffPrice> GetShopStuffPrices(int? stuffId = null, int? shopId = null, int? cityId = null, int? colorId = null, bool showArchive = true, IInclude<ShopStuffPrice> include = null)
    {
      var query = shopStuffRepository.GetQuery(include: include);
      if (shopId != null)
        query = query.Where(x => x.ShopId == shopId);
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (colorId != null)
        query = query.Where(x => x.ColorId == colorId);
      if (cityId != null)
        query = query.Where(x => x.Shop.Owner.Profile.CityId == cityId);
      if (showArchive == false)
        query = query.Where(x => x.DeletedAt == null);
      return query;
    }
    #endregion
  }
}