using System;
using System.Collections.Generic;
using rock.Core.Domains.Documents;
using rock.Core.Domains.Orders;
using rock.Framework.Models;
namespace rock.Core.Domains.Financial
{
  public class Purchase : IEntity, IHasDescription
  {
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public string PaymentTransactionNo { get; set; }
    public string PaymentSapNo { get; set; }
    public string PaymentPayload { get; set; }
    public bool IsVerfied { get; set; }
    public string VerifyPayload { get; set; }
    public string RRN { get; set; }
    public int OrderId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Document Document { get; set; }
    public virtual Order Order { get; set; }
    public virtual ICollection<PurchaseItem> Items { get; set; }
  }
}