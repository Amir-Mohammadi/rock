using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Payment;

namespace rock.Core.Services.Payment
{
  public class BankTransactionService : IBankTransactionService
  {
    private readonly IRepository<BankTransaction> transactionRepository;

    public BankTransactionService(IRepository<BankTransaction> transactionRepository)
    {
      this.transactionRepository = transactionRepository;
    }

    public Task<BankTransaction> GetBankTransactionById(int id, CancellationToken cancellationToken)
    {
      return transactionRepository.GetAsync(x => x.Id == id, cancellationToken);
    }

    public Task<BankTransaction> GetBankTransactionByOrderPaymentId(string orderPaymentId, CancellationToken cancellationToken)
    {
      return transactionRepository.GetAsync(x => x.OrderPaymentId == orderPaymentId, cancellationToken);
    }
    public IQueryable<BankTransaction> GetBankTransactions(IInclude<BankTransaction> include = null)
    {
      return transactionRepository.GetQuery(include);
    }

    public Task InsertBankTransaction(BankTransaction transaction, CancellationToken cancellationToken)
    {
      transaction.CreatedAt = DateTime.UtcNow;
      return transactionRepository.AddAsync(transaction, cancellationToken);
    }

    public Task UpdateBankTransaction(BankTransaction transaction, CancellationToken cancellationToken)
    {
      transaction.UpdatedAt = DateTime.UtcNow;
      return transactionRepository.UpdateAsync(transaction, cancellationToken);
    }
  }
}
