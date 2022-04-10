namespace rock.Models.OrderApi
{
  public class TransportModel
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int CustomCost { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}