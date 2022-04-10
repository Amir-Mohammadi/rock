using System.IO;
using System.IO.Compression;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using rock.Core.Common;
using rock.Core.Services.Payment;
using rock.Framework.Autofac;
using rock.Framework.Extensions;
using rock.Framework.PagedListInfo;
using rock.Framework.Setting;
using rock.Framework.Setting.PaymentSetting;
using rock.Framework.Setting.TransportSetting;
using rock.Framework.StaticFile;
using rock.Framework.Swagger;
using rock.OAuth;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace rock
{
  public class Startup
  {
    public static SiteSettings SiteSetting;
    readonly string AllowSpecificOrigins = "CorsPolicy";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      SiteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
    }
    public IConfiguration Configuration { get; }
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
      services.Configure<PaymentGatewaySettings>(Configuration.GetSection(nameof(PaymentGatewaySettings)));
      services.Configure<TipaxInfo>(Configuration.GetSection(nameof(TipaxInfo)));
      services.AddDistributedMemoryCache();
      services.AddHttpContextAccessor();
      services.AddContext(configuration: Configuration);
      services.AddCustomMvc(AllowSpecificOrigins);
      services.AddControllers(config =>
      {
        config.Filters.Add(new PageListInfoFilter());
      }).AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver()
        {
          NamingStrategy = new SnakeCaseNamingStrategy()
        };
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });
      services.AddCustomValidationResponse(new SnakeCaseNamingStrategy(), ErrorCodes.INVALID_MODEL_SUPPLIED, ErrorCodes.Resolve(ErrorCodes.INVALID_MODEL_SUPPLIED));
      services.AddRadisService(Configuration);
      services.AddJwtAuthentication(SiteSetting.TokenSettings);
      services.AddResponseCompression(options =>
      {
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "image/svg+xml" });
      });
      services.Configure<GzipCompressionProviderOptions>(options =>
       {
         options.Level = CompressionLevel.Fastest;
       });
      services.AddCustomApiVersioning();
      services.AddSwaggerGen(c =>
     {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "rock", Version = "v1" });
       var filePath = Path.Combine(System.AppContext.BaseDirectory, "rock.xml");
       c.IncludeXmlComments(filePath);
       c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
       {
         Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
         Name = "Authorization",
         In = ParameterLocation.Header,
         Type = SecuritySchemeType.ApiKey,
         Scheme = "Bearer"
       });
       c.OperationFilter<SecurityRequirementsOperationFilter>();
     });
      services.AddPayment();
      services.AddSwaggerGenNewtonsoftSupport();
      services.AddOAuthAuthorization(SiteSetting.TokenSettings);
    }
    public void ConfigureContainer(ContainerBuilder builder)
    {
      builder.AddAutofacDependencyServices();
    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseResponseCompression();
      //if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("v1/swagger.json", "rock v1");
          c.DocExpansion(DocExpansion.None);
        });
      }
      app.UseHttpStatusCode();
      app.UseCustomExceptionHandler();
      if (env.IsProduction())
      {
        // app.UseHttpsRedirection();
      }
      // app.UseAntiXssMiddleware();
      app.UseRouting();
      app.UseCors(AllowSpecificOrigins);
      app.MapWhen(
        predicate: context => context.Request.Path.ToString().Contains("document.ashx"),
        configuration: appBranch => { appBranch.UseFileHandler(); });
      if (env.IsProduction())
      {
        app.ConfigureStaticFiles();
      }
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseTransactionsPerRequest();
      app.UseStatistics();
      app.UseEndpoints(endpoints =>
          {
            endpoints.MapControllers();
          });
    }
  }
}