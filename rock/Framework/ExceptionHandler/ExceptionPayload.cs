using System;

namespace rock.Framework.ExceptionHandler
{
  public class ExceptionPayload : IExceptionPayload
  {
    public int Code { get; set; }
    public string Title { get ; set ; }

    public object Info { get; set; }
  }
}