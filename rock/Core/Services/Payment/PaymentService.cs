using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Payment;
using rock.Core.Services.Common;
namespace rock.Core.Services.Payment
{
  public class PaymentService : IPaymentService
  {
    private readonly IBankTransactionService bankTransactionService;
    private readonly IWorkContext workContext;
    private IPGProviderResolver getIPG;
    private PaymentHandlerResolver getHandler;
    public PaymentService(IBankTransactionService bankTransactionService, IWorkContext workContext, IPGProviderResolver getIPG, PaymentHandlerResolver getHandler)
    {
      this.bankTransactionService = bankTransactionService;
      this.workContext = workContext;
      this.getIPG = getIPG;
      this.getHandler = getHandler;
    }
    public async Task<string> Start(string gateway, string paymentKind, Dictionary<string, string> extraParams, CancellationToken cancellationToken)
    {
      var handler = getHandler(paymentKind);
      var provider = getIPG(gateway);
      var facture = await handler.InitPayment(extraParams: extraParams, cancellationToken: cancellationToken);
      var bankTransaction = new BankTransaction
      {
        OrderPaymentId = facture.OrderPaymentId,
        Amount = facture.Price,
        State = BankTransactionState.Waiting,
        UserId = workContext.GetCurrentUserId(),
        PaymentKind = paymentKind,
        PaymentGateway = gateway,
      };
      await bankTransactionService.InsertBankTransaction(bankTransaction, cancellationToken);
      var html = await provider.Purchase(facture, bankTransaction, cancellationToken);
      return html;
    }

    public async Task<string> Finalize(int bankTransactionId, Dictionary<string, string> bankParameters, CancellationToken cancellationToken)
    {
      var bankTransaction = await bankTransactionService.GetBankTransactionById(bankTransactionId, cancellationToken);
      var gateway = bankTransaction.PaymentGateway;
      var kind = bankTransaction.PaymentKind;
      var handler = getHandler(kind);
      var redirectUrl = handler.GetFailedPaymentRedirectUrl(orderPaymentId: bankTransaction.OrderPaymentId);
      if (bankTransaction.State != BankTransactionState.Waiting)
      {
        return redirectUrl;
      }
      var provider = getIPG(gateway);
      var bankInfo = provider.Parse(bankParameters);
      var isBankConfirmationOk = await provider.Verify(bankParameters, bankTransaction, cancellationToken);
      if (isBankConfirmationOk)
      {
        bankTransaction.State = BankTransactionState.Ok;
      }
      else
      {
        bankTransaction.State = BankTransactionState.Failed;
      }
      await bankTransactionService.UpdateBankTransaction(bankTransaction, cancellationToken);
      try
      {
        await handler.HandlePayment(bankInfo, isBankConfirmationOk, cancellationToken);
      }
      catch (System.Exception)
      {
        // To execute transaction as atomic one.
        // To prevent transaction from rollback (see exception handler)
      }
      if (isBankConfirmationOk)
      {
        redirectUrl = handler.GetSuccessfullPaymentRedirectUrl(orderPaymentId: bankTransaction.OrderPaymentId);
      }
      return redirectUrl;
    }
  }
}