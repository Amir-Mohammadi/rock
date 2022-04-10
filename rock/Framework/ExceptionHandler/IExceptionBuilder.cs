using System;
using rock.Framework.Autofac;

namespace rock.Framework.ExceptionHandler
{
  public interface IExceptionFactory<T> : IScopedDependency where T : AppException
  {
    IExceptionBuilder<T> Failed();
    IExceptionBuilder<T> NotFound();
    IExceptionBuilder<T> Forbidden();
    IExceptionBuilder<T> Unauthorized();

  }

  public interface IExceptionBuilder<T>
  {
    IExceptionBuilder<T> UseCode(object code);
    IExceptionBuilder<T> UseInfo(object info);
    T AsException();
    IExceptionPayload AsPayload();
  }
}
