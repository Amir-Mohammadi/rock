using System;
using System.Threading.Tasks;
using rock.Framework.Autofac;
namespace rock.Framework.FileHandler
{
  public interface IFileCacheService : ISingletonDependency
  {
    Task<IFileResult> Get(Guid id, string rowVersion);
  }
}