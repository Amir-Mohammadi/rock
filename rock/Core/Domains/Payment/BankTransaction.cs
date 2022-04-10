using System;
using rock.Core.Domains.Users;
using rock.Core.Services.Payment;
using rock.Framework.Models;
namespace rock.Core.Domains.Payment
{
  public class BankTransaction : IEntity, ITimestamp, IHasRowVersion
  {
    public int Id { get; set; }
    public string OrderPaymentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string PaymentKind { get; set; }
    public string PaymentGateway { get; set; }
    public string TerminalId { get; set; }
    public string CardNo { get; set; }
    public string BankParameter1 { get; set; }
    public string BankParameter2 { get; set; }
    public string BankParameter3 { get; set; }
    public string BankParameter4 { get; set; }
    public int Amount { get; set; }
    public BankTransactionState State { get; set; }
    public int UserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual OrderPayment OrderPayment { get; set; }
  }
}