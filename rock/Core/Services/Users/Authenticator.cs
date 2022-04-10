using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Users;
using Microsoft.EntityFrameworkCore;
using rock.Core.Services.Common;
using rock.Framework.Crypto;
using rock.Framework.StateManager;
using rock.Models.UserApi;
using System.ComponentModel.DataAnnotations;
using rock.Core.Errors;
namespace rock.Core.Services.Users
{
  public class Authenticator : IAuthenticator
  {
    private IRepository<User> userRepository;
    private ITelephonyService telephonyService;
    private ICryptoService cryptoService;
    private IErrorFactory errorFactory;
    private readonly IUserService userService;
    private readonly IStateManagerService stateManagerService;
    const int VERIFICATION_CODE_EXPIRE_MINUTES = 3;
    public Authenticator(IRepository<User> userRepository,
                         IErrorFactory errorFactory,
                         ITelephonyService telephonyService,
                         ICryptoService cryptoService,
                         IStateManagerService stateManagerService,
                         IUserService userService)
    {
      this.userRepository = userRepository;
      this.errorFactory = errorFactory;
      this.telephonyService = telephonyService;
      this.cryptoService = cryptoService;
      this.stateManagerService = stateManagerService;
      this.userService = userService;
    }
    public async Task<IAttemptResponse> AuthenticateAttempt(string loginId, CancellationToken cancellationToken)
    {
      var authenticateLoginType = ValidateLoginId(loginId: loginId);
      var response = new AttemptResponse();
      switch (authenticateLoginType)
      {
        case AuthenticateLoginType.Email:
          response.User = await userRepository.GetAsync(predicate: user => user.Profile.Email == loginId,
                                                        cancellationToken: cancellationToken,
                                                        include: null);
          break;
        case AuthenticateLoginType.Phone:
          response.User = await userRepository.GetAsync(predicate: x => x.Profile.Phone == loginId,
                                            cancellationToken: cancellationToken,
                                            include: null);
          break;
        default:
          throw errorFactory.InvalidLoginId();
      };
      response.AuthenticationLoginType = authenticateLoginType;
      return response;
    }
    public AuthenticateLoginType ValidateLoginId(string loginId)
    {
      if (new EmailAddressAttribute().IsValid(loginId))
        return AuthenticateLoginType.Email;
      else if (new PhoneAttribute().IsValid(loginId))
        return AuthenticateLoginType.Phone;
      else
        throw errorFactory.InvalidLoginId();
    }
    public async Task SendCode(string context, CancellationToken cancellationToken)
    {
      var random = new Random();
      string code = random.Next(1000, 9999).ToString();
      await stateManagerService.SetState(context, code, new TimeSpan(0, VERIFICATION_CODE_EXPIRE_MINUTES, 0));
      await telephonyService.SendSMS(
                             phone: context,
                             message: code,
                             cancellationToken: cancellationToken);
    }
    public async Task<User> VerifyPhone(string phone, string password, AuthenticateType authenticateType, CancellationToken cancellationToken)
    {
      var user = await userRepository.GetAsync(predicate: x => x.Profile.Phone == phone,
                                               cancellationToken: cancellationToken,
                                               include: new Include<User>(query =>
                                               {
                                                 query = query.Include(x => x.Profile);
                                                 return query;
                                               }));
      switch (authenticateType)
      {
        case AuthenticateType.OneTimePassword:
          var verification = await stateManagerService.GetState<string>(phone);
          if (verification == null || verification != password)
          {
            throw errorFactory.InValidVerificationCode();
          }
          await stateManagerService.RemoveState(phone);
          if (user == null)
          {
            user = await userService.Register(context: phone,
                                              credentialType: CredentialType.Phone,
                                              cancellationToken: cancellationToken);
          }
          break;
        case AuthenticateType.Password:
          var hashedPassword = cryptoService.Hash(password);
          if (user.Password != hashedPassword)
          {
            throw errorFactory.InvalidPassword();
          }
          break;
        default:
          throw errorFactory.InvalidAuthenticateType();
      };
      return user;
    }
    public async Task<User> VerifyEmail(string email, string password, CancellationToken cancellationToken)
    {
      var hashedPassword = cryptoService.Hash(values: password);
      var user = await userRepository.GetAsync(predicate: x => (x.Profile.Email == email && x.Password == hashedPassword),
                                                   cancellationToken: cancellationToken,
                                                   include: new Include<User>(query =>
                                                   {
                                                     query = query.Include(x => x.Profile);
                                                     return query;
                                                   }));
      if (user == null)
      {
        throw errorFactory.InvalidEmailCredentials();
      }
      return user;
    }
  }
}