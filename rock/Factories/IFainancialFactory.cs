using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;
using rock.Models.FinancialApi;

namespace rock.Factories
{
  public interface IFinancialFactory : IScopedDependency
  {
    Task<FinancialAccountModel> PrepareFinancialAccountModel(int financialAccountId, CancellationToken cancellationToken);
    Task<IList<FinancialAccountModel>> PrepareFinancialAccountListModel(FinancialAccountSearchParameters parameters, CancellationToken cancellationToken);
  }
}
