using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using rock.Core.Domains.Users;
using rock.Core.Errors;
using rock.Framework.Crypto;
using rock.Framework.Setting;
using rock.Framework.StateManager;
using rock.Core.Services.Common;
namespace rock.Core.Services.Users
{
  public class TokenManagerService : ITokenManagerService
  {
    private readonly IStateManagerService stateManagerService;
    private readonly ICryptoService cryptoService;
    private readonly IWorkContext workContext;
    private readonly IErrorFactory errorFactory;
    private readonly IOptionsSnapshot<SiteSettings> siteSetting;
    public TokenManagerService(
      IOptionsSnapshot<SiteSettings> siteSetting,
      IStateManagerService stateManagerService,
      ICryptoService cryptoService,
      IWorkContext workContext,
      IErrorFactory errorFactory)
    {
      this.stateManagerService = stateManagerService;
      this.cryptoService = cryptoService;
      this.workContext = workContext;
      this.siteSetting = siteSetting;
      this.errorFactory = errorFactory;
    }
    public async Task<bool> SetLoginUserSecurityStamp(string userId, string securityStamp)
    {
      return await this.stateManagerService.SetState(GetLoginUserKey(userId), securityStamp);
    }
    public async Task<bool> IsActiveToken()
    {
      var hashToken = cryptoService.Hash(workContext.GetUserToken());
      var value = await this.stateManagerService.GetState<string>(GetBlockTokeKey(hashToken));
      if (string.IsNullOrEmpty(value))
        return true;
      else
        return false;
    }
    public async Task<bool> CheckUserSecurityStamp(string userId, string hashValue)
    {
      var value = await this.stateManagerService.GetState<string>(GetLoginUserKey(userId));
      var tokenStamp = Encoding.ASCII.GetBytes(hashValue);
      var mainStamp = Encoding.ASCII.GetBytes(value);
      if (CheckStamp(tokenStamp, mainStamp))
        return true;
      else
        return false;
    }
    public async Task<bool> DeactiveToken(string token, DateTime expireDate)
    {
      var date = DateTime.UtcNow;
      var expirationTimeSpan = expireDate.Subtract(date);
      return await this.stateManagerService.SetState(GetBlockTokeKey(token), "deactivated", expirationTimeSpan);
    }
    public string ValidateVerificationToken(string verificationToken)
    {
      var secretkey = Encoding.UTF8.GetBytes(siteSetting.Value.VerifyTokenSettings.SecretKey);
      var validationParameters = new TokenValidationParameters
      {
        ClockSkew = TimeSpan.Zero, // default: 5 min
        RequireSignedTokens = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretkey),
        RequireExpirationTime = true,
        ValidateLifetime = false,
        ValidateAudience = true, //default : false
        ValidAudience = siteSetting.Value.VerifyTokenSettings.Audience,
        ValidateIssuer = true, //default : false
        ValidIssuer = siteSetting.Value.VerifyTokenSettings.Issuer
      };
      var tokenHandler = new JwtSecurityTokenHandler();
      SecurityToken securityToken;
      var principal = tokenHandler.ValidateToken(verificationToken, validationParameters, out securityToken);
      var jwtSecurityToken = securityToken as JwtSecurityToken;
      if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
      {
        throw errorFactory.InValidVerificationToken();
      }
      var loginId = principal.FindFirst("loginId")?.Value;
      if (string.IsNullOrEmpty(loginId))
      {
        throw errorFactory.InValidVerificationToken();
      }
      return loginId;
    }
    public string GenerateSecurityStamp(User user)
    {
      var securityStamp = user.Password + "-" + user.Enabled.ToString();
      return cryptoService.Hash(securityStamp);
    }
    private static string GetLoginUserKey(string userId)
    {
      return $"user:{userId}";
    }
    private static string GetBlockTokeKey(string token)
    {
      return $"tokens:{token}";
    }
    private static bool CheckStamp(byte[] sourceA, byte[] sourceB)
    {
      if (sourceB.Length != sourceA.Length) return false;
      for (int index = 0; index < sourceA.Length; index++)
      {
        if (sourceA[index] != sourceB[index])
          return false;
      }
      return true;
    }
  }
}