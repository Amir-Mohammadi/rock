using System;
using System.Collections.Generic;
using rock.Core.Domains.Orders;

namespace rock.Models.OrderApi
{
  public class GetOrderModel
  {
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public double TotalPrice { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
