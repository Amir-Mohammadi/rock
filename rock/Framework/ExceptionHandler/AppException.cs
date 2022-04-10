using System;
using System.Net;
namespace rock.Framework.ExceptionHandler
{
  public class AppException : Exception, IExceptionPayload
  {
    public HttpStatusCode HttpStatusCode { get; set; }

    public int Code { get; set; }
    public object Info { get; set; }
    public string Title { get ; set ; }

    public AppException(HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    {
      HttpStatusCode = httpStatusCode;
    }
    public AppException(
      int code,
      HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
      object additionalData = null
    ) : base("")
    {
      HttpStatusCode = httpStatusCode;
      Info = additionalData;
      Code = code;
    }
  }
}