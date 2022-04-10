namespace rock.Models.UserApi
{
  public class SaveUserAddressModel
  {
    public int CityId { get; set; }
    public string Description { get; set; }
    public string PostalCode { get; set; }
    public string Phone { get; set; }
    public string AddressOwnerName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}