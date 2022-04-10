using System.Net;
using rock.Framework.ExceptionHandler;

namespace rock.Core.Common.Exception
{
  public class NotfoundException : AppException
  {
    public NotfoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public NotfoundException(int code, object additionalData = null) : base(code, HttpStatusCode.Forbidden, additionalData)
    {
    }
  }
}