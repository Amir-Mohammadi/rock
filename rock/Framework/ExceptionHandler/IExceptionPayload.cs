using System;

namespace rock.Framework.ExceptionHandler
{
  public interface IExceptionPayload
  {
    int Code { get; set; }
    string Title { get; set; }
    object Info { get; set; }
  }
}
