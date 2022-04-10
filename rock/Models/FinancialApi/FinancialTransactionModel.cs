using rock.Core.Domains.Documents;
using rock.Core.Domains.Financial;
namespace rock.Models.FinancialApi
{
  public class FinancialTransactionModel
  {
    public int Id { get; set; }
    public int Factor { get; set; }
    public double Amount { get; set; }
    public Document Document { get; set; }
    public FinancialAccount Account { get; set; }
  }
}