using rock.Core.Domains.Financial;

namespace rock.Models.FinancialApi
{
  public class FinancialAccountModel
  {
    public int Id { get; set; }
    public FinancialCurrencyModel Currency { get; set; }
    public FinancialBankModel Bank { get; set; }
    public FinancialProfileModel Profile { get; set; }
    public string Title { get; set; }
    public string No { get; set; }
    public FinancialAccountType Type { get; set; }
    public byte[] RowVersion { get; set; }
  }
}