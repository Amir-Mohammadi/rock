using System;
using rock.Filters;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Domains.Orders;
namespace rock.Models.ShopApi
{
  public class ShopOrderSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "categoryId")]
    public int? CategoryId { get; set; }
    [FromQuery(Name = "brandId")]
    public int? BrandId { get; set; }
    [FromQuery(Name = "status")]
    public OrderItemStatusType? Status { get; set; }
  }
}