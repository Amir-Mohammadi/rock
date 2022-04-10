using System;

namespace rock.Models.ShopApi
{
  public class ShopInventoryModel
  {
    public int WarehouseId { get; set; }
    public decimal Amount { get; set; }
    public int ColorId { get; set; }
    public string ColorName { get; set; }
    public int ProductId { get; set; }
  }
}
