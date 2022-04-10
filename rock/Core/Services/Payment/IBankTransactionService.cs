using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Payment;
using rock.Framework.Autofac;

namespace rock.Core.Services.Payment
{
  public interface IBankTransactionService : IScopedDependency
  {
    Task<BankTransaction> GetBankTransactionById(int id, CancellationToken cancellationToken);
    Task<BankTransaction> GetBankTransactionByOrderPaymentId(string orderId, CancellationToken cancellationToken);
    Task InsertBankTransaction(BankTransaction transaction, CancellationToken cancellationToken);
    Task UpdateBankTransaction(BankTransaction transaction, CancellationToken cancellationToken);
    IQueryable<BankTransaction> GetBankTransactions(IInclude<BankTransaction> include = null);

  }
}
