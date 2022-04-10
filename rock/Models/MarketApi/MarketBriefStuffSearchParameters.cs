using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using rock.Filters;
namespace rock.Models.MarketApi
{
  public class MarketBriefStuffSearchParameters : PagedListFilter
  {
    [FromQuery(Name = "brands")]
    public ICollection<int> Brands { get; set; }
    [FromQuery(Name = "categories")]
    public ICollection<int> Categories { get; set; }
    [FromQuery(Name = "has_selling_stock")]
    public bool? HasSellingStock { get; set; }
    [FromQuery(Name = "min_price")]
    public int? MinmumPrice { get; set; }
    [FromQuery(Name = "max_price")]
    public int? MaximumPrice { get; set; }
    [FromQuery(Name = "discounted")]
    public bool? Discounted { get; set; }
    [FromQuery(Name = "esp")]
    public ExtraSearchParameter[] ExtraSearchParameters { get; set; }
  }
  public class ExtraSearchParameter
  {
    public string Key { get; set; }
    public string[] Values { get; set; }
  }
}