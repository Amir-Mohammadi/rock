namespace rock.Models.CustomerApi
{
  public class ShoppingProductPriceModel
  {
    public int Id { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public ShoppingCartCityModel City { get; set; }
  }
}