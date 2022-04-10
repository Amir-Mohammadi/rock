using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;
using rock.Models.PaymentApi;

namespace rock.Factories
{
  public interface IPaymentFactory : IScopedDependency
  {
    Task<IList<PaymentGatewayModel>> PreparePaymentGatewayListModel(PaymentGatewaySearchParameters parameters, CancellationToken cancellationToken);
    Task<IList<BriefPaymentGatewayModel>> PrepareBriefPaymentGatewayListModel(PaymentGatewaySearchParameters parameters, CancellationToken cancellationToken);
  }
}