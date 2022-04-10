using System;
using rock.Core.Domains.Profiles;
using rock.Models.CommonApi;
namespace rock.Models.UserApi
{
  public class EditUserModel
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string NationalCode { get; set; }
    public int? CityId { get; set; }
    public DateTime? Birthday { get; set; }
    public string EconomicCode { get; set; }
    public string FatherName { get; set; }
    public string FirstName { get; set; }
    public Gender? Gender { get; set; }
    public string LastName { get; set; }
    public Guid? PictureId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}