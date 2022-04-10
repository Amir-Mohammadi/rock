using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rock.Models.ProductApi
{
  public class FillProductPropertyModel
  {
    [Required]
    public IList<string> Values { get; set; }
  }
}
