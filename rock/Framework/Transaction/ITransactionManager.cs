using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using rock.Framework.Autofac;
namespace rock.Framework.Transaction
{
  public interface ITransactionManager : IScopedDependency
  {
    IDbContextTransaction Transaction { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
  }
}