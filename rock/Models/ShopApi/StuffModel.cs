using System;
using rock.Models.MarketApi;

namespace rock.Models.ShopApi
{
  public class StuffModel
  {
    public int Id { get; internal set; }
    public int? ShopStuffPriceStuffId { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string BrandName { get; set; }
    public int ColorId { get; set; }
    public string ColorName { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public Guid PreviewMarketStuffImageId { get; set; }
    public byte[] PreviewMarketStuffImageRowVersion { get; set; }

  }
}