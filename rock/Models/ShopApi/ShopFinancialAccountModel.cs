using rock.Models.CommonApi;
using rock.Models.FinancialApi;

namespace rock.Models.ShopApi
{
  public class ShopFinancialAccountModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int? BankId { get; set; }
    public int CurrencyId { get; set; }
    public string No { get; set; }
    public virtual BankModel Bank { get; set; }
    public CurrencyModel Currency { get; set; }
    public byte[] RowVersion { get; set; }
  }
}