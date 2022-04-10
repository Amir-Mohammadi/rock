using System;
namespace rock.Models.MarketApi
{
  public class MarketStuffAnswerModel
  {
    public int Id { get; internal set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Payload { get; set; }
    public int? ProfileId { get; internal set; }
    public int? UserId { get; internal set; }
    public byte[] RowVersion { get; internal set; }
  }
}