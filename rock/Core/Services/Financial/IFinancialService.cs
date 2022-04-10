using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Financial;
using rock.Framework.Autofac;

namespace rock.Core.Services.Financial
{
  public interface IFinancialService : IScopedDependency
  {
    double GetAccountDebit(int accountId, DateTime till);
    double GetAccountCredit(int accountId, DateTime till);
    Task CreateOnlinePaymentFinancialTransaction(int debitFinancialAccountId, int creditFinancialAccountId, int amount, int userId, CancellationToken cancellationToken);
  }
}
