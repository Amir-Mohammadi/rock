using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Financial;
using rock.Core.Services.Documents;

namespace rock.Core.Services.Financial
{
  public class FinancialService : IFinancialService
  {
    private readonly IFinancialTransactionService financialTransactionService;
    private readonly IDocumentService documentService;
    public FinancialService(IFinancialTransactionService financialTransactionService, IDocumentService documentService)
    {
      this.financialTransactionService = financialTransactionService;
      this.documentService = documentService;
    }
    public double GetAccountCredit(int accountId, DateTime till)
    {
      throw new NotImplementedException();
    }
    public double GetAccountDebit(int accountId, DateTime till)
    {
      throw new NotImplementedException();
    }

    public async Task CreateOnlinePaymentFinancialTransaction(int debitFinancialAccountId,
                                                              int creditFinancialAccountId,
                                                              int amount,
                                                              int userId,
                                                              CancellationToken cancellationToken)
    {
      #region Create Document
      var document = await documentService.CreateDocumentByUserId(form: StaticData.Forms.OnlinePaymentFinancialDocument,
                                                                  userId: userId,
                                                                  description: "خرید از درگاه پرداخت اینترنتی",
                                                                  cancellationToken: cancellationToken);
      #endregion

      #region Create Credit Financial Transaction
      var creditFinancialTransaction = new FinancialTransaction
      {
        DocumentId = document.Id,
        FinancialAccountId = creditFinancialAccountId,
        Amount = amount,
        Factor = TransactionType.Credit
      };
      await financialTransactionService.InsertFinancialTransaction(financialTransaction: creditFinancialTransaction, cancellationToken: cancellationToken);
      #endregion

      #region Create Debit Financial Transaction
      var debitFinancialTransaction = new FinancialTransaction
      {
        DocumentId = document.Id,
        FinancialAccountId = debitFinancialAccountId,
        Amount = amount,
        Factor = TransactionType.Debit
      };
      await financialTransactionService.InsertFinancialTransaction(financialTransaction: debitFinancialTransaction, cancellationToken: cancellationToken);
      #endregion

    }
  }
}
