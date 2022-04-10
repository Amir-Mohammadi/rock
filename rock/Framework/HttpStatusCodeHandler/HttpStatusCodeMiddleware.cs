using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace rock.Framework.HttpStatusCodeHandler
{
  public class HttpStatusCodeMiddleware
  {
    private readonly RequestDelegate next;
    public HttpStatusCodeMiddleware(RequestDelegate next)
    {
      this.next = next;
    }
    public async Task Invoke(HttpContext context)
    {
      if (context.Request.Method == "POST")
      {
        context.Response.StatusCode = 201;
      }
      else
      {
        context.Response.StatusCode = 200;
      }
      await next(context);
    }
  }
}