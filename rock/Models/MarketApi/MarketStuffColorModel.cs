using rock.Models.CommonApi;
namespace rock.Models.MarketApi
{
  public class MarketStuffColorModel
  {
    public int ProductId { get; internal set; }
    public int ColorId { get; set; }
    public int? Code { get; set; }
    public ColorModel Color { get; internal set; }
  }
}