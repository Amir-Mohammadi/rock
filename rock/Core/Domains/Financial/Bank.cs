using System;
using System.Collections.Generic;
using rock.Framework.Models;

namespace rock.Core.Domains.Financial
{
  public class Bank : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; }

  }
}
