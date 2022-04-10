using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
namespace rock.Framework.StaticFile
{
  public static class StaticFileConfigurationExtensions
  {
    public static void ConfigureStaticFiles(this IApplicationBuilder builder)
    {
      builder.UseStaticFiles();
      DefaultFilesOptions options = new DefaultFilesOptions();
      options.DefaultFileNames.Clear();
      options.DefaultFileNames.Add("index.html");
      builder.UseDefaultFiles(options);
      var assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      var wwwrootDirectory = Path.Combine(assemblyDirectory, "wwwroot");
      builder.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(wwwrootDirectory),
        OnPrepareResponse = ctx =>
        {
          ctx.Context.Response.Headers[HeaderNames.CacheControl] = Startup.SiteSetting.CacheControl;
          ctx.Context.Response.Headers[HeaderNames.ETag] = Startup.SiteSetting.Version;
          ctx.Context.Response.Headers[HeaderNames.Expires] = DateTime.UtcNow.AddDays(10).ToString();
        }
      });
    }
  }
}