using System;
using System.Collections.Generic;
using rock.Core.Domains.Orders;
namespace rock.Models.OrderApi
{
  public class OrderModel
  {
    public int Id { get; set; }
    public OrderCartModel Cart { get; set; }
    public ICollection<OrderItemModel> Items { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}