using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Shops;
using rock.Core.Services.Warehousing;
using rock.Core.Services.Shops;
using rock.Core.Services.Orders;
using rock.Models.ShopApi;
using Microsoft.EntityFrameworkCore;
using rock.Models.WarehousingApi;
using rock.Models.ProductApi;
using rock.Core.Domains.Products;
using rock.Core.Services.Common;
using rock.Core.Domains.Orders;
using System.Linq;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Profiles;
using rock.Core.Services.Financial;
using rock.Core.Services.Products;

namespace rock.Factories
{
  public class ShopFactory : BaseFactory, IShopFactory
  {
    #region Fields
    private readonly IFinancialAccountService financialAccountService;
    private readonly IShopService shopService;
    private readonly IWarehouseService warehouseService;
    private readonly IShopStuffService shopStuffService;
    private readonly ICommonService commonService;
    private readonly IOrderItemService orderItemService;
    private readonly IFinancialTransactionService financialTransactionService;
    private readonly IProductService productService;
    private readonly IShopStuffPriceService shopStuffPriceService;
    #endregion
    #region Constractor
    public ShopFactory(IShopService shopService,
                       IWarehouseService warehouseService,
                       ICommonService commonService,
                       IShopStuffService shopStuffService,
                       IOrderItemService orderItemService,
                       IFinancialAccountService financialAccountService,
                       IFinancialTransactionService financialTransactionService,
                       IProductService productService,
                       IShopStuffPriceService shopStuffPriceService)
    {
      this.financialAccountService = financialAccountService;
      this.financialTransactionService = financialTransactionService;
      this.shopService = shopService;
      this.warehouseService = warehouseService;
      this.commonService = commonService;
      this.shopStuffService = shopStuffService;
      this.orderItemService = orderItemService;
      this.productService = productService;
      this.shopStuffPriceService = shopStuffPriceService;
    }
    #endregion
    #region Shop
    private readonly IInclude<Shop> shopInclude = new Include<Shop>(query =>
    {
      query = query.Include(x => x.City);
      query = query.Include(x => x.City.Province);
      query = query.Include(x => x.Owner);
      query = query.Include(x => x.Owner.Profile);
      query = query.Include(x => x.Owner.Profile.PersonProfile);
      query = query.Include(x => x.Profile);
      query = query.Include(x => x.Profile);
      return query;
    });
    public async Task<ShopModel> PrepareCurrentShopModel(CancellationToken cancellationToken)
    {
      var shop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken, include: shopInclude);
      var result = createShopModel(shop);
      return result;
    }
    public async Task<ShopModel> PrepareShopModel(int shopId, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId, cancellationToken: cancellationToken, include: shopInclude);
      var result = createShopModel(shop);
      return result;
    }
    public async Task<IPagedList<ShopModel>> PrepareShopListModel(ShopSearchParameters parameters,
                                                                  CancellationToken cancellationToken)
    {
      var query = shopService.GetShops(cityId: parameters.CityId, include: shopInclude);
      return await CreateModelPagedListAsync(source: query,
                                             convertFunction: createShopModel,
                                             sortBy: parameters.SortBy,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             cancellationToken: cancellationToken);
    }
    private ShopModel createShopModel(Shop shop)
    {
      if (shop == null)
        return null;
      return new ShopModel
      {
        Id = shop.Id,
        Name = shop.Name,
        OwnerId = shop.OwnerId,
        CityId = shop.CityId,
        Address = shop.Profile?.Address,
        PostalCode = shop.Profile?.PostalCode,
        Telephone = shop.Profile?.Telephone,
        Website = shop.Profile?.Website,
        Active = shop.Active,
        CreatedAt = shop.CreatedAt,
        RowVersion = shop.RowVersion,
        Owner = this.createUserModel(shop.Owner),
        City = this.createCityModel(shop.City),
      };
    }


    public async Task<IPagedList<StuffModel>> PrepareStuffListModel(StuffSearchParameters parameters, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken);

      var products = productService.GetProducts(name: parameters.StuffName,
                                             brandId: parameters.BrandId,
                                             productCategoryId: parameters.CategoryId,
                                             showArchive: false);



      var shopStuffPrices = shopStuffPriceService.GetShopStuffPrices(shopId: currentShop.Id);
      var joinQuery = from product in products
                      from productPrice in product.ProductPrices.Where(x => x.CityId == currentShop.CityId && x.IsPublished == true)
                      join shopStuffPrice in shopStuffPrices on new { stuffId = product.Id, colorId = productPrice.ColorId } equals new { stuffId = shopStuffPrice.StuffId, colorId = shopStuffPrice.ColorId } into tempShopStuffPrice
                      from tShopStuffPrice in tempShopStuffPrice.DefaultIfEmpty()
                      select new StuffModel
                      {
                        Id = product.Id,
                        ShopStuffPriceStuffId = tShopStuffPrice.StuffId,
                        Name = product.Name,
                        CategoryId = product.ProductCategoryId,
                        CategoryName = product.ProductCategory.Name,
                        UrlTitle = product.UrlTitle,
                        BrowserTitle = product.BrowserTitle,
                        BrandName = product.Brand.Name,
                        ColorId = productPrice.ColorId,
                        ColorName = productPrice.Color.Name,
                        MinPrice = productPrice.MinPrice,
                        MaxPrice = productPrice.MaxPrice,
                        PreviewMarketStuffImageId = product.PreviewProductImage.ImageId,
                        PreviewMarketStuffImageRowVersion = product.PreviewProductImage.Image.RowVersion
                      };

      var finalQuery = joinQuery.Where(x => x.ShopStuffPriceStuffId == null);

      return await CreateModelPagedListAsync(source: finalQuery,
                                          convertFunction: e => e,
                                          sortBy: null,
                                          order: null,
                                          pageIndex: 0,
                                          pageSize: 40,
                                          cancellationToken: cancellationToken);
    }
    #endregion
    #region ShopStuff
    private ProductImageModel CreateProductImageModel(ProductImage productImage)
    {
      if (productImage == null)
        return null;
      return new ProductImageModel
      {
        Id = productImage.Id,
        ImageId = productImage.ImageId,
        Order = productImage.Order,
        ImageAlt = productImage.ImageAlt,
        ImageTitle = productImage.ImageTitle,
        RowVersion = productImage.RowVersion,
      };
    }
    public async Task<ShopStuffModel> PrepareShopStuffModel(int shopId, int stuffId,
                                                            CancellationToken cancellationToken)
    {
      var shopStuff = await shopStuffService.GetShopStuff(shopId: shopId,
                                                          stuffId: stuffId,
                                                          cancellationToken: cancellationToken, include: new Include<ShopStuff>(query =>
                                                          {
                                                            query = query.Include(x => x.Stuff);
                                                            query = query.Include(x => x.Stuff).ThenInclude(x => x.ProductColors);
                                                            query = query.Include(x => x.Stuff).ThenInclude(x => x.PreviewProductImage);
                                                            query = query.Include(x => x.Stuff).ThenInclude(x => x.Brand);
                                                            query = query.Include(x => x.Stuff).ThenInclude(x => x.ProductCategory);
                                                            return query;
                                                          }));
      return createShopStuffModel(shopStuff);
    }
    public async Task<IPagedList<ShopStuffModel>> PrepareShopStuffPagedListModel(
      int shopId, ShopStuffSearchParameters parameters, CancellationToken cancellationToken)
    {
      var query = shopStuffService.GetShopStuffs(shopId: shopId,
                                                 stuffId: parameters.StuffId,
                                                 categoryId: parameters.CategoryId,
                                                 brandId: parameters.BrandId,
                                                 q: parameters.Q,
                                                 showArchive: false,
                                                 include: new Include<ShopStuff>(query =>
                                                 {
                                                   query = query.Include(x => x.Stuff).ThenInclude(x => x.Brand);
                                                   query = query.Include(x => x.Stuff)
                                                                .ThenInclude(x => x.ProductCategory);
                                                   query = query.Include(x => x.Stuff)
                                                                .ThenInclude(x => x.ProductColors);
                                                   query = query.Include(x => x.Stuff)
                                                                .ThenInclude(x => x.PreviewProductImage);
                                                   return query;
                                                 }));
      return await CreateModelPagedListAsync(source: query,
                                             convertFunction: createShopStuffModel,
                                             sortBy: parameters.SortBy,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             cancellationToken: cancellationToken);
    }
    private ShopStuffModel createShopStuffModel(ShopStuff shopStuff)
    {
      if (shopStuff == null)
        return null;
      return new ShopStuffModel()
      {
        Id = shopStuff.ShopId,
        ProductName = shopStuff.Stuff.Name,
        StuffId = shopStuff.Stuff.Id,
        BrandName = shopStuff.Stuff.Brand.Name,
        CategoryName = shopStuff.Stuff.ProductCategory.Name,
        Status = false,
        PreviewProductImage = CreateProductImageModel(shopStuff.Stuff.PreviewProductImage),
        VariationNumber = shopStuff.Stuff.ProductColors.Count(),
        RowVersion = shopStuff.RowVersion
      };
    }
    #endregion
    #region ShopStuffInventory
    public async Task<IPagedList<ShopInventoryModel>> PrepareShopInventoryPagedListModel(
      int shopId, InventorySearchParameters parameters, CancellationToken cancellationToken)
    {
      var shop = await shopService.GetShopById(id: shopId,
                                               cancellationToken: cancellationToken,
                                               include: new Include<Shop>(query =>
                                               {
                                                 query = query.Include(x => x.Warehouse);
                                                 return query;
                                               }));
      var inventoryQuery = warehouseService.GetWarehouseInventory(warehouseId: shop.WarehouseId,
                                                                  productId: parameters.ProductId,
                                                                  colorId: parameters.ColorId);
      //TODO: join and fill other properties
      var colors = this.commonService.GetColors();
      var inventoryWithColor = from inventory in inventoryQuery
                               join color in colors on inventory.ColorId equals color.Id
                               select new ShopInventoryModel
                               {
                                 WarehouseId = inventory.WarehouseId,
                                 ProductId = inventory.ProductId,
                                 ColorId = inventory.ColorId,
                                 Amount = inventory.Amount,
                                 ColorName = color.Name,
                               };
      var query = inventoryWithColor.Select(x => new ShopInventoryModel()
      {
        WarehouseId = x.WarehouseId,
        Amount = x.Amount,
        ColorId = x.ColorId,
        ColorName = x.ColorName,
        ProductId = x.ProductId
      });
      return await this.CreateModelPagedListAsync(source: query,
                                                  convertFunction: (ShopInventoryModel shopInventory) => shopInventory,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  cancellationToken: cancellationToken);
    }
    #endregion
    #region ShopOrder
    public async Task<IPagedList<ShopOrderModel>> PrepareShopOrderListModel(
      int shopId, ShopOrderSearchParameters parameters, CancellationToken cancellationToken)
    {
      var query = this.orderItemService.GetOrderItems(parameters: parameters, shopId: shopId,
                                                      include: new Include<OrderItem>(query =>
                                                      {
                                                        query = query.Include(x => x.LatestStatus);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart)
                                                                     .ThenInclude(x => x.ProfileAddress);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart)
                                                                     .ThenInclude(x => x.User)
                                                                     .ThenInclude(x => x.Profile);
                                                        query = query.Include(x => x.CartItem)
                                                                     .ThenInclude(x => x.Product)
                                                                     .ThenInclude(x => x.PreviewProductImage);
                                                        query = query.Include(x => x.CartItem)
                                                                     .ThenInclude(x => x.Product)
                                                                     .ThenInclude(x => x.ProductCategory);
                                                        query = query.Include(x => x.CartItem)
                                                                     .ThenInclude(x => x.Product)
                                                                     .ThenInclude(x => x.Brand);
                                                        query = query.Include(x => x.CartItem)
                                                                     .ThenInclude(x => x.Color);
                                                        return query;
                                                      }));
      return await CreateModelPagedListAsync(source: query,
                                             convertFunction: createShopOrderModel,
                                             sortBy: parameters.SortBy,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             cancellationToken: cancellationToken);
    }
    private ShopOrderModel createShopOrderModel(OrderItem orderItem)
    {
      if (orderItem == null)
        return null;
      return new ShopOrderModel()
      {
        Id = orderItem.Id,
        ProductId = orderItem.CartItem.ProductId,
        ProductName = orderItem.CartItem.Product.Name,
        PreviewProductImage = CreateProductImageModel(orderItem.CartItem.Product.PreviewProductImage),
        ProductCategoryName = orderItem.CartItem.Product.ProductCategory.Name,
        BrandName = orderItem.CartItem.Product.Brand.Name,
        ColorName = orderItem.CartItem.Color.Name,
        CustomerName = createOrderCustomerName(orderItem.Order.Cart.User.Profile),
        Address = createOrderCustomerAddress(orderItem.Order.Cart.ProfileAddress),
        PostalCode = orderItem.Order.Cart.ProfileAddress.PostalCode,
        Phone = orderItem.Order.Cart.User.Profile.Phone,
        RowVersion = orderItem.RowVersion,
      };
    }
    public async Task<ShopOrderModel> PrepareShopOrderModel(int shopId, int orderId,
                                                            CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
    #endregion
    #region ShopDocument
    public async Task<IPagedList<ShopDocumentModel>> PrepareShopDocumentListModel(
      int shopId, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
    #endregion
    #region ShopFinancialWallet
    public async Task<ShopFinancialWalletModel> PrepareShopFinancialWalletModel(
      int shopId, CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
    public async Task<IPagedList<ShopInProgressOrderModel>> PrepareShopInProgressOrders(
      ShopInProgressOrderSearchParameter parameters, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken, include: null);
      var shopStuffs = shopStuffService.GetShopStuffs(shopId: currentShop.Id, include: null);
      var orderItems = orderItemService.GetOrderItems(orderItemStatus: OrderItemStatusType.Created,
                                                      orderStatus: OrderStatusType.InProgress,
                                                      include: new Include<OrderItem>(query =>
                                                      {
                                                        query = query.Include(x => x.Order);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart)
                                                                     .ThenInclude(x => x.ProfileAddress);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart)
                                                                     .ThenInclude(x => x.ProfileAddress)
                                                                     .ThenInclude(x => x.City);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart)
                                                                     .ThenInclude(x => x.ProfileAddress)
                                                                     .ThenInclude(x => x.City)
                                                                     .ThenInclude(x => x.Province);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart)
                                                                     .ThenInclude(x => x.User)
                                                                     .ThenInclude(x => x.Profile)
                                                                     .ThenInclude(x => x.PersonProfile);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Items)
                                                                     .ThenInclude(x => x.CartItem)
                                                                     .ThenInclude(x => x.Product);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Items)
                                                                     .ThenInclude(x => x.CartItem)
                                                                     .ThenInclude(x => x.Product)
                                                                     .ThenInclude(x => x.PreviewProductImage)
                                                                     .ThenInclude(x => x.Image);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Items)
                                                                     .ThenInclude(x => x.CartItem)
                                                                     .ThenInclude(x => x.Product)
                                                                     .ThenInclude(x => x.Brand);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Items)
                                                                     .ThenInclude(x => x.CartItem)
                                                                     .ThenInclude(x => x.Color);
                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Items)
                                                                     .ThenInclude(x => x.CartItem);
                                                        return query;
                                                      }));
      var shopOrders = from orderItem in orderItems
                       join shopStuff in shopStuffs on orderItem.CartItem.ProductId equals shopStuff.StuffId
                       select orderItem;
      return await this.CreateModelPagedListAsync(source: shopOrders,
                                                  convertFunction: createShopInProgressOrderModel,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  cancellationToken: cancellationToken);
    }
    private ShopInProgressOrderModel createShopInProgressOrderModel(OrderItem orderItem)
    {
      if (orderItem == null)
        return null;
      return new ShopInProgressOrderModel()
      {
        OrderItemId = orderItem.Id,
        ProductName = orderItem.CartItem.Product.Name,
        ProductImageId = orderItem.CartItem.Product.PreviewProductImage.Image.Id,
        ProductImageRowVersion = orderItem.CartItem.Product.PreviewProductImage.Image.RowVersion,
        BrandName = orderItem.CartItem.Product.Brand.Name,
        ColorName = orderItem.CartItem.Color.Name,
        ColorCode = orderItem.CartItem.Color.Code,
        ProductCount = orderItem.CartItem.Amount,
        CustomerName = createOrderCustomerName(orderItem.Order.Cart.User.Profile),
        DeliveryDate = null,
        Address = createOrderCustomerAddress(orderItem.Order.Cart.ProfileAddress)
      };
    }
    private string createOrderCustomerName(Profile profile)
    {
      if (profile == null)
        return null;
      if (profile.PersonProfile == null)
        return profile.Phone;
      if (string.IsNullOrEmpty(profile.PersonProfile.FirstName) || string.IsNullOrEmpty(profile.PersonProfile.LastName))
        return profile.Phone;
      return profile.PersonProfile.FirstName + " " + profile.PersonProfile.LastName;
    }
    private string createOrderCustomerAddress(ProfileAddress profileAddress)
    {
      if (profileAddress == null)
        return null;
      if (profileAddress.City == null)
        return null;
      return profileAddress.City?.Province?.Name ??
             "" + " - " + profileAddress?.City.Name ?? "" + " - " + profileAddress.Description ?? "";
    }
    public async Task<ShopOrderStatisticsModel> PrepareShopOrderStatistics(
      ShopOrderStatisticsSearchParameter parameters, CancellationToken cancellationToken)
    {
      var currentShop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken, include: null);
      //should set here fromdate and todate
      var orderItems = orderItemService.GetOrderItems(shopId: currentShop.Id,
                                                      fromDateTime: parameters.FromDateTime,
                                                      toDateTime: parameters.ToDateTime);
      var totalOrderCount = await orderItems.CountAsync(cancellationToken: cancellationToken);
      var sentOrders = await orderItems.CountAsync(x => x.LatestStatus.Type == OrderItemStatusType.Sended,
                                                   cancellationToken: cancellationToken);
      var canceledOrderCount = await orderItems.CountAsync(x => x.LatestStatus.Type == OrderItemStatusType.Cancelled,
                                                           cancellationToken: cancellationToken);
      return new ShopOrderStatisticsModel
      {
        TotalOrderCount = totalOrderCount,
        CanceledOrderCount = canceledOrderCount,
        SentTotalOrderCount = sentOrders,
        RejectedTotalOrderCount = 0
      };
    }
    #endregion
    #region ShopFinancialTransaction
    public async Task<IPagedList<ShopFinancialTransactionModel>> PrepareShopFinancialTransactionListModel(ShopFinancialTransactionsSearchParameter shopFinancialTransactionsSearchParameter,
                                                                                                          CancellationToken cancellationToken)
    {
      var shop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken,
                                                      include: new Include<Shop>(query =>
                                                      {
                                                        query = query.Include(x => x.Owner)
                                                                     .ThenInclude(x => x.Profile)
                                                                     .ThenInclude(x => x.FinancialAccount);
                                                        return query;
                                                      }));

      var financialTransactions = this.financialTransactionService.GetFinancialTransactions(
        financialAccountId: shop.Owner.Profile.FinancialAccount.Id,
        new Include<FinancialTransaction>(query => query.Include(x => x.Document).ThenInclude(x => x.Form)));

      if (shopFinancialTransactionsSearchParameter.TransactionDate != null)
      {
        financialTransactions = financialTransactions
                                  .Where(x => x.CreatedAt.Date
                                  .Equals(shopFinancialTransactionsSearchParameter.TransactionDate.Value.Date));
      }

      return await this.CreateModelPagedListAsync(source: financialTransactions,
                                                  convertFunction: CreateShopFinancialTransactionsModel,
                                                  order: shopFinancialTransactionsSearchParameter.Order,
                                                  pageIndex: shopFinancialTransactionsSearchParameter.PageIndex,
                                                  pageSize: shopFinancialTransactionsSearchParameter.PageSize,
                                                  sortBy: shopFinancialTransactionsSearchParameter.SortBy,
                                                  cancellationToken: cancellationToken);
    }
    public ShopFinancialTransactionModel CreateShopFinancialTransactionsModel(FinancialTransaction financialTransaction)
    {
      return new ShopFinancialTransactionModel()
      {
        Id = financialTransaction.Id,
        CreatedAt = financialTransaction.CreatedAt,
        TransactionType = financialTransaction.Factor,
        Description = financialTransaction.Document.Description,
        FinancialForm = CreateFinancialFormModel(financialTransaction.Document.Form),
        Amount = financialTransaction.Amount
      };
    }
    #endregion
    #region ShopFinancialAccount
    private readonly IInclude<FinancialAccount> financialAccountInclude = new Include<FinancialAccount>(query =>
    {
      query = query.Include(x => x.Bank);
      query = query.Include(x => x.Currency);
      return query;
    });
    public async Task<ShopFinancialAccountModel> PrepareShopFinancialAccountModel(
      CancellationToken cancellationToken)
    {
      var shop = await shopService.GetCurrentUserShop(cancellationToken: cancellationToken, new Include<Shop>(
                                                        query =>
                                                        {
                                                          query = query.Include(x => x.Owner)
                                                                       .ThenInclude(x => x.Profile)
                                                                       .ThenInclude(x => x.FinancialAccount);
                                                          return query;
                                                        }));

      return this.CreateShopFinancialAccountModel(financialAccount: shop.Owner.Profile.FinancialAccount);
    }
    public ShopFinancialAccountModel CreateShopFinancialAccountModel(FinancialAccount financialAccount)
    {
      return new ShopFinancialAccountModel()
      {
        Bank = this.CreateBankModel(bank: financialAccount.Bank),
        Currency = this.CreateCurrencyModel(currency: financialAccount.Currency),
        Id = financialAccount.Id,
        No = financialAccount.No,
        Title = financialAccount.Title,
        BankId = financialAccount.BankId,
        CurrencyId = financialAccount.CurrencyId,
        RowVersion = financialAccount.RowVersion
      };
    }
    #endregion

  }
}