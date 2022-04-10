using System;
using System.Collections.Generic;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Commons;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using System.Linq;
using rock.Models.UserApi;
namespace rock.Core.Services.Profiles
{
  public interface IProfileService : IScopedDependency
  {
    #region Profile
    Task<Profile> InsertProfile(Profile profile, City city, CancellationToken cancellationToken);
    Task UpdateProfile(Profile profile, City city, CancellationToken cancellationToken);
    Task DeleteProfile(Profile profile, CancellationToken cancellationToken);
    Task<Profile> GetProfileById(int Id, CancellationToken cancellationToken, IInclude<Profile> include = null);
    Task<Profile> GetProfileByUserId(int userId, CancellationToken cancellationToken, IInclude<Profile> include = null);
    Task<Profile> GetCurrentUserProfile(CancellationToken cancellationToken, IInclude<Profile> include = null);
    #endregion
    #region PersonProfile
    Task<PersonProfile> InsertPersonProfile(PersonProfile personProfile, Profile profile, City city, CancellationToken cancellationToken);
    Task UpdatePersonProfile(PersonProfile personProfile, Profile profile, City city, CancellationToken cancellationToken);
    Task<PersonProfile> GetPersonProfileById(int id, CancellationToken cancellationToken);
    Task<PersonProfile> GetCurrentUserPersonProfile(CancellationToken cancellationToken, IInclude<PersonProfile> include = null);
    #endregion
    #region CompanyProfile
    Task<CompanyProfile> InsertCompanyProfile(CompanyProfile companyProfile, Profile profile, City city, CancellationToken cancellationToken);
    Task UpdateCompanyProfile(CompanyProfile companyProfile, Profile profile, City city, CancellationToken cancellationToken);
    Task<CompanyProfile> GetCompanyProfileById(int id, CancellationToken cancellationToken);
    Task<CompanyProfile> GetUserCompanyProfile(CancellationToken cancellationToken, IInclude<CompanyProfile> include = null);
    #endregion
    #region  UserOrders
    IQueryable<OrderItem> GetUserOrders(UserOrderSearchParameters parameter, IInclude<OrderItem> include = null);
    #endregion
  }
}