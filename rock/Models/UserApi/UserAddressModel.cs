using rock.Models.CommonApi;

namespace rock.Models.UserApi
{
  public class UserAddressModel
  {
    public int Id { get; set; }
    public CityModel City { get; set; }
    public string Description { get; set; }
    public string PostalCode { get; set; }
    public string Phone { get; set; }
    public string AddressOwnerName { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }
  }
}