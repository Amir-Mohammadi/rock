using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Profiles;

namespace rock.Core.Services.Financial
{
  public class FinancialAccountService : IFinancialAccountService
  {
    private IRepository<FinancialAccount> financialAccountRepository;

    private IRepository<Profile> profileRepository;

    public FinancialAccountService(IRepository<FinancialAccount> financialAccountRepository, IRepository<Profile> profileRepository)
    {
      this.financialAccountRepository = financialAccountRepository;
      this.profileRepository = profileRepository;
    }


    public async Task InsertFinancialAccount(FinancialAccount financialAccount, CancellationToken cancellationToken)
    {
      await financialAccountRepository.AddAsync(entity: financialAccount, cancellationToken: cancellationToken);
    }
    public async Task UpdateFinancialAccount(FinancialAccount financialAccount, CancellationToken cancellationToken)
    {
      await financialAccountRepository.UpdateAsync(entity: financialAccount, cancellationToken: cancellationToken);
    }
    public async Task<FinancialAccount> GetFinancialAccountById(int id, CancellationToken cancellationToken, Include<FinancialAccount> include)
    {
      return await financialAccountRepository.GetAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken, include: include);
    }
    public async Task<FinancialAccount> GetFinancialAccountByProfileId(int profileId, CancellationToken cancellationToken, IInclude<FinancialAccount> include = null)
    {
      return await financialAccountRepository.GetAsync(predicate: x => x.ProfileId == profileId, cancellationToken: cancellationToken, include: include);
    }
    public async Task DeleteFinancialAccount(FinancialAccount financialAccount, CancellationToken cancellationToken)
    {
      await financialAccountRepository.DeleteAsync(entity: financialAccount, cancellationToken: cancellationToken);
    }
    public IQueryable<FinancialAccount> GetFinancialAccounts(int? currencyId = null,
                                                             int? bankId = null,
                                                             int? profileId = null,
                                                             FinancialAccountType? type = null,
                                                             Include<FinancialAccount> include = null)
    {
      var financialAccounts = financialAccountRepository.GetQuery(include: include);
      if (currencyId != null)
        financialAccounts = financialAccounts.Where(m => m.CurrencyId == currencyId);
      if (bankId != null)
        financialAccounts = financialAccounts.Where(x => x.BankId == bankId);
      if (profileId != null)
        financialAccounts = financialAccounts.Where(x => x.ProfileId == profileId);
      if (type != null)
        financialAccounts = financialAccounts.Where(x => x.Type == type);
      return financialAccounts;
    }
    public async Task<FinancialAccount> GetOrCreateFinancialAccount(Profile profile,
                                                                    Currency currency,
                                                                    FinancialAccountType financialAccountType,
                                                                    CancellationToken cancellationToken)
    {

      if (profile.PersonProfile == null)
        await profileRepository.LoadReferenceAsync(entity: profile, x => x.PersonProfile, cancellationToken: cancellationToken);

      var financialAccount = await GetFinancialAccounts(profileId: profile.Id,
                                                        currencyId: currency.Id,
                                                        type: financialAccountType).FirstOrDefaultAsync(cancellationToken: cancellationToken);

      if (financialAccount == null)
      {
        var fullName = profile.PersonProfile.FatherName == null ? profile.Phone : profile.PersonProfile.FirstName + " " + profile.PersonProfile.LastName;
        financialAccount = new FinancialAccount();
        financialAccount.ProfileId = profile.PersonProfile.ProfileId;
        financialAccount.Title = "حساب مالی  کاربر  " + fullName;
        financialAccount.CurrencyId = currency.Id;
        financialAccount.Type = financialAccountType;
        await InsertFinancialAccount(financialAccount: financialAccount, cancellationToken: cancellationToken);
      }



      return financialAccount;
    }
  }
}