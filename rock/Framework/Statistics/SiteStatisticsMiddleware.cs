using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using rock.Core.Services.Products;
namespace rock.Framework.Statistics
{
  public class SiteStatisticsMiddleware
  {
    #region Fields
    private readonly RequestDelegate next;
    private readonly IProductService productService;
    #endregion
    #region Constractor
    public SiteStatisticsMiddleware(RequestDelegate next,
                                    IProductService productService)
    {
      this.next = next;
      this.productService = productService;
    }
    #endregion
    #region Invoke
    public async Task Invoke(HttpContext context)
    {
      await next(context);
      var request = context.Request;
      if (request.Method == "GET"
          && Convert.ToString(request.RouteValues["controller"]) == "MarketApi"
          && Convert.ToString(request.RouteValues["action"]) == "GetMarketStuff")
      {
        var productId = int.Parse(Convert.ToString(context.Request.RouteValues["marketStuffId"]));
        await productService.VisitProductById(id: productId, cancellationToken: CancellationToken.None);
      }
    }
    #endregion
  }
}