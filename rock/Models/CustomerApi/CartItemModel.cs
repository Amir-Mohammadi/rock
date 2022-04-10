using System;

namespace rock.Models.CustomerApi
{
  public class CartItemModel
  {
    public int Id { get; set; }
    public int Amount { get; set; }
    public ShoppingProductModel Product { get; set; }
    public bool IsAvailableShipping { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
