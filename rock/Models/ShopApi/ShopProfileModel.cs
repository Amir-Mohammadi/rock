using rock.Models.CommonApi;

namespace rock.Models.ShopApi
{
  public class ShopProfileModel
  {
    public int Id { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    public string Website { get; set; }
    public CityModel City { get; set; }
  }
}