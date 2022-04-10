using System;
namespace rock.Models.MarketApi
{
  public class MarketStuffImageModel
  {
    public int Id { get; internal set; }
    public string ImageAlt { get; internal set; }
    public int Order { get; set; }
    public string ImageTitle { get; internal set; }
    public Guid ImageId { get; internal set; }
    public byte[] RowVersion { get; internal set; }
  }
}