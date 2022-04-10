using System;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Serialization;

namespace rock.Factories
{
  public class SortNameResolver<T>
  {
    private string sortBy;

    public SortNameResolver(string sortBy)
    {
      this.sortBy = sortBy.ToLower();
    }

    private string resolve(Expression<Func<T, Object>> s)
    {
      var name = "";
      if (s.Body is MemberExpression)
      {
        name = ((MemberExpression)s.Body).Member.Name;
      }
      else
      {
        var op = ((UnaryExpression)s.Body).Operand;
        name = ((MemberExpression)op).Member.Name;
      }
      var resolver = new SnakeCaseNamingStrategy();
      return resolver.GetPropertyName(name, false);
    }

    public bool Match(Expression<Func<T, Object>> s)
    {
      return resolve(s) == sortBy;
    }



  }
}
