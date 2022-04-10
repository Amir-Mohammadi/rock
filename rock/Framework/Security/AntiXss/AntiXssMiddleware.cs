using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using rock.Core.Errors;
namespace rock.Framework.Security.AntiXss
{
  public class AntiXssMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IErrorFactory errorFactory;
    public AntiXssMiddleware(RequestDelegate next, IErrorFactory errorFactory)
    {
      this.next = next;
      this.errorFactory = errorFactory;
    }
    public async Task Invoke(HttpContext context)
    {
      // Check XSS in URL
      if (!string.IsNullOrWhiteSpace(context.Request.Path.Value))
      {
        var url = context.Request.Path.Value;
        if (CrossSiteScriptingValidation.IsDangerousString(url, out _))
        {
          throw errorFactory.XssDetect();
        }
      }
      // Check XSS in query string
      if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
      {
        var queryString = WebUtility.UrlDecode(context.Request.QueryString.Value);
        if (CrossSiteScriptingValidation.IsDangerousString(queryString, out _))
        {
          throw errorFactory.XssDetect();
        }
      }
      // Check XSS in request content
      var originalBody = context.Request.Body;
      try
      {
        var content = await ReadRequestBody(context);
        if (CrossSiteScriptingValidation.IsDangerousString(content, out _))
        {
          throw errorFactory.XssDetect();
        }
        await this.next(context).ConfigureAwait(false);
      }
      finally
      {
        context.Request.Body = originalBody;
      }
    }
    private static async Task<string> ReadRequestBody(HttpContext context)
    {
      var buffer = new MemoryStream();
      await context.Request.Body.CopyToAsync(buffer);
      context.Request.Body = buffer;
      buffer.Position = 0;
      var encoding = Encoding.UTF8;
      var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
      context.Request.Body.Position = 0;
      return requestContent;
    }
  }
}