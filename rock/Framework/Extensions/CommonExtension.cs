using System.Linq;
using Microsoft.Data.SqlClient;

namespace rock.Framework.Extensions
{
  public static class CommonExtension
  {
    public static string GetOrderByQuery(this string orderby, SortOrder orderDirection)
    {
      var items = orderby.Split(',').Select(m =>   m + " " + orderDirection.ToString() );
      string query = string.Join(separator: ',', values: items);
      return query;
    }

    public static bool CompareRowVersion(this byte[] entity, byte[] rowVersion)
    {
      if (rowVersion.Length != entity.Length) return false;

      for (int index = 0; index < entity.Length; index++)
      {
        if (entity[index] != rowVersion[index])
          return false;
      }
      return true;
    }
  }
}