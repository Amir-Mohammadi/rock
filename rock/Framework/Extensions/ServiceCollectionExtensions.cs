using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rock.Framework.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using System.Threading.Tasks;
using rock.Core.Services.Users;
using StackExchange.Redis;
using rock.Core.Errors;
namespace rock.Framework.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static void AddContext(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<rock.Core.Data.ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
    }
    public static void AddCustomMvc(this IServiceCollection services, string MyAllowSpecificOrigins)
    {
      services.AddMvcCore().AddDataAnnotations();
      var withOrigins = Startup.SiteSetting.WithOrigins.Split(",");
      services.AddCors(options =>
      {
        options.AddPolicy(MyAllowSpecificOrigins,
              builder =>
              {
                builder.WithOrigins(withOrigins)
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowCredentials();
              });
      });
    }
    public static void AddCustomValidationResponse(this IServiceCollection services, NamingStrategy namingStrategy, int errorCode, string title)
    {
      services.Configure<ApiBehaviorOptions>((options) =>
      {
        options.InvalidModelStateResponseFactory = (context) =>
        {
          var state = context.ModelState;
          var additionalData = state.Select(s => new
          {
            Field = namingStrategy.GetPropertyName(s.Key, false),
            Messages = s.Value.Errors.Select(e =>
              {
                var msg = e.ErrorMessage;
                if (!string.IsNullOrWhiteSpace(s.Key))
                  e.ErrorMessage.Replace(s.Key, namingStrategy.GetPropertyName(s.Key, false));
                return msg;
              }
            )
          });
          var response = new
          {
            Code = errorCode,
            Title = title,
            Info = additionalData
          };
          return new BadRequestObjectResult(response);
        };
      });
    }
    public static void AddRadisService(this IServiceCollection services, IConfiguration configuration)
    {
      var radis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RadisServer"));
      services.AddScoped(s => radis.GetDatabase());
    }
    public static void AddJwtAuthentication(this IServiceCollection services, TokenSettings tokenSettings)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        var secretkey = Encoding.UTF8.GetBytes(tokenSettings.SecretKey);
        var encryptionkey = Encoding.UTF8.GetBytes(tokenSettings.EncryptKey);
        var validationParameters = new TokenValidationParameters
        {
          ClockSkew = TimeSpan.Zero, // default: 5 min
          RequireSignedTokens = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secretkey),
          RequireExpirationTime = true,
          ValidateLifetime = true,
          ValidateAudience = true, //default : false
          ValidAudience = tokenSettings.Audience,
          ValidateIssuer = true, //default : false
          ValidIssuer = tokenSettings.Issuer,
          TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
          NameClaimType = ClaimTypes.NameIdentifier
        };
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = validationParameters;
        options.Events = new JwtBearerEvents
        {
          OnAuthenticationFailed = context =>
                {
                  if (context.Exception != null)
                  {
                    var errorFactory = context.HttpContext.RequestServices.GetRequiredService<IErrorFactory>();
                    context.Fail(errorFactory.AccessDenied());
                  }
                  return Task.CompletedTask;
                },
          OnTokenValidated = async context =>
                {
                  var errorFactory = context.HttpContext.RequestServices.GetRequiredService<IErrorFactory>();
                  var tokenMangerContext = context.HttpContext.RequestServices.GetRequiredService<ITokenManagerService>();
                  var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                  var userClaim = claimsIdentity.FindFirst("user-id");
                  var securityStamp = claimsIdentity.FindFirst("security-stamp");
                  var checkLoggedUser = await tokenMangerContext.IsActiveToken();
                  var IsValidUser = await tokenMangerContext.CheckUserSecurityStamp(userClaim.Value, securityStamp.Value);
                  if (!checkLoggedUser)
                    context.Fail(errorFactory.InvalidToken());
                  if (!IsValidUser)
                    context.Fail(errorFactory.ThisUserIsAltered());
                },
          OnChallenge = context =>
                {
                  var errorFactory = context.HttpContext.RequestServices.GetRequiredService<IErrorFactory>();
                  if (context.AuthenticateFailure != null)
                    return Task.FromException(errorFactory.AccessDenied());
                  else
                    return Task.FromException(errorFactory.AccessDenied());
                }
        };
      });
    }
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
      services.AddApiVersioning(options =>
      {
        options.AssumeDefaultVersionWhenUnspecified = true;  //default => false;
        options.DefaultApiVersion = new ApiVersion(1, 0);   //v1.0 == v1
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
      });
    }
  }
}