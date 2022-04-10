using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Warehousing;
using rock.Core.Domains.Products;
using rock.Core.Domains.Shops;
using rock.Core.Domains.Users;
using rock.Core.Services.Warehousing;
using rock.Core.Services.Products;
using rock.Core.Services.Common;
using rock.Core.Errors;
using rock.Core.Services.Users;
namespace rock.Core.Services.Shops
{
  public class ShopService : IShopService
  {
    #region Fields
    private readonly IRepository<Shop> shopRepository;
    private readonly IProductService productService;
    private readonly IShopProfileService shopProfileService;
    private readonly IShopStuffService shopStuffService;
    private readonly IWarehouseService warehouseService;
    private readonly IErrorFactory errors;
    private readonly IUserService userService;
    private readonly IWorkContext workContext;
    #endregion
    #region Constractor
    public ShopService(IRepository<Shop> shopRepository,
                       IProductService productService,
                       IShopStuffService shopStuffService,
                       IWarehouseService warehouseService,
                       IErrorFactory errors,
                       IWorkContext workContext,
                       IShopProfileService shopProfileService, IUserService userService)
    {
      this.workContext = workContext;
      this.errors = errors;
      this.shopRepository = shopRepository;
      this.productService = productService;
      this.shopStuffService = shopStuffService;
      this.warehouseService = warehouseService;
      this.shopProfileService = shopProfileService;
      this.userService = userService;
    }
    #endregion
    #region Shop
    public async Task<Shop> CreateShop(Shop shop, User owner, ShopProfile shopProfile,
                                       CancellationToken cancellationToken)
    {
      var warehouse = new Warehouse
      {
        Name = shop.Name
      };

      await warehouseService.InsertWarehouse(warehouse: warehouse,
                                             cancellationToken: cancellationToken);

      shop.Warehouse = warehouse;
      shop.Owner = owner;
      await shopRepository.AddAsync(entity: shop,
                                    cancellationToken: cancellationToken);

      if (owner.Role < UserRole.Merchant)
      {
        owner.Role = UserRole.Merchant; // update user role
        await this.userService.UpdateUser(user: owner, cancellationToken: cancellationToken);
      }

      await shopProfileService.CreateShopProfile(shopProfile: shopProfile,
                                                 shop: shop,
                                                 cancellationToken: cancellationToken);

      return shop;
    }
    public async Task UpdateShop(Shop shop, ShopProfile shopProfile, CancellationToken cancellationToken)
    {
      shop.Profile = shopProfile;
      await shopRepository.UpdateAsync(entity: shop,
                                       cancellationToken: cancellationToken);
    }
    public async Task ActivateShop(Shop shop, CancellationToken cancellationToken)
    {
      shop.Active = true;
      await shopRepository.UpdateAsync(entity: shop,
                                       cancellationToken: cancellationToken);
    }
    public async Task DeactivateShop(Shop shop, CancellationToken cancellationToken)
    {
      shop.Active = false;
      await shopRepository.UpdateAsync(entity: shop,
                                       cancellationToken: cancellationToken);
    }
    public async Task<Shop> GetShopById(int id, CancellationToken cancellationToken, IInclude<Shop> include = null)
    {
      var shop = await shopRepository.GetAsync(predicate: x => x.Id == id,
                                               cancellationToken: cancellationToken,
                                               include: include);
      return shop;
    }
    public async Task<Shop> GetCurrentUserShop(CancellationToken cancellationToken, IInclude<Shop> include = null)
    {
      var currentUserId = workContext.GetCurrentUserId();
      var shop = await shopRepository.GetAsync(predicate: x => x.OwnerId == currentUserId,
                                               cancellationToken: cancellationToken,
                                               include: include);
      return shop;
    }
    public IQueryable<Shop> GetShops(int? cityId = null, IInclude<Shop> include = null)
    {
      var query = shopRepository.GetQuery(include);
      if (cityId != null)
        query = query.Where(x => x.Owner.Profile.CityId == cityId);
      return query;
    }
    public async Task IncreaseShopInventory(Shop shop, Product stuff, Color color, int amount,
                                            CancellationToken cancellationToken)
    {
      await checkShopStuffColor(shop: shop,
                                stuff: stuff,
                                color: color,
                                cancellationToken: cancellationToken);
      if (shop.Warehouse == null)
        shopRepository.LoadReference(entity: shop,
                                     referenceProperty: x => x.Warehouse);
      await warehouseService.IncreaseShopInventory(warehouse: shop.Warehouse,
                                                   product: stuff,
                                                   color: color,
                                                   amount: amount,
                                                   cancellationToken: cancellationToken);
    }
    public async Task DecreaseShopInventory(Shop shop, Product stuff, Color color, int amount,
                                            CancellationToken cancellationToken)
    {
      await checkShopStuffColor(shop: shop,
                                stuff: stuff,
                                color: color,
                                cancellationToken: cancellationToken);
      if (shop.Warehouse == null)
        shopRepository.LoadReference(entity: shop,
                                     referenceProperty: x => x.Warehouse);
      var inventory = warehouseService.GetWarehouseInventory(warehouseId: shop.WarehouseId,
                                                             productId: stuff.Id,
                                                             colorId: color.Id).FirstOrDefault();
      decimal inventorAmount = 0;
      if (inventory != null)
      {
        inventorAmount = inventory.Amount;
      }
      if (inventorAmount < amount)
      {
        throw errors.LessThanMinimumInventoryAmount();
      }
      await warehouseService.DecreaseShopInventory(warehouse: shop.Warehouse,
                                                   product: stuff,
                                                   color: color,
                                                   amount: amount,
                                                   cancellationToken: cancellationToken);
    }
    private async Task checkShopStuffColor(Shop shop, Product stuff, Color color, CancellationToken cancellationToken)
    {
      var shopStuff = await shopStuffService.GetShopStuff(shopId: shop.Id,
                                                          stuffId: stuff.Id,
                                                          cancellationToken: cancellationToken);
      // if (shopStuff == null)
      // throw 
      var productColor = await productService.GetProductColorById(productId: stuff.Id,
                                                                  colorId: color.Id,
                                                                  cancellationToken: cancellationToken);
    }
    #endregion
  }
}