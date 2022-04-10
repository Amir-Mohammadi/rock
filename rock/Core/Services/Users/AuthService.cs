using rock.Core.Common;
using rock.Core.Common.Exception;
using rock.Core.Domains.Users;
using rock.Core.Services.Profiles;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Crypto;
using rock.Models.UserApi;
using rock.Core.Errors;
using rock.Core.Services.Common;
using rock.Framework.StateManager;
using rock.OAuth;
namespace rock.Core.Services.Users
{
  public class AuthService : IAuthService
  {
    #region Fields
    private IAuthenticator authenticator;
    private ITokenGenerator tokenGenerator;
    private ITokenManagerService tokenManagerService;
    private IClaimGenerator claimGenerator;
    private IUserService userService;
    private IProfileService profileService;
    private readonly IWorkContext workContext;
    private readonly ICryptoService cryptoService;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public AuthService(
                       IAuthenticator authenticator,
                       ITokenGenerator tokenGenerator,
                       IClaimGenerator claimGenerator,
                       IUserService userSerice,
                       IProfileService profileService,
                       IWorkContext workContext,
                       ICryptoService cryptoService,
                       ITokenManagerService tokenManagerService,
                       IErrorFactory errorFactory)
    {
      this.authenticator = authenticator;
      this.tokenGenerator = tokenGenerator;
      this.claimGenerator = claimGenerator;
      this.userService = userSerice;
      this.profileService = profileService;
      this.workContext = workContext;
      this.cryptoService = cryptoService;
      this.tokenManagerService = tokenManagerService;
      this.errorFactory = errorFactory;
    }
    #endregion
    public async Task<VerifyModel> Authenticate(string loginId, CancellationToken cancellationToken)
    {
      var attemptResult = await authenticator.AuthenticateAttempt(loginId: loginId,
                                                                            cancellationToken: cancellationToken);
      var verifyModel = new VerifyModel();
      verifyModel.IsOptionalAuthenticateType = false;
      if (attemptResult.User == null
          && attemptResult.AuthenticationLoginType == AuthenticateLoginType.Email)
      {
        throw errorFactory.EmailIsNotRegistered();
      }
      switch (attemptResult.AuthenticationLoginType)
      {
        case AuthenticateLoginType.Email:
          verifyModel.AuthenticateLoginType = AuthenticateLoginType.Email;
          verifyModel.AuthenticateType = AuthenticateType.Password;
          break;
        case AuthenticateLoginType.Phone:
          verifyModel.AuthenticateLoginType = AuthenticateLoginType.Phone;
          verifyModel.AuthenticateType = AuthenticateType.OneTimePassword;
          if (attemptResult.User != null && attemptResult.User.Password != null)
            verifyModel.IsOptionalAuthenticateType = true;
          await authenticator.SendCode(context: loginId, cancellationToken: cancellationToken);
          break;
      };
      verifyModel.Token = await tokenGenerator.CreateVerifyToken(context: loginId);
      return verifyModel;
    }
    public async Task Logout()
    {
      var token = cryptoService.Hash(workContext.GetUserToken());
      await tokenManagerService.DeactiveToken(token, workContext.GetUserTokenExpiration());
    }
    public async Task ChangePassword(string oldPassword, string newPassword, CancellationToken cancellationToken)
    {
      var currentUserId = workContext.GetCurrentUserId();
      var user = await userService.GetUserById(currentUserId, cancellationToken);
      if (cryptoService.Hash(oldPassword) != user.Password)
        throw new UnauthorizedException(ErrorCodes.PASSWORDS_DOSE_NOT_MATCH);
      user.Password = cryptoService.Hash(newPassword);
      await userService.UpdateUser(user: user,
                                   cancellationToken: cancellationToken);
    }
    public async Task<LoginResult> VerifyAuthenticate(string password,
                                                     string verificationToken,
                                                     AuthenticateType authenticateType,
                                                     CancellationToken cancellationToken)
    {
      var loginId = tokenManagerService.ValidateVerificationToken(verificationToken);
      var loginType = authenticator.ValidateLoginId(loginId: loginId);
      var user = new User();
      switch (loginType)
      {
        case AuthenticateLoginType.Email:
          user = await authenticator.VerifyEmail(email: loginId,
                                                 password: password,
                                                 cancellationToken: cancellationToken);
          break;
        case AuthenticateLoginType.Phone:
          user = await authenticator.VerifyPhone(phone: loginId,
                                                 password: password,
                                                 authenticateType: authenticateType,
                                                 cancellationToken: cancellationToken);
          break;
      };
      var loginModel = new LoginResult();
      loginModel.Token = await CreateToken(user: user, cancellationToken: cancellationToken);
      return loginModel;
    }
    public async Task<LoginResult> Login(AuthGatewayType authGatewayType, string email, string password, CancellationToken cancellationToken)
    {
      var user = await authenticator.VerifyEmail(email: email,
                                                 password: password,
                                                 cancellationToken: cancellationToken);
      if (!AuthGateway.CheckAuthGateway(authGateway: authGatewayType, userRole: user.Role))
      {
        throw this.errorFactory.InvalidEmailCredentials();
      }
      var loginModel = new LoginResult();
      loginModel.Token = await CreateToken(user: user, cancellationToken: cancellationToken);
      return loginModel;
    }
    public async Task<string> CreateToken(User user, CancellationToken cancellationToken)
    {
      var securityStamp = tokenManagerService.GenerateSecurityStamp(user: user);
      await tokenManagerService.SetLoginUserSecurityStamp(userId: user.Id.ToString(), securityStamp: securityStamp);
      var claims = claimGenerator.Create(user: user, securityStamp: securityStamp);
      return await tokenGenerator.Create(claims: claims,
                                         cancellationToken: cancellationToken);
    }
    public async Task<string> CreateVerifyToken(string context, CancellationToken cancellationToken)
    {
      return await tokenGenerator.CreateVerifyToken(context: context);
    }
    public async Task<string> VerifyVerificationToken(string verificationToken, CancellationToken cancellationToken)
    {
      var phone = tokenManagerService.ValidateVerificationToken(verificationToken);
      return await Task.Run(() => phone);
    }
  }
}