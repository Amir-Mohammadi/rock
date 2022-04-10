using System;
using System.Collections.Generic;

namespace rock.Framework.ExceptionHandler
{
  public class DevelopmentExceptionPayload : IExceptionPayload
  {
    public int Code { get; set; }
    public object Info { get; set; }
    public string Title { get ; set ; }

    public List<object> Exceptions { get; set; } = new List<object>();
  }











}
