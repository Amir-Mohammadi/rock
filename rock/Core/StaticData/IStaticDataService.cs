using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;

namespace rock.Core.StaticData
{
  public interface IStaticDataService : IScopedDependency
  {
    Task Init();
  }
}