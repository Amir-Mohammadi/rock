using System;
namespace rock.Models.MarketApi
{
  public class MarketBrandModel
  {
    public int Id { get; internal set; }
    public string Name { get; internal set; }
    public string UrlTitle { get; internal set; }
    public string BrowserTitle { get; internal set; }
    public string MetaDescription { get; internal set; }
    public string ImageAlt { get; internal set; }
    public string ImageTitle { get; internal set; }
    public Guid ImageId { get; internal set; }
    public int? ProfileId { get; internal set; }
    public byte[] RowVersion { get; internal set; }
    public string Description { get; internal set; }
  }
}