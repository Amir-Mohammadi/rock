using System;
using System.Collections.Generic;
using System.Linq;
using rock.Core.Common.Exception;
using rock.Framework.Autofac;

namespace rock.Core.Common
{

  public interface IComplexIdBundlerService : IScopedDependency
  {
    IComplexIdBundler<T> Use<T>(IComplexIdConfig<T> config);

  }
  public interface IComplexIdBundler<T>
  {
    string Pack(Action<T> reqs);
    T Unpack(string complexId);
  }

}
