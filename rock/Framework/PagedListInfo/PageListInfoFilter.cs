using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using rock.Core.Common;
namespace rock.Framework.PagedListInfo
{
  public class PageListInfoFilter : IActionFilter
  {
    public void OnActionExecuted(ActionExecutedContext context)
    {
      var result = context.Result as ObjectResult;
      if (result?.Value is IPagedModel pagedResult)
      {
        var metadata = new
        {
          pagedResult.TotalCount,
          pagedResult.PageSize,
          pagedResult.PageIndex,
          pagedResult.TotalPages,
          pagedResult.HasNextPage,
          pagedResult.HasPreviousPage
        };
        context.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      }
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
  }
}