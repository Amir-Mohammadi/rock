namespace rock.Models.WarehousingApi
{
  public class InventoryModel
  {
    public int WarehouseId { get; internal set; }
    public int ProductId { get; internal set; }
    public int ColorId { get; internal set; }
    public decimal Amount { get; internal set; }
  }
}