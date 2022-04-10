using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rock.Core.StaticData;
namespace rock
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var webHost = CreateHostBuilder(args).Build();
      var staticDataService = webHost.Services.GetRequiredService<IStaticDataService>();
      await staticDataService.Init();
        webHost.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
      .UseServiceProviderFactory(new AutofacServiceProviderFactory())
      .ConfigureWebHostDefaults(webBuilder =>
      {
        webBuilder.UseStartup<Startup>().UseSetting("detailedErrors", "true");
      });
  }
}