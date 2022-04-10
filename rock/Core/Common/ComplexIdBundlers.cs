using System;
using System.Collections.Generic;
using System.Linq;

namespace rock.Core.Common
{
  public class ComplexIdBundlerService: IComplexIdBundlerService
  {
    public IComplexIdBundler<T> Use<T>(IComplexIdConfig<T> config)
    {
      return new Bundler<T>(config);
    }

    class Bundler<T> : IComplexIdBundler<T>
    {
      private IComplexIdConfig<T> config;

      public Bundler(IComplexIdConfig<T> config)
      {
        this.config = config;
      }

      public string Pack(Action<T> reqs)
      {
        var req = config.Shape;
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(config.Prefix) && !string.IsNullOrEmpty(config.Prefix))
        {
          parts.Add(config.Prefix);
        }
        reqs.Invoke(req);
        config.Map(req, parts);
        return string.Join(config.Seprator, parts);
      }

      public T Unpack(string complexId)
      {
        var req = config.Shape;
        var parts = complexId.Split(config.Seprator);
        if (!string.IsNullOrWhiteSpace(config.Prefix) && !string.IsNullOrEmpty(config.Prefix))
        {
          parts = parts.Skip(1).ToArray();
        }
        config.Map(parts.ToList(), req);
        return req;
      }
    }
  }
}
