using System;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Forms;
using rock.Models.FinancialApi;

namespace rock.Models.ShopApi
{
  public class ShopFinancialTransactionModel
  {
    public int Id { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public TransactionType TransactionType { get; set; }
    public virtual FinancialFormModel FinancialForm { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}