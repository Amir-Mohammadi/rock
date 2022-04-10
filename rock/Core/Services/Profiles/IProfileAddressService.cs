using rock.Framework.Autofac;
using rock.Core.Domains.Profiles;
using System.Threading.Tasks;
using System.Threading;
using rock.Core.Domains.Commons;
using System.Linq;
using rock.Core.Data;
namespace rock.Core.Services.Profiles
{
  public interface IProfileAddressService : IScopedDependency
  {
    #region ProfileAddress
    Task<ProfileAddress> InsertProfileAddress(ProfileAddress profileAddress, Profile profile, City city, CancellationToken cancellationToken);
    Task UpdateProfileAddress(ProfileAddress profileAddress, City city, CancellationToken cancellationToken);
    Task SetDefaultProfileAddress(ProfileAddress profileAddress, CancellationToken cancellationToken);
    Task DeleteProfileAddress(ProfileAddress profileAddress, CancellationToken cancellationToken);
    Task<ProfileAddress> GetProfileAddressById(int profileId, int profileAddressId, CancellationToken cancellationToken, Include<ProfileAddress> include = null);
    IQueryable<ProfileAddress> GetProfileAddresses(int? profileId = null, bool? isDefault = null, IInclude<ProfileAddress> include = null);
    Task<ProfileAddress> GetDefaultOrFirstProfileAddressByUserId(int profileId, CancellationToken cancellationToken);
    #endregion
  }
}