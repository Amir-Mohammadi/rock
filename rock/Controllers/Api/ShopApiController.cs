using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Shops;
using System.Linq;
using rock.Core.Domains.Users;
using rock.Core.Errors;
using rock.Core.Services.Common;
using rock.Core.Services.Orders;
using rock.Core.Services.Products;
using rock.Core.Services.Shops;
using rock.Core.Services.Users;
using rock.Core.Services.Warehousing;
using rock.Factories;
using rock.Models.MarketApi;
using rock.Models.ShopApi;
using rock.Models.UserApi;
using rock.Models.WarehousingApi;
using rock.OAuth;

namespace rock.Controllers
{
  [Route("/api/v1/shops")]
  [ApiController]
  [Authorize]
  public class ShopApiController : BaseController
  {
    #region Fields
    private readonly IOrderItemService orderItemService;
    private readonly IUserService userService;
    private readonly IShopService shopService;
    private readonly IShopFactory shopFactory;
    private readonly IShopStuffService shopStuffService;
    private readonly IShopStuffPriceService shopStuffPriceService;
    private readonly IProductService productService;
    private readonly ICommonService commonService;
    private readonly IAuthService authService;
    private readonly IShopProfileService shopProfileService;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public ShopApiController(IOrderItemService orderItemService,
                             IShopService shopService,
                             IShopFactory shopFactory,
                             IShopStuffService stuffService,
                             IShopStuffPriceService stuffPriceService,
                             IProductService productService,
                             IWarehouseService warehouseService,
                             ICommonService commonService,
                             IAuthService authService,
                             IUserService userService,
                             IShopProfileService shopProfileService, IErrorFactory errorFactory)
    {
      this.orderItemService = orderItemService;
      this.shopService = shopService;
      this.shopFactory = shopFactory;
      this.shopStuffService = stuffService;
      this.shopStuffPriceService = stuffPriceService;
      this.productService = productService;
      this.commonService = commonService;
      this.authService = authService;
      this.shopProfileService = shopProfileService;
      this.errorFactory = errorFactory;
      this.userService = userService;
    }
    #endregion
    #region stuff
    [HttpGet("stuffs")]
    public async Task<IPagedList<StuffModel>> GetStuffs([FromQuery] StuffSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareStuffListModel(parameters: parameters, cancellationToken: cancellationToken);
    }
    #endregion
    #region MyShop
    [HttpGet("me")]
    public async Task<ShopModel> GeCurrentUserShop(CancellationToken cancellationToken)
    {
      return await this.shopFactory.PrepareCurrentShopModel(cancellationToken: cancellationToken);
    }
    [HttpPatch("me")]
    public async Task EditShop([FromBody] EditShopModel model, CancellationToken cancellationToken)
    {
      var shop = await this.shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      var shopProfile = await this.shopProfileService.GetShopProfileByShopId(shop.Id, cancellationToken);
      mapShop(shop, model);
      mapShopProfile(shopProfile, model);
      await shopService.UpdateShop(shop: shop,
                                   shopProfile: shopProfile,
                                   cancellationToken: cancellationToken);
    }
    [HttpPost("me/activate")]
    public async Task Activate([FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var shop = await this.shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      shop.RowVersion = rowVersion;
      await shopService.ActivateShop(shop: shop,
                                     cancellationToken: cancellationToken);
    }
    [HttpPost("me/deactivate")]
    public async Task Deactivate([FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var shop = await this.shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      shop.RowVersion = rowVersion;
      await shopService.DeactivateShop(shop: shop,
                                       cancellationToken: cancellationToken);
    }
    private void mapShop(Shop shop, EditShopModel model)
    {
      if (model.Name != null)
        shop.Name = model.Name;
      shop.RowVersion = model.RowVersion;
    }
    private void mapShopProfile(ShopProfile shopProfile, EditShopModel model)
    {
      if (model.Website != null)
        shopProfile.Website = model.Website;
      if (model.Address != null)
        shopProfile.Address = model.Address;
      if (model.Telephone != null)
        shopProfile.Telephone = model.Telephone;
    }
    #endregion
    #region MyShopStuff
    [HttpPost("me/stuffs")]
    public async Task CreateCurrentShopStuff([FromBody] CreateShopStuffModel model, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);

      var product = await productService.GetProductById(id: model.StuffId,
                                                        cancellationToken: cancellationToken);
      var shopStuff = new ShopStuff();
      await shopStuffService.InsertShopStuff(shopStuff: shopStuff,
                                             shop: currentShop,
                                             stuff: product,
                                             cancellationToken: cancellationToken);
    }
    [HttpGet("me/stuffs/{stuffId}")]
    public async Task<ShopStuffModel> GetCurrentShopStuff([FromRoute] int stuffId, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken);
      return await shopFactory.PrepareShopStuffModel(shopId: currentShop.Id,
                                                     stuffId: stuffId,
                                                     cancellationToken: cancellationToken);
    }
    [HttpDelete("me/stuffs/{stuffId:int}")]
    public async Task DeleteCurrentShopStuff([FromRoute] int stuffId, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken);
      var shopStuff = await shopStuffService.GetShopStuff(shopId: currentShop.Id,
                                                          stuffId: stuffId,
                                                          cancellationToken: cancellationToken);
      await shopStuffService.RemoveShopStuff(shopStuff: shopStuff,
                                             cancellationToken: cancellationToken);
    }
    [HttpGet("me/stuffs")]
    public async Task<IPagedList<ShopStuffModel>> GetPagedCurrentShopStuffs(
      [FromQuery] ShopStuffSearchParameters parameters, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken);
      return await shopFactory.PrepareShopStuffPagedListModel(shopId: currentShop.Id,
                                                              parameters: parameters,
                                                              cancellationToken: cancellationToken);
    }
    #endregion
    #region MyShopStuffInventory
    [HttpGet("me/inventory")]
    public async Task<IList<ShopInventoryModel>> GetCurrentPagedShopInventory(
      [FromQuery] InventorySearchParameters parameters, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopInventoryPagedListModel(shopId: currentShop.Id,
                                                                  parameters: parameters,
                                                                  cancellationToken: cancellationToken);
    }
    [HttpPost("me/stuffs/{stuffId}/inventory/increase")]
    public async Task IncreaseCurrentShopInventory([FromRoute] int stuffId,
                                                   [FromBody] ShopInventoryAdjustmentModel adjustment,
                                                   CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      var stuff = await productService.GetProductById(id: stuffId, cancellationToken: cancellationToken);
      var color = await this.commonService.GetColorById(id: adjustment.ColorId, cancellationToken: cancellationToken);
      await shopService.IncreaseShopInventory(shop: currentShop,
                                              stuff: stuff,
                                              color: color,
                                              amount: adjustment.Quantity,
                                              cancellationToken: cancellationToken);
    }
    [HttpPost("me/stuffs/{stuffId}/inventory/decrease")]
    public async Task DecreaseShopInventory([FromRoute] int stuffId, [FromBody] ShopInventoryAdjustmentModel adjustment,
                                            CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      var stuff = await productService.GetProductById(id: stuffId, cancellationToken: cancellationToken);
      var color = await this.commonService.GetColorById(id: adjustment.ColorId, cancellationToken: cancellationToken);
      await shopService.DecreaseShopInventory(shop: currentShop,
                                              stuff: stuff,
                                              color: color,
                                              amount: adjustment.Quantity,
                                              cancellationToken: cancellationToken);
    }
    [HttpGet("me/inventory/documents")]
    public async Task<IPagedList<ShopDocumentModel>> GetPagedCurrentShopDocuments(CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      return await this.shopFactory.PrepareShopDocumentListModel(shopId: currentShop.Id,
                                                                 cancellationToken: cancellationToken);
    }
    #endregion
    #region MyShopOrder
    [HttpGet("me/shop-orders")]
    public async Task<IPagedList<ShopOrderModel>> GetPagedCurrentShopOrders(
      [FromQuery] ShopOrderSearchParameters parameters, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopOrderListModel(shopId: currentShop.Id,
                                                         parameters: parameters,
                                                         cancellationToken: cancellationToken);
    }
    [HttpGet("me/shop-orders/{shopOrderId}")]
    public async Task<ShopOrderModel> GetCurrentShopOrder([FromRoute] int shopOrderId,
                                                          CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopOrderModel(shopId: currentShop.Id,
                                                     orderId: shopOrderId,
                                                     cancellationToken: cancellationToken);
    }
    [HttpPost("me/shop-orders/{shopOrderId:int}/send")]
    public async Task SendCurrentShopOrder([FromRoute] int shopOrderId, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      var orderItem = await orderItemService.GetOrderItemById(id: shopOrderId,
                                                              cancellationToken: cancellationToken);
      AssertShopHasOrderItem(shop: currentShop,
                             orderItem: orderItem);
      await orderItemService.SendOrderItem(orderItem: orderItem, cancellationToken: cancellationToken);
    }
    [HttpPost("me/shop-orders/{shopOrderId:int}/prepare")]
    public async Task PrepareCurrentShopOrder([FromRoute] int shopOrderId, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);

      var orderItem = await orderItemService.GetOrderItemById(id: shopOrderId,
                                                              cancellationToken: cancellationToken);
      AssertShopHasOrderItem(shop: currentShop,
                             orderItem: orderItem);
      await orderItemService.PrepareOrderItem(orderItem: orderItem,
                                              cancellationToken: cancellationToken);
    }
    private static void AssertShopHasOrderItem(Shop shop, OrderItem orderItem)
    {
      // if (!shop.OrderItems.Contains(orderItem))
      // {
      //   throw errors.ResourceNotFound(orderItem.Id);
      // }
    }
    #endregion
    #region MyShopFinancial
    [HttpGet("me/financial/wallet")]
    public async Task<ShopFinancialWalletModel> GetCurrentShopFinancialStatus(CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);
      return await shopFactory.PrepareShopFinancialWalletModel(shopId: currentShop.Id,
                                                               cancellationToken: cancellationToken);
    }
    [HttpGet("me/financial/account")]
    public async Task<ShopFinancialAccountModel> GetCurrentShopFinancialAccount(CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareShopFinancialAccountModel(cancellationToken: cancellationToken);
    }
    [HttpGet("me/financial/transactions")]
    public async Task<IPagedList<ShopFinancialTransactionModel>> GetPagedShopFinancialTransactions(
      [FromQuery] ShopFinancialTransactionsSearchParameter shopFinancialTransactionsSearchParameter,
      CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareShopFinancialTransactionListModel(shopFinancialTransactionsSearchParameter:
                                                                        shopFinancialTransactionsSearchParameter,
                                                                        cancellationToken: cancellationToken);
    }
    [HttpPost("me/stuffs/{stuffId:int}/price")]
    public async Task CreateShopStuffPrice([FromRoute] int stuffId, ShopStuffPriceAdjustmentModel adjustment,
                                           CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken,
                                                             include: new Include<Shop>(query =>
                                                             {
                                                               query = query.Include(x => x.Owner)
                                                                            .ThenInclude(x => x.Profile).ThenInclude(x => x.City);
                                                               return query;
                                                             }));
      var shopStuff = await shopStuffService.GetShopStuff(shopId: currentShop.Id,
                                                          stuffId: stuffId,
                                                          cancellationToken: cancellationToken);
      var stuffColor = await productService.GetProductColorById(productId: shopStuff.StuffId,
                                                                colorId: adjustment.ColorId,
                                                                cancellationToken: cancellationToken);
      var shopStuffPrice = new ShopStuffPrice
      {
        Price = adjustment.Price
      };

      await shopStuffPriceService.InsertShopStuffPrice(shopStuffPrice: shopStuffPrice,
                                                       shopStuff: shopStuff,
                                                       stuffColor: stuffColor,
                                                       shop: currentShop,
                                                       cancellationToken: cancellationToken);
    }
    [HttpGet("me/in-progress-orders")]
    public async Task<IPagedList<ShopInProgressOrderModel>> PrepareShopInProgressOrders(
      [FromQuery] ShopInProgressOrderSearchParameter parameter, CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareShopInProgressOrders(parameter: parameter, cancellationToken: cancellationToken);
    }

    [HttpGet("me/order-statistics")]
    public async Task<ShopOrderStatisticsModel> PrepareShopOrderStatistics(
      [FromQuery] ShopOrderStatisticsSearchParameter parameters, CancellationToken cancellationToken)
    {
      return await shopFactory.PrepareShopOrderStatistics(parameters: parameters, cancellationToken: cancellationToken);
    }
    #endregion
    #region Login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<LoginResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
      await this.CheckIfUserIsAShopOwner(email: model.Email, cancellationToken: cancellationToken);

      return await authService.Login(authGatewayType: AuthGatewayType.Seller,
                                     email: model.Email,
                                     password: model.Password,
                                     cancellationToken: cancellationToken);
    }
    private async Task CheckIfUserIsAShopOwner(string email, CancellationToken cancellationToken)
    {
      var user = await this.userService.GetUsers(include: new Include<User>(q =>
      {
        q = q.Include(x => x.Profile);
        return q;
      }), email: email).FirstOrDefaultAsync();

      if (user == null)
      {
        throw this.errorFactory.EmailIsNotRegistered();
      }

      var query = this.shopService.GetShops();
      var userShop = await query.Where(x => x.OwnerId == user.Id).FirstOrDefaultAsync();

      if (userShop == null)
      {
        throw this.errorFactory.EmailIsNotRegistered();
      }
    }
    #endregion
  }
}