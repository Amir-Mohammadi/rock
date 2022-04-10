using Microsoft.AspNetCore.Builder;
using rock.Framework.ExceptionHandler;
using rock.Framework.FileHandler;
using rock.Framework.Security.AntiXss;
using rock.Framework.Transaction;
namespace rock.Framework.Extensions
{
  public static class MiddlewareExtensions
  {
    public static IApplicationBuilder UseTransactionsPerRequest(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<TransactionMiddleware>();
    }
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
    public static IApplicationBuilder UseFileHandler(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<FileHandlerMiddleware>();
    }
    public static IApplicationBuilder UseHttpStatusCode(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<HttpStatusCodeHandler.HttpStatusCodeMiddleware>();
    }
    public static IApplicationBuilder UseStatistics(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<Statistics.SiteStatisticsMiddleware>();
    }
    public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<AntiXssMiddleware>();
    }
  }
}