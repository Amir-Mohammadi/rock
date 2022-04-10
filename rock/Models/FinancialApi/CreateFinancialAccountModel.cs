
using rock.Core.Domains.Financial;
namespace rock.Models.FinancialApi
{
  public class CreateFinancialAccountModel
  {
    public int CurrencyId { get; set; }
    public int BankId { get; set; }
    public int ProfileId { get; set; }
    public string Title { get; set; }
    public string No { get; set; }
    public FinancialAccountType Type { get; set; }
  }
}