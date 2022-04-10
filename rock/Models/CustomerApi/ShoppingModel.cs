using System;
using System.Collections.Generic;
using rock.Core.Domains.Orders;

namespace rock.Models.CustomerApi
{
  public class ShoppingModel
  {
    public int Id { get; set; }
    public int TrackingId { get; set; }
    public DateTime PurchasedAt { get; set; }
    public Cart Cart { get; set; }
    public ShoppingStatus Status { get; set; }
    public IList<ShoppingStatusHistoryModel> History { get; set; }
  }
}
