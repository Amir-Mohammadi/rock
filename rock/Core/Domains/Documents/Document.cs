using System;
using System.Collections.Generic;
using rock.Core.Domains.Users;
using rock.Framework.Models;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Warehousing;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Forms;
namespace rock.Core.Domains.Documents
{
  public class Document : IEntity, IHasDescription
  {
    public int Id { get; set; }
    public int FormId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public virtual User User { get; set; }
    public virtual Form Form { get; set; }
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    public virtual ICollection<WarehouseTransaction> WarehouseTransactions { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
  }
}