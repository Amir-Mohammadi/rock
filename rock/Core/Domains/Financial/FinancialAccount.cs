using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Payment;
using rock.Core.Domains.Profiles;
using rock.Framework.Models;

namespace rock.Core.Domains.Financial
{
  public class FinancialAccount : IEntity
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int ProfileId { get; set; }
    public int? BankId { get; set; }
    public int CurrencyId { get; set; }
    public FinancialAccountType Type { get; set; }
    public string No { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Bank Bank { get; set; }
    public Currency Currency { get; set; }
    public virtual Profile Profile { get; set; }
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    public virtual ICollection<PaymentGateway> PaymentGateways { get; set; }
  }
}
