using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using rock.Filters;

namespace rock.Models.CommonApi
{
    public class CitySearchParameters: PagedListFilter
    {
        [FromQuery]
        public string Name {get; set;}
    }
}