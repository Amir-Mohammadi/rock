namespace rock.Core.Services.Tipax.TipaxResponse
{
  public class TipaxCalculateContractResult
  {
    public double ShippingCost { get; set; }
    public double SpecialService { get; set; }
    public double PackageService { get; set; }
    public double Tax { get; set; }
    public double Discount { get; set; }
    public double FinalAmount { get; set; }
    public double CitiesDistance { get; set; }
    public double ExtraServiceAmount { get; set; }
  }

}