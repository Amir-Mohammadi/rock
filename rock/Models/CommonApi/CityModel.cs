namespace rock.Models.CommonApi
{
  public class CityModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ProvinceModel Province { get; internal set; }
    public byte[] RowVersion { get; set; }
    public int ProvinceId { get; set; }
  }
}