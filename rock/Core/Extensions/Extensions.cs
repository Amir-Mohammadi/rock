using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using Newtonsoft.Json;
namespace rock.Core.Extensions
{
  public static class Extensions
  {
    public static IList<TResult> Convert<TInput, TResult>(this IList<TInput> source, Func<TInput, TResult> convertFunction)
    {
      return source.Select<TInput, TResult>(item => convertFunction.Invoke(item)).ToList();
    }
    public static string FirstCharToUpper(this string input) =>
           input switch
           {
             null => throw new ArgumentNullException(nameof(input)),
             "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
             _ => input.First().ToString().ToUpper() + input.Substring(1)
           };
    public static string GetSortBy(string sort)
    {
      if (sort == null)
        return sort;
      var sortBy = string.Concat(sort.Split("_").Select(m => m.FirstCharToUpper()));
      sortBy = string.Join('.', sortBy.Split(".").Select(m => m.FirstCharToUpper()));
      return sortBy;
    }
    public static bool IsDescending(string order)
    {
      if (string.IsNullOrEmpty(order))
        return false;
      return order.ToLower() == "desc" || order.ToLower() == "descending";
    }
    public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, string sortBy, bool convert = true, string order = null)
    {
      var x = source.ToArray();
      if (!string.IsNullOrEmpty(sortBy))
      {
        if (convert)
        {
          sortBy = GetSortBy(sortBy);
        }
        if (order.ToLower() == "desc" || order.ToLower() == "descending")
        {
          source = source.AsQueryable().OrderByDescendingDynamic(x => "x." + sortBy).AsQueryable();
        }
        else
        {
          source = source.AsQueryable().OrderByDynamic(x => "x." + sortBy).AsQueryable();
        }
      }
      return source;
    }
    public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, string order)
    {
      if (string.IsNullOrEmpty(order))
      {
        return source.AsQueryable().OrderByDescending(keySelector).AsQueryable();
      }
      if (order.ToLower() == "desc" || order.ToLower() == "descending")
      {
        source = source.AsQueryable().OrderByDescending(keySelector).AsQueryable();
      }
      else
      {
        source = source.AsQueryable().OrderBy(keySelector).AsQueryable();
      }
      return source;
    }
    public static IQueryable<T> Paging<T>(this IQueryable<T> query, int pageSize, int pageIndex)
    {
      if (pageIndex * pageSize <= 0) return query;
      if (pageIndex <= 0) throw new ArgumentOutOfRangeException(nameof(pageIndex));
      if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));
      var skip = (pageIndex - 1) * pageSize;
      return query.Skip(skip).Take(pageSize);
    }
    public static StringContent GetStringContent(this object obj, Encoding encoding)
        => new StringContent(JsonConvert.SerializeObject(obj), encoding, "application/json");
  }
}