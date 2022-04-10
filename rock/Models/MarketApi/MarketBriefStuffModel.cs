
using System;
using rock.Core.Domains.Products;
using rock.Models.ProductApi;
namespace rock.Models.MarketApi
{
  public class MarketBriefStuffModel
  {
    public int Id { get; internal set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string BrandName { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public MarketStuffImageModel PreviewMarketStuffImage { get; set; }
    public string MetaDescription { get; set; }
    public string BriefDescription { get; set; }
  }
}