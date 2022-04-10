using System;
namespace rock.Models.MarketApi
{
  public class MarketStuffCommentModel
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProfileId { get; set; }
    public string FullName => FirstName + " " + LastName;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}