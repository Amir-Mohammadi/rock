using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Payment;
using rock.Framework.Autofac;

namespace rock.Core.Services.Payment.Providers
{
  public interface IBankSamanIPGProvider : IIPGProvider, IScopedDependency
  {

  }

  public class BankTestSamanIPGProvider : IBankSamanIPGProvider
  {
    private const string REDIRECT_URL = "http://95.80.182.57:6001/api/v1/payments/{bankTransactionId}/verify";
    private const string TERM_ID = "Gl8Gv9iG-sFDAbs";
    private const string GATEWAY_ADDRESS = "https://banktest.ir/gateway/saman/gate";
    private const string GATEWAY_METHOD = "POST";


    private const string PARAM_STATE = "State";
    private const string PARAM_STATE_CODE = "StateCode";
    private const string PARAM_RES_NUM = "ResNum";
    private const string PARAM_MID = "MID";
    private const string PARAM_REF_NUM = "RefNum";
    private const string PARAM_CID = "CID";
    private const string PARAM_TRACE_NO = "TraceNo";
    private const string PARAM_SECURE_PAN = "SecurePan";




    private Libs.Saman.Reference.PaymentIFBindingSoapClient transaction;
    private Libs.Saman.Token.PaymentIFBindingSoapClient init;
    private readonly IBankTransactionService bankTransactionService;

    public BankTestSamanIPGProvider(IBankTransactionService bankTransactionService)
    {
      transaction = new Libs.Saman.Reference.PaymentIFBindingSoapClient(Libs.Saman.Reference.PaymentIFBindingSoapClient.EndpointConfiguration.PaymentIFBindingSoap);
      init = new Libs.Saman.Token.PaymentIFBindingSoapClient(Libs.Saman.Token.PaymentIFBindingSoapClient.EndpointConfiguration.PaymentIFBindingSoap);
      this.bankTransactionService = bankTransactionService;
    }

    public BankInfo Parse(Dictionary<string, string> requestParameters)
    {
      var refNum = requestParameters[PARAM_REF_NUM];
      var resNum = requestParameters[PARAM_RES_NUM];
      var mid = requestParameters[PARAM_MID];
      var state = requestParameters[PARAM_STATE];
      var traceNo = requestParameters[PARAM_TRACE_NO];
      var securePan = requestParameters[PARAM_SECURE_PAN];
      return new BankInfo
      {
        OrderPaymentId = resNum
      };
    }

    public async Task<string> Purchase(Facture facture, BankTransaction bankTransaction, CancellationToken cancellationToken)
    {
      var token = await RequestToken(facture.OrderPaymentId, facture.Price);
      var rrg = new RedirectRequestGenerator(GATEWAY_ADDRESS, GATEWAY_METHOD);
      bankTransaction.TerminalId = TERM_ID;
      await bankTransactionService.UpdateBankTransaction(bankTransaction, cancellationToken);
      rrg.Params["Token"] = token;
      rrg.Params["RedirectUrl"] = REDIRECT_URL.Replace("{bankTransactionId}", bankTransaction.Id.ToString());
      return rrg.ToHtmlString();
    }



    private async Task<string> RequestToken(string orderId, long amount)
    {
      var c = await init.RequestTokenAsync(TERM_ID, orderId, amount, 0, 0, 0, 0, 0, 0, "", "", 0);
      return c;
    }



    public async Task<bool> Verify(Dictionary<string, string> requestParameters, BankTransaction bankTransaction, CancellationToken cancellationToken)
    {
      var refNum = requestParameters[PARAM_REF_NUM];
      var resNum = requestParameters[PARAM_RES_NUM];
      var mid = requestParameters[PARAM_MID];
      var state = requestParameters[PARAM_STATE];
      var traceNo = requestParameters[PARAM_TRACE_NO];
      var securePan = requestParameters[PARAM_SECURE_PAN];
      bankTransaction.BankParameter1 = state;
      bankTransaction.BankParameter2 = refNum;
      bankTransaction.BankParameter3 = mid;
      bankTransaction.BankParameter4 = traceNo;
      bankTransaction.CardNo = securePan;
      var isRefNumUsedBefore = bankTransactionService.GetBankTransactions()
                                               .Any(x =>
                                                         x.State != BankTransactionState.Ok &&
                                                         x.PaymentGateway == bankTransaction.PaymentGateway &&
                                                         x.BankParameter2 == refNum
                                               );

      if (isRefNumUsedBefore)
      {
        return false;
      }
      
      await bankTransactionService.UpdateBankTransaction(bankTransaction, cancellationToken);


      var amount = await transaction.verifyTransactionAsync(refNum, mid);
      return amount == bankTransaction.Amount && amount > 0;
    }
  }
}
