using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Shops;
using rock.Core.Services.Common;
using rock.Core.Services.Orders;
using rock.Core.Services.Products;
using rock.Core.Services.Shops;
using rock.Core.Services.Users;
using rock.Core.Services.Warehousing;
using rock.Factories;
using rock.Models;
using rock.Models.ShopApi;
using rock.Models.WarehousingApi;

namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [Authorize]
  public class ShopAdminApiController : BaseController
  {
    #region Fields
    private readonly IOrderItemService orderItemService;
    private readonly IShopService shopService;
    private readonly IShopProfileService shopProfileService;
    private readonly IShopFactory shopFactory;
    private readonly IShopStuffService shopStuffService;
    private readonly IShopStuffPriceService shopStuffPriceService;
    private readonly IProductService productService;
    private readonly IWarehouseService warehouseService;
    private readonly ICommonService commonService;
    private readonly IUserService userService;
    private readonly IAuthService authService;
    #endregion
    #region Constractor
    public ShopAdminApiController(IOrderItemService orderItemService,
                                  IShopService shopService,
                                  IShopFactory shopFactory,
                                  IShopStuffService stuffService,
                                  IShopStuffPriceService stuffPriceService,
                                  IProductService productService,
                                  IWarehouseService warehouseService,
                                  ICommonService commonService,
                                  IUserService userService,
                                  IAuthService authService,
                                  IShopProfileService shopProfileService)
    {
      this.orderItemService = orderItemService;
      this.shopService = shopService;
      this.shopFactory = shopFactory;
      this.shopStuffService = stuffService;
      this.shopStuffPriceService = stuffPriceService;
      this.productService = productService;
      this.warehouseService = warehouseService;
      this.commonService = commonService;
      this.userService = userService;
      this.authService = authService;
      this.shopProfileService = shopProfileService;
    }
    #endregion
    #region Shop
    [HttpPost("shops")]
    public async Task<Key<int>> CreateShop([FromBody] ShopModel model, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: model.OwnerId,
                                               cancellationToken: cancellationToken);

      var shop = new Shop();
      var shopProfile = new ShopProfile();
      mapShop(shop, model);
      mapShopProfile(shopProfile, model);
      var createShopResult = await shopService.CreateShop(shop: shop,
                                                          owner: user,
                                                          shopProfile: shopProfile,
                                                          cancellationToken: cancellationToken);
      return new Key<int>(createShopResult.Id);
    }
    private void mapShop(Shop shop, ShopModel model)
    {
      shop.Name = model.Name;
      shop.Active = model.Active;
      shop.CityId = model.CityId;
      shop.RowVersion = model.RowVersion;
    }
    private void mapShopProfile(ShopProfile shopProfile, ShopModel model)
    {
      shopProfile.Address = model.Address;
      shopProfile.PostalCode = model.PostalCode;
      shopProfile.Website = model.Website;
      shopProfile.Telephone = model.Telephone;
      shopProfile.RowVersion = model.RowVersion;
    }
    [HttpGet("shops/{shopId}")]
    public async Task<ShopModel> GeShop([FromRoute] int shopId, CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareShopModel(shopId: shopId,
                                                cancellationToken: cancellationToken);
    }
    [HttpGet("shops")]
    public async Task<IPagedList<ShopModel>> GeShops([FromQuery] ShopSearchParameters parameters,
                                                     CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareShopListModel(parameters: parameters,
                                                    cancellationToken: cancellationToken);
    }
    [HttpPut("shops/{shopId}")]
    public async Task EditShop([FromRoute] int shopId, [FromBody] ShopModel model, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId,
                                               cancellationToken: cancellationToken);
      var shopProfile = await shopProfileService.GetShopProfileByShopId(shopId: shop.Id,
                                                                        cancellationToken: cancellationToken);
      mapShop(shop, model);
      if (shopProfile == null) shopProfile = new ShopProfile();
      mapShopProfile(shopProfile, model);
      await shopService.UpdateShop(shop: shop,
                                   shopProfile: shopProfile,
                                   cancellationToken: cancellationToken);
    }
    [HttpPost("shops/{shopId}/change-owner")]
    public Task ChangeShopOwner([FromRoute] int shopId, [FromBody] EditShopOwnerModel model,
                                CancellationToken cancellationToken)
    {
      return null; // TODO if user role is merchant, then revert user role to customer
    }
    [HttpPost("shops/{shopId}/activate")]
    public async Task Activate([FromRoute] int shopId, [FromHeader] byte[] rowVersion,
                               CancellationToken cancellationToken)
    {
      var shop = await this.shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      shop.RowVersion = rowVersion;
      await shopService.ActivateShop(shop: shop,
                                     cancellationToken: cancellationToken);
    }
    [HttpPost("shops/{shopId}/deactivate")]
    public async Task Deactivate([FromRoute] int shopId, [FromHeader] byte[] rowVersion,
                                 CancellationToken cancellationToken)
    {
      var shop = await this.shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      shop.RowVersion = rowVersion;
      await shopService.DeactivateShop(shop: shop,
                                       cancellationToken: cancellationToken);
    }
    #endregion
    #region ShopStuff
    [HttpPost("shops/{shopId}/stuffs")]
    public async Task CreateShopStuff([FromRoute] int shopId, ShopStuffModel model, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId,
                                               cancellationToken: cancellationToken);
      var product = await productService.GetProductById(id: model.StuffId,
                                                        cancellationToken: cancellationToken);
      var shopStuff = new ShopStuff();
      await shopStuffService.InsertShopStuff(shopStuff: shopStuff,
                                             shop: shop,
                                             stuff: product,
                                             cancellationToken: cancellationToken);
    }
    [HttpGet("shops/{shopId}/stuffs/{stuffId}")]
    public async Task<ShopStuffModel> GetShopStuff([FromRoute] int shopId, [FromRoute] int stuffId,
                                                   CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId,
                                               cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopStuffModel(shopId: shop.Id,
                                                     stuffId: stuffId,
                                                     cancellationToken: cancellationToken);
    }
    [HttpDelete("shops/{shopId}/stuffs/{stuffId}")]
    public async Task DeleteShopStuff([FromRoute] int shopId, [FromRoute] int stuffId,
                                      CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId,
                                               cancellationToken: cancellationToken);
      var shopStuff = await shopStuffService.GetShopStuff(shopId: shop.Id,
                                                          stuffId: stuffId,
                                                          cancellationToken: cancellationToken);
      await shopStuffService.RemoveShopStuff(shopStuff: shopStuff,
                                             cancellationToken: cancellationToken);
    }
    [HttpGet("shops/{shopId}/stuffs")]
    public async Task<IPagedList<ShopStuffModel>> GetPagedShopStuffs([FromRoute] int shopId,
                                                                     [FromQuery] ShopStuffSearchParameters parameters,
                                                                     CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId,
                                               cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopStuffPagedListModel(shopId: shop.Id,
                                                              parameters: parameters,
                                                              cancellationToken: cancellationToken);
    }
    #endregion
    #region ShopStuffInventory
    [HttpGet("shops/{shopId}/inventory")]
    public async Task<IPagedList<ShopInventoryModel>> GetPagedShopInventory(
      [FromRoute] int shopId, InventorySearchParameters parameters, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopInventoryPagedListModel(shopId: shop.Id,
                                                                  parameters: parameters,
                                                                  cancellationToken: cancellationToken);
    }
    [HttpPost("shops/{shopId}/stuffs/{stuffId}/inventory/increase")]
    public async Task IncreaseShopInventory([FromRoute] int shopId, [FromRoute] int stuffId,
                                            [FromBody] ShopInventoryAdjustmentModel adjustment,
                                            CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      var stuff = await productService.GetProductById(id: stuffId, cancellationToken: cancellationToken);
      var color = await this.commonService.GetColorById(id: adjustment.ColorId, cancellationToken: cancellationToken);
      await shopService.IncreaseShopInventory(shop: shop,
                                              stuff: stuff,
                                              color: color,
                                              amount: adjustment.Quantity,
                                              cancellationToken: cancellationToken);
    }
    [HttpPost("shops/{shopId}/stuffs/{stuffId}/inventory/decrease")]
    public async Task DecreaseShopInventory([FromRoute] int shopId, [FromRoute] int stuffId,
                                            [FromBody] ShopInventoryAdjustmentModel adjustment,
                                            CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      var stuff = await productService.GetProductById(id: stuffId, cancellationToken: cancellationToken);
      var color = await this.commonService.GetColorById(id: adjustment.ColorId, cancellationToken: cancellationToken);
      await shopService.DecreaseShopInventory(shop: shop,
                                              stuff: stuff,
                                              color: color,
                                              amount: adjustment.Quantity,
                                              cancellationToken: cancellationToken);
    }
    [HttpGet("shops/{shopId}/inventory/documents")]
    public async Task<IPagedList<ShopDocumentModel>> GetPagedShopInventoryDocuments(
      [FromRoute] int shopId, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      return await this.shopFactory.PrepareShopDocumentListModel(shopId: shop.Id,
                                                                 cancellationToken: cancellationToken);
    }
    #endregion
    #region ShopOrder
    [HttpGet("shops/{shopId}/shop-orders")]
    public async Task<IPagedList<ShopOrderModel>> GetPagedShopOrders([FromRoute] int shopId,
                                                                     [FromQuery] ShopOrderSearchParameters parameters,
                                                                     CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopOrderListModel(shopId: shop.Id,
                                                         parameters: parameters,
                                                         cancellationToken: cancellationToken);
    }
    [HttpGet("shops/{shopId}/shop-orders/{shopOrderId}")]
    public async Task<ShopOrderModel> GetShopByIdOrder([FromRoute] int shopId, [FromRoute] int shopOrderId,
                                                       CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopOrderModel(shopId: shop.Id,
                                                     orderId: shopOrderId,
                                                     cancellationToken: cancellationToken);
    }
    [HttpPost("shops/{shopId}/shop-orders/{shopOrderId}/send")]
    public async Task SendShopOrder([FromRoute] int shopId, [FromRoute] int shopOrderId,
                                    CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      var orderItem = await orderItemService.GetOrderItemById(id: shopOrderId,
                                                              cancellationToken: cancellationToken);
      await orderItemService.SendOrderItem(orderItem: orderItem, cancellationToken: cancellationToken);
    }
    [HttpPost("shops/{shopId}/shop-orders/{shopOrderId}/prepare")]
    public async Task PrepareShopOrder([FromRoute] int shopId, [FromRoute] int shopOrderId,
                                       CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      var orderItem = await orderItemService.GetOrderItemById(id: shopOrderId,
                                                              cancellationToken: cancellationToken);
      await orderItemService.PrepareOrderItem(orderItem: orderItem,
                                              cancellationToken: cancellationToken);
    }
    #endregion
    #region ShopFinancial
    [HttpGet("shops/{shopId}/financial/wallet")]
    public async Task<ShopFinancialWalletModel> GetShopByIdFinancialStatus(
      [FromRoute] int shopId, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopFinancialWalletModel(shopId: shop.Id,
                                                               cancellationToken: cancellationToken);
    }
    [HttpGet("shops/{shopId}/financial/documents")]
    public async Task<IPagedList<ShopDocumentModel>> GetPagedShopFinancialDocuments(
      [FromRoute] int shopId, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopDocumentListModel(shopId: shop.Id,
                                                            cancellationToken: cancellationToken);
    }
    [HttpPut("shops/{shopId}/stuffs/{stuffId}/price")]
    public async Task CreateShopStuffPrice([FromRoute] int shopId, [FromRoute] int stuffId,
                                           ShopStuffPriceAdjustmentModel adjustment,
                                           CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken,
                                               include: new Include<Shop>(query =>
                                               {
                                                 query = query.Include(x => x.Owner).ThenInclude(x => x.Profile);
                                                 return query;
                                               }));
      var shopStuff = await shopStuffService.GetShopStuff(shopId: shop.Id,
                                                          stuffId: stuffId,
                                                          cancellationToken: cancellationToken);
      var stuffColor = await productService.GetProductColorById(productId: shopStuff.StuffId,
                                                                colorId: adjustment.ColorId,
                                                                cancellationToken: cancellationToken);
      var shopStuffPrice = new ShopStuffPrice();
      shopStuffPrice.Price = adjustment.Price;
      // this.assertPrice(shopStuffPrice);
      await shopStuffPriceService.InsertShopStuffPrice(shopStuffPrice: shopStuffPrice,
                                                       shopStuff: shopStuff,
                                                       stuffColor: stuffColor,
                                                       shop: shop,
                                                       cancellationToken: cancellationToken);
    }
    #endregion
  }
}