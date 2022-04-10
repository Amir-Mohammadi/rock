using System;
using System.Collections.Generic;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Forms;
using rock.Core.Domains.Orders;
namespace rock.Models.ShopApi
{
    public class ShopDocumentModel
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual Form Form { get; set; }
        public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}