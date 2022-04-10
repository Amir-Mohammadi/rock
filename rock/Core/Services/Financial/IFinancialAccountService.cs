using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Profiles;
using rock.Framework.Autofac;

namespace rock.Core.Services.Financial
{
  public interface IFinancialAccountService : IScopedDependency
  {
    Task InsertFinancialAccount(FinancialAccount financialAccount, CancellationToken cancellationToken);
    Task<FinancialAccount> GetFinancialAccountById(int id, CancellationToken cancellationToken, Include<FinancialAccount> include = null);
    Task DeleteFinancialAccount(FinancialAccount financialAccount, CancellationToken cancellationToken);
    Task<FinancialAccount> GetFinancialAccountByProfileId(int profileId, CancellationToken cancellationToken, IInclude<FinancialAccount> include = null);
    Task UpdateFinancialAccount(FinancialAccount financialAccount, CancellationToken cancellationToken);
    IQueryable<FinancialAccount> GetFinancialAccounts(int? currencyId = null,
                                                      int? bankId = null,
                                                      int? profileId = null,
                                                      FinancialAccountType? type = null,
                                                      Include<FinancialAccount> include = null);
    Task<FinancialAccount> GetOrCreateFinancialAccount(Profile profile, Currency currency, FinancialAccountType financialAccountType, CancellationToken cancellationToken);
  }

}