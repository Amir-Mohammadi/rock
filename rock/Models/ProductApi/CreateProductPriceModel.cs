namespace rock.Models.ProductApi
{
  public class CreateProductPriceModel
  {
    public int CityId { get; set; }
    public int ColorId { get; set; }
    public double Price { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public double Discount { get; set; }
    public bool IsPublished { get; set; }
  }
}