using System;
using Microsoft.AspNetCore.Mvc;
using rock.Filters;

namespace rock.Models.ProductApi
{
  public class ProductCategorySearchParameters
  {
    [FromQuery(Name = "name")]
    public string Name { get; set; }
  }
}
