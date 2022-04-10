using System.Collections.Generic;
using rock.Core.Domains.Documents;
using rock.Core.Domains.Financial;
namespace rock.Models.FinancialApi
{
  public class BillModel
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public Document Document { get; set; }
    public ICollection<BillItem> Items { get; set; }
    public byte[] RowVersion { get; set; }
  }
}