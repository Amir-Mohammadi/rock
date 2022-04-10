using System.Net;
using rock.Framework.ExceptionHandler;

namespace rock.Core.Common.Exception
{
  public class FailedException : AppException
  {
    public FailedException() : base(HttpStatusCode.BadRequest)
    {
    }

    public FailedException(int code, object additionalData = null) : base(code, HttpStatusCode.BadRequest, additionalData)
    {
    }
  }
}