using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;

namespace rock.Core.Services.Common
{
  public interface ITelephonyService : IScopedDependency
  {
    Task SendSMS(string phone, string message, CancellationToken cancellationToken);
  }
}
