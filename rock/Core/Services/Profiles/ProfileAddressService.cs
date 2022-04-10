using rock.Core.Domains.Profiles;
using System.Threading.Tasks;
using System.Threading;
using rock.Core.Domains.Commons;
using rock.Core.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using rock.Core.Services.Orders;
using System;

namespace rock.Core.Services.Profiles
{
  public class ProfileAddressService : IProfileAddressService
  {
    private IRepository<ProfileAddress> profileAddressRepository;

    public ProfileAddressService(IRepository<ProfileAddress> profileAddressRepository)
    {
      this.profileAddressRepository = profileAddressRepository;
    }
    public async Task DeleteProfileAddress(ProfileAddress profileAddress, CancellationToken cancellationToken)
    {
      if (profileAddress.Carts == null)
        await profileAddressRepository.LoadCollectionAsync(entity: profileAddress,
                                                     collectionProperty: x => x.Carts,
                                                     cancellationToken: cancellationToken);
      if (!profileAddress.Carts.Any())
      {
        await profileAddressRepository.DeleteAsync(entity: profileAddress, cancellationToken: cancellationToken);
      }
      else
      {
        profileAddress.DeletedAt = DateTime.UtcNow;
        await profileAddressRepository.UpdateAsync(entity: profileAddress, cancellationToken: cancellationToken);

      }

    }
    public Task<ProfileAddress> GetProfileAddressById(int profileId, int profileAddressId, CancellationToken cancellationToken, Include<ProfileAddress> include = null)
    {
      return profileAddressRepository.GetAsync(
                                      predicate: x => x.Id == profileAddressId && x.ProfileId == profileId,
                                      cancellationToken: cancellationToken,
                                      include: include);
    }

    public async Task<ProfileAddress> GetDefaultOrFirstProfileAddressByUserId(int profileId, CancellationToken cancellationToken)
    {

      var addresses = await GetProfileAddresses(profileId: profileId, include: null).ToListAsync(cancellationToken: cancellationToken);
      var address = addresses.FirstOrDefault(x => x.IsDefault == true);
      if (address == null)
      {
        return addresses.FirstOrDefault();
      }
      return address;
    }
    public IQueryable<ProfileAddress> GetProfileAddresses(int? profileId = null, bool? isDefault = null, IInclude<ProfileAddress> include = null)
    {
      var query = profileAddressRepository.GetQuery(include);
      if (profileId != null)
        query = query.Where(x => x.ProfileId == profileId);
      if (isDefault != null)
        query = query.Where(x => x.IsDefault == isDefault);
      return query;
    }


    public async Task<ProfileAddress> InsertProfileAddress(ProfileAddress profileAddress, Profile profile, City city, CancellationToken cancellationToken)
    {
      profileAddress.Profile = profile;
      profileAddress.City = city;
      await profileAddressRepository.AddAsync(
                                      entity: profileAddress,
                                      cancellationToken: cancellationToken);
      return profileAddress;
    }
    public async Task UpdateProfileAddress(ProfileAddress profileAddress, City city, CancellationToken cancellationToken)
    {
      profileAddress.City = city;

      var isModified = profileAddressRepository.IsModified(entity: profileAddress,
                                                           "CityId",
                                                           "Description",
                                                           "PostalCode",
                                                           "AddressOwnerName",
                                                           "Phone");
      if (isModified)
      {
        if (profileAddress.Carts == null)
          await profileAddressRepository.LoadCollectionAsync(entity: profileAddress,
                                                       collectionProperty: x => x.Carts,
                                                       cancellationToken: cancellationToken);
        if (!profileAddress.Carts.Any())
        {
          await profileAddressRepository.UpdateAsync(entity: profileAddress, cancellationToken: cancellationToken);
        }
        else
        {

          #region  delete profile address
          profileAddress.DeletedAt = DateTime.UtcNow;
          await profileAddressRepository.UpdateAsync(entity: profileAddress, cancellationToken: cancellationToken);
          #endregion

          #region  create profile address
          var newProfileAddress = new ProfileAddress();
          newProfileAddress.ProfileId = profileAddress.ProfileId;
          newProfileAddress.Phone = profileAddress.Phone;
          newProfileAddress.PostalCode = profileAddress.PostalCode;
          newProfileAddress.Description = profileAddress.Description;
          newProfileAddress.AddressOwnerName = profileAddress.AddressOwnerName;
          newProfileAddress.City = profileAddress.City;
          newProfileAddress.IsDefault = profileAddress.IsDefault;
          await profileAddressRepository.AddAsync(entity: newProfileAddress, cancellationToken: cancellationToken);
          #endregion
        }
      }
    }

    public async Task SetDefaultProfileAddress(ProfileAddress profileAddress, CancellationToken cancellationToken)
    {
      var defaultAddresses = await GetProfileAddresses(profileId: profileAddress.ProfileId,
                                                 isDefault: true).ToListAsync(cancellationToken: cancellationToken);

      foreach (var item in defaultAddresses)
      {
        item.IsDefault = false;
        await profileAddressRepository.UpdateAsync(entity: item, cancellationToken: cancellationToken);
      }

      profileAddress.IsDefault = true;
      await profileAddressRepository.UpdateAsync(entity: profileAddress, cancellationToken: cancellationToken);
    }
  }
}