using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using rock.Core.Common;
namespace rock.Framework.ExceptionHandler
{
  public class CustomExceptionHandlerMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IHostEnvironment hostEnvironment;
    public CustomExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
    {
      this.next = next;
      this.hostEnvironment = hostEnvironment;
    }
    private void importAppExceptionPayload(IExceptionPayload payload, AppException exception)
    {
      payload.Code = exception.Code;
      payload.Title = ErrorCodes.Resolve(exception.Code);
      payload.Info = exception.Info;
    }
    private object createExceptionPayloadForDevelopmentMode(Exception exception)
    {
      var payload = new
      {
        Message = exception.Message,
        StackTrace = exception.StackTrace
      };
      return payload;
    }
    public async Task Invoke(HttpContext httpContext)
    {
      var contractResolver = new DefaultContractResolver
      {
        NamingStrategy = new SnakeCaseNamingStrategy()
      };
      var jsonSettings = new JsonSerializerSettings
      {
        ContractResolver = contractResolver,
        Formatting = Formatting.Indented
      };
      string payload = null;
      try
      {
        await this.next(httpContext);
      }
      catch (Exception exception)
      {
        AppException appException = exception as AppException;
        if (hostEnvironment.IsDevelopment())
        {
          var exceptionToSerialize = new DevelopmentExceptionPayload();
          exceptionToSerialize.Exceptions.Add(createExceptionPayloadForDevelopmentMode(exception));
          if (exception.InnerException != null)
          {
            exceptionToSerialize.Exceptions.Add(createExceptionPayloadForDevelopmentMode(exception.InnerException));
          }
          if (appException != null)
          {
            importAppExceptionPayload(exceptionToSerialize, appException);
          }
          else
          {
            exceptionToSerialize.Code = ErrorCodes.INTERNAL_SERVER_ERROR;
            exceptionToSerialize.Title = ErrorCodes.Resolve(exceptionToSerialize.Code);
            exceptionToSerialize.Info = exception.InnerException == null ? exception.Message : exception.InnerException.Message;
          }
          payload = JsonConvert.SerializeObject(exceptionToSerialize, jsonSettings);
        }
        else
        {
          var exceptionToSerialize = new ExceptionPayload();
          if (appException != null)
          {
            importAppExceptionPayload(exceptionToSerialize, appException);
          }
          else
          {
            exceptionToSerialize.Code = ErrorCodes.INTERNAL_SERVER_ERROR;
            exceptionToSerialize.Title = ErrorCodes.Resolve(ErrorCodes.INTERNAL_SERVER_ERROR);
            exceptionToSerialize.Info = "An internal server error has occurred.";
          }
          payload = JsonConvert.SerializeObject(exceptionToSerialize, jsonSettings);
        }
        httpContext.Response.StatusCode = appException != null ? (int)appException.HttpStatusCode : (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(payload);
      }
    }
  }
}