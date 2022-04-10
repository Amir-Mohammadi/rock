using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Financial;

namespace rock.Core.Services.Financial
{
  public class FinancialTransactionService : IFinancialTransactionService
  {
    private IRepository<FinancialTransaction> financialTransactionRepository;
    public FinancialTransactionService(IRepository<FinancialTransaction> financialTransactionRepository)
    {
      this.financialTransactionRepository = financialTransactionRepository;
    }
    public IQueryable<FinancialTransaction> GetFinancialTransactions(int? financialAccountId = null,
                                                                     Include<FinancialTransaction> include = null)
    {
      var financialTransactions = financialTransactionRepository.GetQuery(include: include);
      if (financialAccountId != null)
      {
        financialTransactions = financialTransactions.Where(x => x.FinancialAccountId == financialAccountId);
      }
      return financialTransactions;
    }
    public async Task DeleteFinancialTransaction(FinancialTransaction financialTransaction,
                                                 CancellationToken cancellationToken)
    {
      financialTransaction.DeletedAt = DateTime.UtcNow;
      await financialTransactionRepository.UpdateAsync(entity: financialTransaction,
                                                       cancellationToken: cancellationToken);
    }

    public async Task InsertFinancialTransaction(FinancialTransaction financialTransaction,
                                                 CancellationToken cancellationToken)
    {
      financialTransaction.CreatedAt = DateTime.UtcNow;
      await financialTransactionRepository.AddAsync(entity: financialTransaction, cancellationToken: cancellationToken);
    }
  }
}