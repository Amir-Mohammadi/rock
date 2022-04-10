using rock.Filters;
namespace rock.Models.ShopApi
{
  public class ShopSearchParameters : PagedListFilter
  {
    public int? CityId { get; set; }
  }
}