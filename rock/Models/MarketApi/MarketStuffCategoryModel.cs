namespace rock.Models.MarketApi
{
  public class MarketStuffCategoryModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public string Explanation { get; set; }
    public int? ParentId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}