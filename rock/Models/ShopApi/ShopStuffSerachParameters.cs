using System;
using rock.Filters;
using Microsoft.AspNetCore.Mvc;

namespace rock.Models.ShopApi
{
  public class ShopStuffSearchParameters : PagedListFilter
  {
    public int? ColorId { get; set; }
    public int? StuffId { get; set; }
    [FromQuery(Name = "categoryId")]
    public int? CategoryId { get; set; }
    [FromQuery(Name = "brandId")]
    public int? BrandId { get; set; }
  }
}