using System;
using rock.Core.Domains.Orders;

namespace rock.Core.Domains.Financial
{
  public class BillItem
  {
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public int BillId { get; set; }

    public virtual OrderItem OrderItem { get; set; }
    public virtual Bill Bill { get; set; }

  }
}
