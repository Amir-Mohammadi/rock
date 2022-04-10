using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Profiles;
using rock.Core.Errors;
using rock.Core.Services.Financial;
using rock.Models.FinancialApi;

namespace rock.Factories
{
  public class FinancialFactory : BaseFactory, IFinancialFactory
  {
    private readonly IFinancialAccountService financialAccountService;
    public FinancialFactory(IFinancialAccountService financialAccountService)
    {
      this.financialAccountService = financialAccountService;
    }

    #region  FinancialAccount
    public async Task<IList<FinancialAccountModel>> PrepareFinancialAccountListModel(FinancialAccountSearchParameters parameters,
                                                                             CancellationToken cancellationToken)
    {
      var financialAccounts = financialAccountService.GetFinancialAccounts(currencyId: parameters.CurrencyId,
                                                            bankId: parameters.BankId,
                                                            include: new Include<FinancialAccount>(query =>
                                                            {
                                                              query = query.Include(x => x.Bank);
                                                              query = query.Include(x => x.Currency);
                                                              query = query.Include(x => x.Profile).ThenInclude(x => x.PersonProfile);
                                                              return query;
                                                            }));

      return await this.CreateModelPagedListAsync(source: financialAccounts,
                                             convertFunction: this.createFinancialAccountModel,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             sortBy: parameters.SortBy,
                                             cancellationToken: cancellationToken);
    }

    public async Task<FinancialAccountModel> PrepareFinancialAccountModel(int financialAccountId, CancellationToken cancellationToken)
    {
      var financialAccount = await financialAccountService.GetFinancialAccountById(id: financialAccountId,
                                                                      cancellationToken: cancellationToken,
                                                                      new Include<FinancialAccount>(query =>
                                                                      {
                                                                        query = query.Include(x => x.Currency);
                                                                        query = query.Include(x => x.Bank);
                                                                        query = query.Include(x => x.Profile);
                                                                        query = query.Include(x => x.Profile).ThenInclude(x => x.PersonProfile);
                                                                        return query;
                                                                      }));

      return this.createFinancialAccountModel(financialAccount: financialAccount);
    }

    private FinancialAccountModel createFinancialAccountModel(FinancialAccount financialAccount)
    {
      if (financialAccount == null)
        return null;
      return new FinancialAccountModel
      {
        Id = financialAccount.Id,
        Bank = this.createFinancialBankModel(financialAccount.Bank),
        Currency = this.createFinancialCurrencyModel(financialAccount.Currency),
        Profile = this.createFinancialProfileModel(financialAccount.Profile),
        Title = financialAccount.Title,
        No = financialAccount.No,
        Type = financialAccount.Type,
        RowVersion = financialAccount.RowVersion
      };
    }
    private FinancialBankModel createFinancialBankModel(Bank bank)
    {
      if (bank == null)
        return null;
      return new FinancialBankModel
      {
        Id = bank.Id,
        Name = bank.Name
      };
    }

    private FinancialCurrencyModel createFinancialCurrencyModel(Currency currency)
    {
      if (currency == null)
        return null;
      return new FinancialCurrencyModel
      {
        Id = currency.Id,
        Name = currency.Name
      };
    }

    private FinancialProfileModel createFinancialProfileModel(Profile profile)
    {
      if (profile == null)
        return null;
      return new FinancialProfileModel
      {
        Id = profile.Id,
        PhoneNumber = profile.Phone,
        Email = profile.Email,
        PersonProfile = this.createFinancialPersonProfileModel(profile.PersonProfile)
      };
    }

    private FinancialPersonProfileModel createFinancialPersonProfileModel(PersonProfile personProfile)
    {
      if (personProfile == null)
        return null;
      return new FinancialPersonProfileModel
      {
        Id = personProfile.Id,
        FirstName = personProfile.FirstName,
        LastName = personProfile.LastName
      };
    }
    #endregion

  }
}