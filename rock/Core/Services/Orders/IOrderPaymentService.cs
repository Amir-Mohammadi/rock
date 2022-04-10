using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Payment;
using rock.Framework.Autofac;
using rock.Models.CommonApi;
using rock.Models.CustomerApi;

namespace rock.Core.Services.Orders
{
  public interface IOrderPaymentService : IScopedDependency
  {
    Task InsertOrderPayment(Domains.Payment.OrderPayment orderPayment, CancellationToken cancellationToken);
    Task UpdateOrderPayment(Domains.Payment.OrderPayment orderPayment, CancellationToken cancellationToken);
    Task<OrderPayment> GetOrderPaymentById(int id, CancellationToken cancellationToken, Include<OrderPayment> include);
    Task<double> CalculateMinimumOrderPayment(int orderId, CancellationToken cancellationToken);
    Task CheckOrderPaymentVisit(int orderPaymentId, CancellationToken cancellationToken);
    Task SetOrderPaymentVisit(int orderPaymentId, CancellationToken cancellationToken);

  }
}