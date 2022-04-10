using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;
namespace rock.Framework.FileHandler
{
  public partial interface IFileService : IScopedDependency
  {
    Task<IFileResult> GetFileResultWithStreamById(Guid id, CancellationToken cancellationToken);
  }
}