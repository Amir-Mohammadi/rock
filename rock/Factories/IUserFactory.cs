using System;
using System.Collections.Generic;
using rock.Framework.Autofac;
using rock.Core.Common;
using rock.Models.UserApi;
using rock.Core.Domains.Profiles;
using rock.Core.Services.Users;
using rock.Core.Domains.Users;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Orders;
namespace rock.Factories
{
  public interface IUserFactory : IScopedDependency
  {
    #region CurrentUser
    Task<UserModel> PrepareCurrentUserModel(CancellationToken cancellationToken);
    #endregion
    #region Users
    Task<UserModel> PrepareUserModel(int id, CancellationToken cancellationToken);
    Task<IPagedList<UserModel>> PrepareUserPagedListModel(UserSearchParameters parameters, CancellationToken cancellationToken);
    Task<UserAddressModel> PrepareUserAddressModel(int profileId, int profileAddressId, CancellationToken cancellationToken);
    Task<IList<UserAddressModel>> PrepareUserAddressListModel(int profileId, CancellationToken cancellationToken);
    Task<IList<UserOrderModel>> PrepareUserOrdersListModel(UserOrderSearchParameters parameter, CancellationToken cancellationToken);
    #endregion
  }
}