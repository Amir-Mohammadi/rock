using System;
using rock.Core.Domains.Orders;
using rock.Core.Services.Payment;
using rock.Framework.Models;

namespace rock.Core.Domains.Payment
{
  public class OrderPayment : ITimestamp, IRemovable, IEntity
  {
    public OrderPayment()
    {
      this.Visited = false;
    }
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Nullable<int> BankTransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int Amount { get; set; }
    public bool Visited { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public virtual Order Order { get; set; }
    public virtual BankTransaction BankTransaction { get; set; }
    public byte[] RowVersion { get; set; }
  }
}