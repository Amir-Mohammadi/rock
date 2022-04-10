using Microsoft.AspNetCore.Mvc;

namespace rock.Models.CommonApi
{
    public class ProvincesSearchParameters
    {
      [FromQuery]
      public string Name { get; set; }
        
    }
}