using System;
namespace rock.Models.MarketApi
{
  public class MarketStuffQuestionModel
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProfileId { get; set; }
    public string Payload { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public MarketStuffAnswerModel[] Answers { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
