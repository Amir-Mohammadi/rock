using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Payment;
using rock.Framework.Autofac;

namespace rock.Core.Services.Payment
{
  public interface IPaymentGatewayService : IScopedDependency
  {
    Task InsertPaymentGateway(PaymentGateway paymentGateway, FinancialAccount financialAccount, CancellationToken cancellationToken);
    IQueryable<PaymentGateway> GetPaymentGateways(string gateway = null, Include<PaymentGateway> include = null);
    Task<PaymentGateway> GetPaymentGateway(string gateway, CancellationToken cancellationToken, Include<PaymentGateway> include = null);
    Task UpdatePaymentGateway(PaymentGateway paymentGateway, CancellationToken cancellationToken);
    Task SetDefaultPaymentGateway(PaymentGateway paymentGateway, CancellationToken cancellationToken);
    Task DeletePaymentGateway(PaymentGateway paymentGateway, CancellationToken cancellationToken);
  }
}