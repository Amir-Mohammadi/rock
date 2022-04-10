using System.Net;
using rock.Framework.ExceptionHandler;

namespace rock.Core.Common.Exception
{
  public class ForbiddenException : AppException
  {
    public ForbiddenException() : base(HttpStatusCode.Forbidden)
    {
    }

    public ForbiddenException(int code, object additionalData = null) : base(code, HttpStatusCode.Forbidden, additionalData)
    {
    }
  }
}