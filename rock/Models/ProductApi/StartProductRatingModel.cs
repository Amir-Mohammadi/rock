using System;
using System.Collections.Generic;

namespace rock.Models.ProductApi
{
  public class StartProductRatingModel
  {
    public string Title { get; set; }

    public IList<string> Conditions { get; set; }
  }
}
