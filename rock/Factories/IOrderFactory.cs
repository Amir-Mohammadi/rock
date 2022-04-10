using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Models.OrderApi;
namespace rock.Factories
{
  public interface IOrderFactory : IScopedDependency
  {
    Task<GetOrderModel> PrepareOrderModel(int orderId, CancellationToken cancellationToken);
    Task<IList<OrderItemModel>> PrepareOrderItemModel(int orderId, CancellationToken cancellationToken);
    Task<OrderDetailModel> PrepareOrderDetailModel(int orderId, CancellationToken cancellationToken);
    Task<IPagedList<GetOrderModel>> PrepareOrderListModel(OrderSearchParameters parameters, CancellationToken cancellationToken);
  }
}