using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using rock.Framework.Setting;
using System.Collections.Generic;

namespace rock.Core.Services.Users
{
  public class TokenGenerator : ITokenGenerator
  {
    private IOptionsSnapshot<SiteSettings> siteSetting;

    public TokenGenerator(IOptionsSnapshot<SiteSettings> settings)
    {
      this.siteSetting = settings;
    }

    public Task<string> Create(Claim[] claims, CancellationToken cancellationToken, long? lifetime = null)
    {
      var secretKey = Encoding.UTF8.GetBytes(siteSetting.Value.TokenSettings.SecretKey);
      var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

      var encryptionkey = System.Text.Encoding.UTF8.GetBytes(siteSetting.Value.TokenSettings.EncryptKey);
      var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);
      var descriptor = new SecurityTokenDescriptor
      {
        Issuer = siteSetting.Value.TokenSettings.Issuer,
        Audience = siteSetting.Value.TokenSettings.Audience,
        IssuedAt = DateTime.UtcNow,
        NotBefore = DateTime.UtcNow.AddMinutes(siteSetting.Value.TokenSettings.NotBeforeMinutes),
        Expires = DateTime.UtcNow.AddMinutes(siteSetting.Value.TokenSettings.ExpirationMinutes),
        SigningCredentials = signingCredentials,
        EncryptingCredentials = encryptingCredentials,
        Subject = new ClaimsIdentity(claims)
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var securityToken = tokenHandler.CreateToken(descriptor);

      var jwt = tokenHandler.WriteToken(securityToken);

      return Task.Run(() => jwt.ToString());
    }


    public Task<string> CreateVerifyToken(string context)
    {
      var secretKey = Encoding.UTF8.GetBytes(siteSetting.Value.VerifyTokenSettings.SecretKey);
      var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

      var claims = new List<Claim>();
      claims.Add(new Claim("loginId", context));

      var descriptor = new SecurityTokenDescriptor
      {
        Issuer = siteSetting.Value.VerifyTokenSettings.Issuer,
        Audience = siteSetting.Value.VerifyTokenSettings.Audience,
        IssuedAt = DateTime.UtcNow,
        NotBefore = DateTime.UtcNow.AddMinutes(siteSetting.Value.VerifyTokenSettings.NotBeforeMinutes),
        Expires = DateTime.UtcNow.AddMinutes(siteSetting.Value.VerifyTokenSettings.ExpirationMinutes),
        SigningCredentials = signingCredentials,
        Subject = new ClaimsIdentity(claims)
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var securityToken = tokenHandler.CreateToken(descriptor);

      var jwt = tokenHandler.WriteToken(securityToken);

      return Task.Run(() => jwt.ToString());
    }

  }
}

