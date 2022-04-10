using rock.Framework.Autofac;
using rock.Core.Domains.Users;
using rock.Core.Domains.Profiles;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using System.Linq;
namespace rock.Core.Services.Users
{
  public interface IUserService : IScopedDependency
  {
    #region User
    Task<User> CreateUser(User user, Profile profile, PersonProfile personProfile, CancellationToken cancellationToken);
    Task UpdateUser(User user, CancellationToken cancellationToken);
    Task EnableUser(User user, CancellationToken cancellationToken);
    Task DisableUser(User user, CancellationToken cancellationToken);
    Task ChangeUserRole(User user, UserRole roles, CancellationToken cancellationToken);
    Task DeleteUser(User user, CancellationToken cancellationToken);
    Task<User> GetUserById(int id, CancellationToken cancellationToken, IInclude<User> include = null);
    IQueryable<User> GetUsers(int? cityId = null, UserRole? roles = null, string phone = null, string email = null, IInclude<User> include = null);
    Task UpdateUserProfile(User user, Profile profile, City city, CancellationToken cancellationToken);
    Task<User> GetCurrentUser(CancellationToken cancellationToken, IInclude<User> include = null);
    #endregion
    #region Register
    Task<User> RegisterViaPhone(string phone, string firstName, string lastName, CancellationToken cancellationToken);
    Task<User> RegisterViaEmail(string email, string password, string firstName, string lastName, CancellationToken cancellationToken);
    Task<User> Register(string context, CredentialType credentialType, CancellationToken cancellationToken);
    #endregion
    #region PersonUser
    Task<User> InsertPersonUser(User user, Profile profile, PersonProfile personProfile, City city, CancellationToken cancellationToken);
    Task UpdatePersonUser(User user, Profile profile, PersonProfile personProfile, City city, CancellationToken cancellationToken);
    #endregion
    #region CompanyUser
    Task<User> InsertCompanyUser(User user, Profile profile, City city, CompanyProfile companyProfile, CancellationToken cancellationToken);
    Task UpdateCompanyUser(User user, Profile profile, City city, CompanyProfile companyProfile, CancellationToken cancellationToken);
    Task ResetPassword(User user, string newPassword, CancellationToken cancellationToken);
    #endregion
  }
}