using System;
namespace rock.Models.MarketApi
{
  public class MarketStuffBrochureModel
  {
    public int Id { get; internal set; }
    public int ProductId { get; internal set; }
    public string Html { get; internal set; }
    public byte[] RowVersion { get; internal set; }
  }
}