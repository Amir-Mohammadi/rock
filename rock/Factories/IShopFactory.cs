using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Models.ShopApi;
using rock.Models.WarehousingApi;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Shops;
using rock.Models.MarketApi;

namespace rock.Factories
{
  public interface IShopFactory : IScopedDependency
  {
    #region Shop
    Task<ShopModel> PrepareCurrentShopModel(CancellationToken cancellationToken);
    Task<ShopModel> PrepareShopModel(int shopId, CancellationToken cancellationToken);
    Task<IPagedList<ShopModel>> PrepareShopListModel(ShopSearchParameters parameters,
                                                     CancellationToken cancellationToken);
    Task<IPagedList<ShopInProgressOrderModel>> PrepareShopInProgressOrders(
      ShopInProgressOrderSearchParameter parameter, CancellationToken cancellationToken);
    Task<ShopOrderStatisticsModel> PrepareShopOrderStatistics(ShopOrderStatisticsSearchParameter parameters,
                                                              CancellationToken cancellationToken);

    Task<IPagedList<StuffModel>> PrepareStuffListModel(StuffSearchParameters parameters, CancellationToken cancellationToken);

    #endregion
    #region ShopStuff
    Task<ShopStuffModel> PrepareShopStuffModel(int shopId, int stuffId, CancellationToken cancellationToken);
    Task<IPagedList<ShopStuffModel>> PrepareShopStuffPagedListModel(int shopId, ShopStuffSearchParameters parameters,
                                                                    CancellationToken cancellationToken);
    #endregion
    #region ShopStuffInventory
    Task<IPagedList<ShopInventoryModel>> PrepareShopInventoryPagedListModel(
      int shopId, InventorySearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region ShopOrder
    Task<IPagedList<ShopOrderModel>> PrepareShopOrderListModel(int shopId, ShopOrderSearchParameters parameters,
                                                               CancellationToken cancellationToken);
    Task<ShopOrderModel> PrepareShopOrderModel(int shopId, int orderId, CancellationToken cancellationToken);
    #endregion
    #region ShopDocument
    Task<IPagedList<ShopDocumentModel>> PrepareShopDocumentListModel(int shopId, CancellationToken cancellationToken);
    #endregion
    #region ShopFinancialWallet
    Task<ShopFinancialWalletModel> PrepareShopFinancialWalletModel(int shopId, CancellationToken cancellationToken);
    #endregion
    Task<IPagedList<ShopFinancialTransactionModel>> PrepareShopFinancialTransactionListModel(ShopFinancialTransactionsSearchParameter shopFinancialTransactionsSearchParameter,
                                                                                             CancellationToken cancellationToken);
    Task<ShopFinancialAccountModel> PrepareShopFinancialAccountModel(CancellationToken cancellationToken);
  }
}