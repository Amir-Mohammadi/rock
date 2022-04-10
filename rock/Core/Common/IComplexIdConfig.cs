using System;
using System.Collections.Generic;

namespace rock.Core.Common
{
  public interface IComplexIdConfig<T>
  {
    string Seprator { get; }
    string Prefix { get; }
    T Shape { get; }
    void Map(IList<string> input, T output);
    void Map(T input, IList<string> output);
  }
}
