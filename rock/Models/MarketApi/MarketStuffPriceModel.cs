using rock.Core.Domains.Products;
using rock.Models.CommonApi;

namespace rock.Models.MarketApi
{
  public class MarketStuffPriceModel
  {
    public int Id { get; set; }
    public CityModel City { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public double Price { get; set; }
    public double MaxPrice { get; set; }
    public double MinPrice { get; set; }
    public double Discount { get; set; }
    public double DiscountedPrice { get; set; }

  }
}