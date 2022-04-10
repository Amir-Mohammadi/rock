using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Users;
using rock.Models.CommonApi;
namespace rock.Models.UserApi
{
  public class UserModel
  {
    public int Id { get; set; }
    public bool Enabled { get; set; }
    public UserRole Roles { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string NationalCode { get; set; }
    public DateTime? Birthday { get; set; }
    public string EconomicCode { get; set; }
    public string FatherName { get; set; }
    public string FirstName { get; set; }
    public CityModel City { get; set; }
    public Gender? Gender { get; set; }
    public string LastName { get; set; }
    public Guid? PictureId { get; set; }
    public int? cityId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}