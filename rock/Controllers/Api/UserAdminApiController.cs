using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Users;
using rock.Core.Services.Common;
using rock.Core.Services.Profiles;
using rock.Core.Services.Users;
using rock.Factories;
using rock.Framework.Crypto;
using rock.Models.UserApi;
using rock.OAuth;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  public class UserAdminApiController : BaseController
  {
    #region Fields
    private readonly ICryptoService cryptoService;
    private readonly IUserService userService;
    private readonly IAuthService authService;
    private readonly IProfileService profileService;
    private readonly IProfileAddressService profileAddressService;
    private readonly ICommonService commonService;
    private readonly IUserFactory userFactory;
    private readonly IProductFactory productFactory;
    #endregion
    #region Constractor
    public UserAdminApiController(IProductFactory productFactory,
                             IUserService userService,
                             IAuthService authService,
                             IUserFactory userFactory,
                             IProfileService profileService,
                             IProfileAddressService profileAddressService,
                             ICommonService commonService, ICryptoService cryptoService)
    {
      this.userService = userService;
      this.authService = authService;
      this.userFactory = userFactory;
      this.productFactory = productFactory;
      this.profileService = profileService;
      this.profileAddressService = profileAddressService;
      this.commonService = commonService;
      this.cryptoService = cryptoService;
    }
    #endregion
    #region  User
    [HttpPost("users")]
    public async Task CreateUser([FromBody] CreateUserModel model, CancellationToken cancellationToken)
    {
      var user = mapCreateUserModel(model: model);
      var userProfile = mapProfile(model: model);
      var personProfile = mapPersonProfile(model: model);
      await userService.CreateUser(user: user, profile: userProfile, personProfile: personProfile, cancellationToken: cancellationToken);
    }

    private User mapCreateUserModel(CreateUserModel model)
    {
      return new User
      {
        Password = model.Phone,
        Role = model.UserRole
      };
    }

    private Profile mapProfile(CreateUserModel model)
    {
      return new Profile
      {
        Email = model.Email,
        Phone = model.Phone
      };
    }
    private PersonProfile mapPersonProfile(CreateUserModel model)
    {
      return new PersonProfile
      {
        FirstName = model.FirstName,
        LastName = model.LastName
      };
    }
    #endregion
    #region User Actions
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/reset-password")]
    public async Task ResetPassword([FromRoute] int userId, [FromBody] ResetPasswordModel input, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken);
      user.RowVersion = input.RowVersion;
      await userService.ResetPassword(user: user,
                                      newPassword: input.NewPassword,
                                      cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/change-role")]
    public async Task ChangeRole([FromRoute] int userId, [FromBody] ChangeRoleModel model, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken);
      user.RowVersion = model.RowVersion;
      this.assertUserCanChangeRole(user);
      await userService.ChangeUserRole(user: user,
                                       roles: model.Roles,
                                       cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/enable")]
    public async Task EnableUser([FromRoute] int userId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken);
      user.RowVersion = rowVersion;
      await userService.EnableUser(user: user,
                                   cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/disable")]
    public async Task DisableUser([FromRoute] int userId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken);
      user.RowVersion = rowVersion;
      await userService.DisableUser(user: user,
                                    cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpDelete("users/{userId}")]
    public async Task DeleteUser([FromRoute] int userId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken);
      user.RowVersion = rowVersion;
      await userService.DeleteUser(user: user,
                                   cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_READ)]
    [HttpGet("users/{userId}")]
    public async Task<UserModel> GetUser([FromRoute] int userId, CancellationToken cancellationToken)
    {
      return await userFactory.PrepareUserModel(id: userId,
                                                cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_READ)]
    [HttpGet("users")]
    public async Task<IPagedList<UserModel>> GetUsersPaged([FromQuery] UserSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await userFactory.PrepareUserPagedListModel(parameters: parameters,
                                                         cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPatch("users/{userId}")]
    public async Task EditUser([FromRoute] int userId, [FromBody] EditUserModel model, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken,
                                               include: new Include<User>(query =>
                                               {
                                                 query = query.Include(x => x.Profile)
                                                 .ThenInclude(x => x.PersonProfile);
                                                 return query;
                                               }));
      var personProfile = user.Profile.PersonProfile;
      user.RowVersion = model.RowVersion;
      this.mapPersonProfile(personProfile: personProfile,
                            model: model);
      var city = await commonService.GetCityById(id: model.CityId ?? 0,
                                                 cancellationToken: cancellationToken);
      await userService.UpdatePersonUser(user: user,
                                         personProfile: personProfile,
                                         profile: user.Profile,
                                         city: city,
                                         cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/change-phone")]
    public async Task SetUserPhone([FromRoute] int userId, [FromBody] ChangeUserPhoneModel model, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                               cancellationToken: cancellationToken,
                                               include: new Include<User>(query =>
                                               {
                                                 query = query.Include(x => x.Profile)
                                                 .ThenInclude(x => x.City);
                                                 return query;
                                               }));
      user.RowVersion = model.RowVersion;
      var profile = user.Profile;
      profile.Phone = model.Phone;
      await userService.UpdateUserProfile(user: user,
                                          profile: profile,
                                          city: profile.City,
                                          cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/change-email")]
    public async Task SetUserEmail([FromRoute] int userId, [FromBody] ChangeUserEmailModel model, CancellationToken cancellationToken)
    {
      var user = await userService.GetUserById(id: userId,
                                                cancellationToken: cancellationToken,
                                                include: new Include<User>(query =>
                                                {
                                                  query = query.Include(x => x.Profile)
                                                  .ThenInclude(x => x.City);
                                                  return query;
                                                }));
      user.RowVersion = model.RowVersion;
      var profile = user.Profile;
      profile.Email = model.Email;
      await userService.UpdateUserProfile(user: user,
                                          profile: profile,
                                          city: profile.City,
                                          cancellationToken: cancellationToken);
    }
    private void assertUserCanChangeRole(User user)
    {
      // var currentUser = this.workContext.GetCurrentUser();
      // if (currentUser.Id == user.Id)
      // {
      //   throw errors.CannotChangeYourRoles();
      // }
    }
    #endregion
    #region User Profile Address
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPost("users/{userId}/addresses")]
    public async Task CreateProfileAddress([FromRoute] int userId, [FromBody] SaveUserAddressModel model, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetProfileByUserId(userId: userId,
                                                            cancellationToken: cancellationToken);
      var address = new ProfileAddress();
      var city = await commonService.GetCityById(id: model.CityId,
                                                 cancellationToken: cancellationToken);
      this.mapProfileAddress(address, model);
      await profileAddressService.InsertProfileAddress(profileAddress: address,
                                                profile: profile,
                                                city: city,
                                                cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpPut("users/{userId}/addresses/{addressId}")]
    public async Task EditProfileAddress([FromRoute] int userId, [FromRoute] int addressId, [FromBody] SaveUserAddressModel model, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetProfileById(userId, cancellationToken);
      var address = await profileAddressService.GetProfileAddressById(profile.Id, addressId, cancellationToken);
      var city = await commonService.GetCityById(model.CityId, cancellationToken);
      this.mapProfileAddress(address, model);
      await profileAddressService.UpdateProfileAddress(address, city, cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_READ)]
    [HttpGet("users/{userId}/addresses")]
    public async Task<IList<UserAddressModel>> GetProfileAddresses([FromRoute] int userId, CancellationToken cancellationToken)
    {
      return await userFactory.PrepareUserAddressListModel(userId, cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_READ)]
    [HttpGet("users/{userId}/addresses/{addressId}")]
    public async Task<UserAddressModel> GetProfileAddress([FromRoute] int userId, [FromRoute] int addressId, CancellationToken cancellationToken)
    {
      return await userFactory.PrepareUserAddressModel(userId, addressId, cancellationToken);
    }
    [Authorize(Scopes.ROCK_USER_MANGE)]
    [HttpDelete("users/{userId}/addresses/{addressId}")]
    public async Task DeleteProfileAddress([FromRoute] int userId, [FromRoute] int addressId, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetProfileById(userId, cancellationToken);
      var address = await profileAddressService.GetProfileAddressById(profile.Id, addressId, cancellationToken);
      await profileAddressService.DeleteProfileAddress(address, cancellationToken);
    }
    #endregion


    #region  Login
    [HttpPost("admin/login")]
    [AllowAnonymous]
    public async Task<LoginResult> Login([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
      return await authService.Login(authGatewayType: AuthGatewayType.Admin,
                                    email: model.Email,
                                    password: model.Password,
                                    cancellationToken: cancellationToken);
    }
    #endregion
  }
}