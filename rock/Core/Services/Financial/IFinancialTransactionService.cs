using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Financial;
using rock.Framework.Autofac;

namespace rock.Core.Services.Financial
{
  public interface IFinancialTransactionService : IScopedDependency
  {
    Task InsertFinancialTransaction(FinancialTransaction financialTransaction, CancellationToken cancellationToken);
    Task DeleteFinancialTransaction(FinancialTransaction financialTransaction, CancellationToken cancellationToken);
    public IQueryable<FinancialTransaction> GetFinancialTransactions(int? financialAccountId = null,
                                                                     Include<FinancialTransaction> include = null);

  }
}