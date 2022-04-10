using rock.Filters;
namespace rock.Models.WarehousingApi
{
  public class InventorySearchParameters : PagedListFilter
  {
    public int? ProductId { get; set; }
    public int? ColorId { get; set; }
  }
}