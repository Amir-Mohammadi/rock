using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Payment;
using rock.Framework.Autofac;

namespace rock.Core.Services.Payment
{
  public interface IPaymentService : IScopedDependency
  {


    /// <summary>
    /// Start payment process
    /// </summary>
    /// <param name="gateway">Payment Gateway identifier from PaymentGateways</param>
    /// <param name="paymentKind">Describes paymentkind</param>
    /// <param name="extraParams">Describes paymentkind</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Html that contiains a form which submits automatically with required parameters to send request to bank gateway</returns>
    Task<string> Start(string gateway, string paymentKind, Dictionary<string, string> extraParams, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies bank transaction and redirect to UI
    /// </summary>
    /// <param name="bankTransactionId">Transaction Id to verify</param>
    /// <param name="bankParameters">Parameters return from bank</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Valid uid route</returns>
    Task<string> Finalize(int bankTransactionId, Dictionary<string, string> bankParameters, CancellationToken cancellationToken);

  }
}
