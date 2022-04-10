using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Profiles;
using rock.Core.Services.Common;
using rock.Core.Services.Profiles;
using rock.Core.Services.Users;
using rock.Factories;
using rock.Framework.Crypto;
using rock.Models.ProductApi;
using rock.Models.UserApi;
using rock.OAuth;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  public class UserApiController : BaseController
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
    private readonly IAuthenticator authenticator;
    #endregion
    #region Constractor
    public UserApiController(IProductFactory productFactory,
                             IUserService userService,
                             IAuthService authService,
                             IUserFactory userFactory,
                             IProfileService profileService,
                             IProfileAddressService profileAddressService,
                             ICommonService commonService,
                             ICryptoService cryptoService,
                             IAuthenticator authenticator)
    {
      this.userService = userService;
      this.authService = authService;
      this.userFactory = userFactory;
      this.productFactory = productFactory;
      this.profileService = profileService;
      this.profileAddressService = profileAddressService;
      this.commonService = commonService;
      this.cryptoService = cryptoService;
      this.authenticator = authenticator;
    }
    #endregion
    #region My Profile
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpPatch("me")]
    public async Task EditSelfProfile([FromBody] EditUserModel model, CancellationToken cancellationToken)
    {
      var personProfile = await profileService.GetCurrentUserPersonProfile(cancellationToken: cancellationToken,
                                                                           include: new Include<PersonProfile>(query =>
                                                                           {
                                                                             query = query.Include(x => x.Profile);
                                                                             return query;
                                                                           }));
      this.mapPersonProfile(personProfile: personProfile,
                            model: model);
      var city = await commonService.GetCityById(id: model.CityId ?? 0,
                                                 cancellationToken: cancellationToken);
      await profileService.UpdatePersonProfile(personProfile: personProfile,
                                               profile: personProfile.Profile,
                                               city: city,
                                               cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpPost("me/change-phone")]
    public async Task ChangeSelfPhone([FromBody] ChangeUserPhoneModel model, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      profile.Phone = model.Phone;
      profile.RowVersion = model.RowVersion;
      await profileService.UpdateProfile(profile: profile,
                                         city: profile.City,
                                         cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpPost("me/change-email")]
    public async Task ChangeSelfEmail([FromBody] ChangeUserEmailModel model, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      profile.Email = model.Email;
      profile.RowVersion = model.RowVersion;
      await profileService.UpdateProfile(profile: profile,
                                         city: profile.City,
                                         cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_READ)]
    [HttpGet("me")]
    public async Task<UserModel> GetSelfUser(CancellationToken cancellationToken)
    {
      return await userFactory.PrepareCurrentUserModel(cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpPost("me/change-password")]
    public async Task ChangePassword([FromBody] ChangePasswordModel input, CancellationToken cancellationToken)
    {
      await authService.ChangePassword(oldPassword: input.OldPassword,
                                       newPassword: input.NewPassword,
                                       cancellationToken: cancellationToken);
    }
    #endregion
    #region My Profile Address
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpPost("me/addresses")]
    public async Task CreateSelfProfileAddress([FromBody] SaveUserAddressModel model, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      var city = await commonService.GetCityById(id: model.CityId,
                                                 cancellationToken: cancellationToken);
      var profileAddress = new ProfileAddress();
      mapProfileAddress(profileAddress: profileAddress,
                        model: model);
      await profileAddressService.InsertProfileAddress(profileAddress: profileAddress,
                                                profile: profile,
                                                city: city,
                                                cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpPut("me/addresses/{addressId}")]
    public async Task EditSelfProfileAddress([FromRoute] int addressId, [FromBody] SaveUserAddressModel model, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      var profileAddress = await profileAddressService.GetProfileAddressById(profileId: profile.Id,
                                                                      profileAddressId: addressId,
                                                                      cancellationToken: cancellationToken,
                                                                      include: new Include<ProfileAddress>(query =>
                                                                      {
                                                                        query = query.Include(x => x.Carts);
                                                                        return query;
                                                                      }));
      var city = await commonService.GetCityById(id: model.CityId,
                                                 cancellationToken: cancellationToken);
      mapProfileAddress(profileAddress: profileAddress,
                        model: model);
      await profileAddressService.UpdateProfileAddress(profileAddress: profileAddress,
                                                city: city,
                                                cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpPatch("me/addresses/{addressId}/default")]
    public async Task SetSelfDefaultProfileAddress([FromRoute] int addressId, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      var profileAddress = await profileAddressService.GetProfileAddressById(profileId: profile.Id,
                                                                      profileAddressId: addressId,
                                                                      cancellationToken: cancellationToken);
      await profileAddressService.SetDefaultProfileAddress(profileAddress: profileAddress,
                                                           cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("me/addresses")]
    public async Task<IList<UserAddressModel>> GetSelfProfileAddresses(CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      return await userFactory.PrepareUserAddressListModel(profileId: profile.Id,
                                                           cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("me/orders")]
    public async Task<IList<UserOrderModel>> GetSelfOrders([FromQuery] UserOrderSearchParameters parameter, CancellationToken cancellationToken)
    {
      return await userFactory.PrepareUserOrdersListModel(parameter: parameter, cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_READ)]
    [HttpGet("me/addresses/{addressId}")]
    public async Task<UserAddressModel> GetSelfProfileAddress([FromRoute] int addressId, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      return await userFactory.PrepareUserAddressModel(profileId: profile.Id,
                                                       profileAddressId: addressId,
                                                       cancellationToken: cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_MANGE)]
    [HttpDelete("me/addresses/{addressId}")]
    public async Task DeleteSelfProfileAddress([FromRoute] int addressId, CancellationToken cancellationToken)
    {
      var profile = await profileService.GetCurrentUserProfile(cancellationToken: cancellationToken);
      var profileAddress = await profileAddressService.GetProfileAddressById(profileId: profile.Id,
                                                                profileAddressId: addressId,
                                                                cancellationToken: cancellationToken,
                                                                include: new Include<ProfileAddress>(query =>
                                                                {
                                                                  query = query.Include(x => x.Carts);
                                                                  return query;
                                                                }));
      await profileAddressService.DeleteProfileAddress(profileAddress: profileAddress,
                                                cancellationToken: cancellationToken);
    }
    #endregion
    #region My ThreadActivities
    [Authorize(Scopes.ROCK_PROFILE_READ)]
    [HttpGet("me/liked-products")]
    public async Task<IList<ProductModel>> GetLikedProducts(CancellationToken cancellationToken)
    {
      return await productFactory.PrepareLikedProductListModel(cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_READ)]
    [HttpGet("me/comments")]
    public async Task<IList<ProductCommentModel>> GetUserComments(CancellationToken cancellationToken)
    {
      return await this.productFactory.PrepareUserCommnetListModel(cancellationToken);
    }
    [Authorize(Scopes.ROCK_PROFILE_READ)]
    [HttpGet("me/recent-visits")]
    public async Task<IList<ProductModel>> GetUserRecentProductVisits(CancellationToken cancellationToken)
    {
      return await productFactory.PrepareRecentVisitProductListModel(cancellationToken);
    }
    #endregion
    #region User Actions
    [HttpPost("users/authenticate")]
    [AllowAnonymous]
    public async Task<VerifyModel> Authenticate([FromBody] AuthenticationModel model, CancellationToken cancellationToken)
    {
      return await authService.Authenticate(loginId: model.LoginId, cancellationToken: cancellationToken);
    }
    [HttpPost("users/verify-authenticate")]
    [AllowAnonymous]
    public async Task<LoginResult> VerifyAuthenticate([FromBody] VerifyAuthenticateModel model, CancellationToken cancellationToken)
    {
      return await authService.VerifyAuthenticate(password: model.Password,
                                                  verificationToken: model.VerificationToken,
                                                  authenticateType: model.AuthenticateType,
                                                  cancellationToken: cancellationToken);
    }
    [HttpPost("users/logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
      await authService.Logout();
      return Ok();
    }
    #endregion
  }
}