namespace rock.Models.CommonApi
{
  public class TransportationModel
  {
    public int Id { get; set; }
    public CityModel FromCity { get; set; }
    public CityModel ToCity { get; set; }
    public int Cost { get; set; }
    public int Distance { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}