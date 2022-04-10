using rock.Core.Domains.Users;
using rock.Core.Domains.Profiles;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using System;
using rock.Core.Services.Profiles;
using System.Linq;
using rock.Framework.Crypto;
using rock.Core.Services.Common;
using Microsoft.EntityFrameworkCore;
using rock.Core.Errors;

namespace rock.Core.Services.Users
{
  public class UserService : IUserService
  {
    #region Fields
    private readonly IRepository<User> userRepository;
    private readonly IProfileService profileService;
    private readonly ICryptoService cryptoService;
    private readonly IWorkContext workContext;
    private readonly ITokenManagerService tokenManagerService;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public UserService(IRepository<User> userRepository,
                       IProfileService profileService,
                       ICryptoService cryptoService,
                       IWorkContext workContext,
                       ITokenManagerService tokenManagerService,
                       IErrorFactory errorFactory)
    {
      this.userRepository = userRepository;
      this.profileService = profileService;
      this.cryptoService = cryptoService;
      this.workContext = workContext;
      this.tokenManagerService = tokenManagerService;
      this.errorFactory = errorFactory;
    }
    #endregion
    #region User
    public async Task<User> CreateUser(User user, Profile profile, PersonProfile personProfile, CancellationToken cancellationToken)
    {

      await CheckUserExistInfo(profile: profile, cancellationToken: cancellationToken);
      user.Password = this.cryptoService.Hash(user.Password);
      await profileService.InsertPersonProfile(personProfile: personProfile,
                                               profile: profile,
                                               city: null,
                                               cancellationToken: cancellationToken);

      user.CreatedAt = DateTime.UtcNow;
      user.Enabled = true;
      user.Profile = profile;
      await userRepository.AddAsync(entity: user,
                                    cancellationToken: cancellationToken);



      return user;
    }


    public async Task UpdateUser(User user, CancellationToken cancellationToken)
    {
      var securityStamp = tokenManagerService.GenerateSecurityStamp(user: user);
      await tokenManagerService.SetLoginUserSecurityStamp(userId: user.Id.ToString(), securityStamp: securityStamp);
      await userRepository.UpdateAsync(entity: user,
                                       cancellationToken: cancellationToken);
    }
    public async Task EnableUser(User user, CancellationToken cancellationToken)
    {
      user.Enabled = true;
      await UpdateUser(user: user,
                       cancellationToken: cancellationToken);
    }
    public async Task DisableUser(User user, CancellationToken cancellationToken)
    {
      user.Enabled = false;
      await UpdateUser(user: user,
                       cancellationToken: cancellationToken);
    }
    public async Task ChangeUserRole(User user, UserRole newRole, CancellationToken cancellationToken)
    {
      user.Role = newRole;
      await UpdateUser(user: user,
                       cancellationToken: cancellationToken);
    }
    public async Task DeleteUser(User user, CancellationToken cancellationToken)
    {
      await profileService.DeleteProfile(profile: user.Profile,
                                         cancellationToken: cancellationToken);
      await userRepository.DeleteAsync(entity: user,
                                       cancellationToken: cancellationToken);
    }
    public async Task<User> GetUserById(int id, CancellationToken cancellationToken, IInclude<User> include = null)
    {
      var user = await userRepository.GetAsync(predicate: x => x.Id == id,
                                               cancellationToken: cancellationToken,
                                               include: include);
      return user;
    }
    public IQueryable<User> GetUsers(int? cityId = null,
                                     UserRole? roles = null,
                                     string phone = null,
                                     string email = null,
                                     IInclude<User> include = null)
    {
      var query = userRepository.GetQuery(include);
      if (cityId != null)
        query = query.Where(x => x.Profile.CityId == cityId);
      if (roles != null)
        query = query.Where(x => (x.Role & roles) > 0);
      if (phone != null)
        query = query.Where(x => x.Profile.Phone == phone);
      if (email != null)
        query = query.Where(x => x.Profile.Email == email);
      return query;
    }
    public async Task UpdateUserProfile(User user, Profile profile, City city, CancellationToken cancellationToken)
    {
      user.UpdatedAt = DateTime.UtcNow;
      user.Profile = profile;
      await UpdateUser(user: user,
                       cancellationToken: cancellationToken);
      await profileService.UpdateProfile(profile: profile,
                                         city: city,
                                         cancellationToken: cancellationToken);
    }
    public async Task<User> GetCurrentUser(CancellationToken cancellationToken, IInclude<User> include = null)
    {
      var currentUserId = this.workContext.GetCurrentUserId();
      var user = await userRepository.GetAsync(predicate: x => x.Id == currentUserId,
                                               include: include,
                                               cancellationToken: cancellationToken);
      return user;
    }
    #endregion
    #region RegisterUser
    public async Task<User> RegisterViaEmail(string email, string password, string firstName, string lastName, CancellationToken cancellationToken)
    {
      var user = new User();
      var profile = new Profile();
      var personProfile = new PersonProfile();
      user.Password = cryptoService.Hash(password);
      profile.Email = email;
      personProfile.LastName = lastName;
      personProfile.FirstName = firstName;
      await profileService.InsertPersonProfile(personProfile: personProfile,
                                               profile: profile,
                                               city: null,
                                               cancellationToken: cancellationToken);
      user.Profile = profile;
      user.CreatedAt = DateTime.UtcNow;
      user.Role = UserRole.Customer;
      user.Enabled = true;
      await userRepository.AddAsync(entity: user,
                                    cancellationToken: cancellationToken);
      //TODO send code to email 
      return user;
    }
    public async Task<User> RegisterViaPhone(string phone, string firstName, string lastName, CancellationToken cancellationToken)
    {
      var user = new User();
      var profile = new Profile();
      var personProfile = new PersonProfile();
      profile.Phone = phone;
      personProfile.LastName = lastName;
      personProfile.FirstName = firstName;
      await profileService.InsertPersonProfile(personProfile: personProfile,
                                               profile: profile,
                                               city: null,
                                               cancellationToken: cancellationToken);
      user.Profile = profile;
      user.CreatedAt = DateTime.UtcNow;
      user.Role = UserRole.Customer;
      user.Enabled = true;
      await userRepository.AddAsync(entity: user,
                                    cancellationToken: cancellationToken);
      return user;
    }
    public async Task<User> Register(string context, CredentialType credentialType, CancellationToken cancellationToken)
    {
      var user = new User();
      var profile = new Profile();
      var personProfile = new PersonProfile();
      switch (credentialType)
      {
        case CredentialType.Email:
          profile.Email = context;
          break;
        case CredentialType.Phone:
          profile.Phone = context;
          break;
      };
      await profileService.InsertPersonProfile(personProfile: personProfile,
                                               profile: profile,
                                               city: null,
                                               cancellationToken: cancellationToken);
      user.Profile = profile;
      user.CreatedAt = DateTime.UtcNow;
      user.Role = UserRole.Customer;
      user.Enabled = true;
      await userRepository.AddAsync(entity: user,
                                    cancellationToken: cancellationToken);
      return user;
    }
    #endregion
    #region PersonUser
    public async Task<User> InsertPersonUser(User user, Profile profile, PersonProfile personProfile, City city, CancellationToken cancellationToken)
    {
      await profileService.InsertPersonProfile(personProfile: personProfile,
                                               profile: profile,
                                               city: city,
                                               cancellationToken: cancellationToken);
      user.Profile = profile;
      user.CreatedAt = DateTime.UtcNow;
      user.Role = UserRole.Customer;
      user.Enabled = true;
      await userRepository.AddAsync(entity: user,
                                    cancellationToken: cancellationToken);
      return user;
    }
    public async Task UpdatePersonUser(User user, Profile profile, PersonProfile personProfile, City city, CancellationToken cancellationToken)
    {
      await profileService.UpdatePersonProfile(personProfile: personProfile,
                                               profile: profile,
                                               city: city,
                                               cancellationToken: cancellationToken);
      user.Profile = profile;
      await userRepository.UpdateAsync(entity: user,
                                       cancellationToken: cancellationToken);
    }
    #endregion
    #region CompanyUser
    public async Task<User> InsertCompanyUser(User user, Profile profile, City city, CompanyProfile companyProfile, CancellationToken cancellationToken)
    {
      await profileService.InsertCompanyProfile(companyProfile: companyProfile,
                                                profile: profile,
                                                city: city,
                                                cancellationToken: cancellationToken);
      user.Profile = profile;
      user.CreatedAt = DateTime.UtcNow;
      await userRepository.AddAsync(entity: user,
                                    cancellationToken: cancellationToken);
      return user;
    }
    public async Task UpdateCompanyUser(User user, Profile profile, City city, CompanyProfile companyProfile, CancellationToken cancellationToken)
    {
      await profileService.UpdateCompanyProfile(companyProfile: companyProfile,
                                               profile: profile,
                                               city: city,
                                               cancellationToken: cancellationToken);
      user.Profile = profile;
      await userRepository.UpdateAsync(entity: user,
                                       cancellationToken: cancellationToken);
    }
    private async Task CheckUserExistInfo(Profile profile, CancellationToken cancellationToken)
    {
      var currentUsers = GetUsers(include: new Include<User>(query =>
      {
        query = query.Include(x => x.Profile).ThenInclude(x => x.PersonProfile);
        return query;
      }));

      if (await currentUsers.AnyAsync(x => x.Profile.Phone == profile.Phone, cancellationToken: cancellationToken))
      {
        throw errorFactory.ProfileWithCurrentPhoneExists();
      }

      if (await currentUsers.AnyAsync(x => x.Profile.Email == profile.Email, cancellationToken: cancellationToken))
      {
        throw errorFactory.ProfileWithCurrentEmailExists();
      }

    }
    public async Task ResetPassword(User user, string newPassword, CancellationToken cancellationToken)
    {
      user.Password = this.cryptoService.Hash(newPassword);
      user.UpdatedAt = DateTime.UtcNow;
      await userRepository.UpdateAsync(user, cancellationToken);
    }
    #endregion
  }
}