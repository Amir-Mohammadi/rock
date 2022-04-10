using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Core.Domains.Orders;
using rock.Framework.Autofac;
using rock.Models.CustomerApi;
namespace rock.Factories
{
  public interface ICustomerFactory : IScopedDependency
  {
    Task<ShoppingModel> PrepareShoppingModel(int shoppingId, CancellationToken cancellationToken);
    Task<IPagedList<ShoppingModel>> PrepareShoppingPagedListModel(ShoppingSearchParameters parameters, CancellationToken cancellationToken);
    Task<CartModel> PrepareShoppingCartModel(CancellationToken cancellationToken);
    Task<BriefCartModel> PrepareShoppingBriefCartModel(CancellationToken cancellationToken);
    Task<CustomerWalletModel> PrepareCustomerWalletModel(CancellationToken cancellationToken);
    Task<IPagedList<CustomerDocumentModel>> PrepareCustomerDocumentPagedListModel(CustomerDocumentsSearchParameters parameters, CancellationToken cancellationToken);
    Task<ShoppingCouponModel> ValidateCoupon(Coupon coupon, CancellationToken cancellationToken);
    Task<OrderCheckoutModel> PrepareOrderSummary(int orderId, CancellationToken cancellationToken);
    Task<OrderPaymentStatusModel> PrepareOrderPaymentStatus(int orderPaymentId, CancellationToken cancellationToken);
  }
}