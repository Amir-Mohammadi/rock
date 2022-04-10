using System;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Documents;
using rock.Framework.Models;

namespace rock.Core.Domains.Financial
{
  public class FinancialTransaction : IEntity, IRemovable
  {
    public int Id { get; set; }
    public TransactionType Factor { get; set; }
    public int Amount { get; set; }
    public int DocumentId { get; set; }
    public int FinancialAccountId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Document Document { get; set; }
    public virtual FinancialAccount Account { get; set; }

  }
}