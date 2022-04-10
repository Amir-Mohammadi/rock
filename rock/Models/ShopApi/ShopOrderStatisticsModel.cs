namespace rock.Models.ShopApi
{
  public class ShopOrderStatisticsModel
  {
    public int TotalOrderCount { get; set; }
    public int CanceledOrderCount { get; set; }
    public int RejectedTotalOrderCount { get; set; }
    public int SentTotalOrderCount { get; set; }
  }
}