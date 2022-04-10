namespace rock.Models.FinancialApi
{
  public class FinancialProfileModel
  {
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public FinancialPersonProfileModel PersonProfile { get; set; }

  }
}