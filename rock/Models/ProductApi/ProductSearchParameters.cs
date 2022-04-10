using System;
using Microsoft.AspNetCore.Mvc;
using rock.Filters;
namespace rock.Models.ProductApi
{
  public class ProductSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "name")]
    public string Name { get; set; }
    [FromQuery(Name = "brand_id")]
    public int? BrandId { get; set; }
    [FromQuery(Name = "category_id")]
    public int? ProductCategoryId { get; set; }
  }
}