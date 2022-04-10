using System.Net;
using rock.Framework.ExceptionHandler;

namespace rock.Core.Common.Exception
{
  public class UnauthorizedException : AppException
  {
    public UnauthorizedException() : base(HttpStatusCode.NotFound)
    {
    }

    public UnauthorizedException(int code, object additionalData = null) : base(code, HttpStatusCode.Forbidden, additionalData)
    {
    }
  }
}