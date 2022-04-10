using System;
using System.Collections.Generic;
using rock.Models.UserApi;

namespace rock.Models.CustomerApi
{
  public class CartModel
  {
    public int Id { get; set; }
    public CartBillModel CartBill { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UserAddressId { get; set; }
    public IList<CartItemModel> CartItems { get; set; }
    public byte[] RowVersion { get; set; }
  }
}