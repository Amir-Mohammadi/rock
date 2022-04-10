using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rock.Core.Common;
using rock.Core.Domains.Profiles;
using rock.Models.UserApi;
namespace rock.Controllers
{
  [ApiController]
  public class BaseController : ControllerBase
  {
    protected internal void mapProfileAddress(ProfileAddress profileAddress, SaveUserAddressModel model)
    {
      profileAddress.CityId = profileAddress.CityId;
      profileAddress.Phone = model.Phone;
      profileAddress.PostalCode = model.PostalCode;
      profileAddress.Description = model.Description;
      profileAddress.AddressOwnerName = model.AddressOwnerName;
      profileAddress.RowVersion = model.RowVersion;
    }
    protected internal void mapPersonProfile(PersonProfile personProfile, EditUserModel model)
    {
      if (model.NationalCode != null)
        personProfile.NationalCode = model.NationalCode;
      if (model.Birthday != null)
        personProfile.Birthdate = model.Birthday;
      if (model.EconomicCode != null)
        personProfile.EconomicCode = model.EconomicCode;
      if (model.FatherName != null)
        personProfile.FatherName = model.FatherName;
      if (model.FirstName != null)
        personProfile.FirstName = model.FirstName;
      if (model.Gender != null)
        personProfile.Gender = model.Gender;
      if (model.LastName != null)
        personProfile.LastName = model.LastName;
      if (model.PictureId != null)
        personProfile.PictureId = model.PictureId;
    }
  }
}