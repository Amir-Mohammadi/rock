using System;
using rock.Framework.ExceptionHandler;

namespace rock.Core.Common.Exception
{
  public class ExceptionFactory : IExceptionFactory<AppException>
  {
    public IExceptionBuilder<AppException> Failed()
    {
      return new Builder(new FailedException());
    }

    public IExceptionBuilder<AppException> Forbidden()
    {
      return new Builder(new ForbiddenException());
    }

    public IExceptionBuilder<AppException> NotFound()
    {
      return new Builder(new NotfoundException());
    }

    public IExceptionBuilder<AppException> Unauthorized()
    {
      return new Builder(new UnauthorizedException());
    }

    class Builder : IExceptionBuilder<AppException>
    {
      private AppException _exception;

      public Builder(AppException exception)
      {
        _exception = exception;
      }

      public AppException AsException()
      {
        return _exception;
      }

      public IExceptionPayload AsPayload()
      {
        return _exception;
      }

      public IExceptionBuilder<AppException> UseCode(object code)
      {
        _exception.Code = (int)code;
        return this;
      }

      public IExceptionBuilder<AppException> UseInfo(object info)
      {
        _exception.Info = info;
        return this;
      }
    }
  }


}
