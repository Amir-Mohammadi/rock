namespace rock.Models.ProductApi
{
  public class ProductShippingInfoModel
  {
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public byte[] Rowversion { get; set; }
  }
}