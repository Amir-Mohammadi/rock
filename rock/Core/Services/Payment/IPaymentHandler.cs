using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace rock.Core.Services.Payment
{
  public delegate IPaymentHandler PaymentHandlerResolver(string gateway);


  public interface IPaymentHandler
  {

    Task<Facture> InitPayment(Dictionary<string, string> extraParams, CancellationToken cancellationToken);
    Task HandlePayment(BankInfo bankInfo, bool isBankConfirmationOk, CancellationToken cancellationToken);

    string GetSuccessfullPaymentRedirectUrl(string orderPaymentId);
    string GetFailedPaymentRedirectUrl(string orderPaymentId);

  }
}
