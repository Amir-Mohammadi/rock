using Microsoft.AspNetCore.Mvc;
namespace rock.Filters
{
  public class PagedListFilter
  {
    [FromQuery(Name = "q")]
    public string Q { get; set; }
    [FromQuery(Name = "page_index")]
    public int PageIndex { get; set; }
    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; }
    [FromQuery(Name = "sort_by")]
    public string SortBy { get; set; }
    [FromQuery(Name = "order")]
    public string Order { get; set; }
  }
}