using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Extensions;
namespace rock.Core.Common
{
  [Serializable]
  public class PagedList<T> : List<T>, IPagedList<T>
  {
    private PagedList()
    {
    }
    public static async Task<PagedList<T>> CreatePagedListAsync<W>(IQueryable<W> source,
                                                             int pageIndex,
                                                             int pageSize,
                                                             Func<W, T> convertFunction,
                                                             CancellationToken cancellationToken)
    {
      var totalCount = await source.CountAsync();
      if (pageIndex * pageSize > 0)
        source = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
      var list = await source.AsQueryable().ToListAsync(cancellationToken);
      var items = list.Convert(convertFunction);
      var result = new PagedList<T>(source: items,
                                    pageIndex: pageIndex,
                                    pageSize: pageSize,
                                    totalCount: totalCount);
      return result;
    }
    public static PagedList<T> CreatePagedList<W>(IQueryable<W> source,
                                            int pageIndex,
                                            int pageSize,
                                            Func<W, T> convertFunction)
    {
      var totalCount = source.Count();
      if (pageIndex * pageSize > 0)
        source = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
      var list = source.AsQueryable().ToList();
      var items = list.Convert(convertFunction);
      var result = new PagedList<T>(source: items,
                                    pageIndex: pageIndex,
                                    pageSize: pageSize,
                                    totalCount: totalCount);
      return result;
    }
    public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
    {
      calculatePageInfo(pageIndex: pageIndex,
                        pageSize: pageSize,
                        totalCount: totalCount);
      TotalCount = totalCount;
      PageSize = pageSize;
      PageIndex = pageIndex;
      AddRange(source);
    }
    void calculatePageInfo(int pageIndex, int pageSize, int totalCount)
    {
      this.TotalCount = totalCount;
      this.PageSize = pageSize;
      this.PageIndex = pageIndex;
      if (pageSize * pageIndex > 0)
      {
        this.TotalPages = totalCount / pageSize;
        if (totalCount % pageSize > 0)
          TotalPages++;
      }
    }
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => PageIndex > 0;
    public bool HasNextPage => PageIndex + 1 < TotalPages;
  }
}