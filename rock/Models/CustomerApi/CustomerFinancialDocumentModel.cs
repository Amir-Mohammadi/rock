using System;
using System.Collections.Generic;
using System.Security.Principal;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Forms;

namespace rock.Models.CustomerApi
{
  public class CustomerDocumentModel
  {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public Form Form { get; set; }
    public ICollection<FinancialTransaction> Transactions { get; set; }
  }
}
