using rock.Filters;
namespace rock.Models.FileApi
{
  public class FileSearchParameters : PagedListFilter
  {
    public string Tag { get; set; }
  }
}