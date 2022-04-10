using System.Collections.Generic;

namespace rock.Models.CustomerApi
{
  public class OrderShippingCostModel
  {
    public int TotalShippingCost { get; set; }
    public List<int> NotAvailableCartItems { get; set; }
  }
}