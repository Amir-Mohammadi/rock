using System;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Commons;
using System.Threading.Tasks;
using System.Threading;
using rock.Core.Data;
using Microsoft.EntityFrameworkCore;
using rock.Core.Domains.Users;
using rock.Core.Services.Common;
using rock.Core.Errors;
using rock.Core.Domains.Orders;
using System.Linq;
using rock.Core.Services.Orders;
using rock.Models.UserApi;
namespace rock.Core.Services.Profiles
{
  public class ProfileService : IProfileService
  {
    private readonly IRepository<Profile> profileRepository;
    private readonly IRepository<CompanyProfile> companyProfileRepository;
    private readonly IRepository<PersonProfile> personProfileRepository;
    private readonly IRepository<User> userRepository;
    private readonly IOrderItemService orderItemService;
    private readonly IWorkContext workContext;
    private readonly IErrorFactory errors;
    public ProfileService(IWorkContext workContext,
                          IRepository<Profile> profileRepository,
                          IRepository<CompanyProfile> companyProfileRepository,
                          IRepository<PersonProfile> personProfileRepository,
                          IOrderItemService orderItemService,
                          IRepository<User> userRepository, IErrorFactory errors)
    {
      this.workContext = workContext;
      this.profileRepository = profileRepository;
      this.companyProfileRepository = companyProfileRepository;
      this.personProfileRepository = personProfileRepository;
      this.userRepository = userRepository;
      this.orderItemService = orderItemService;
      this.errors = errors;
    }
    public Task DeleteProfile(Profile profile, CancellationToken cancellationToken)
    {
      return profileRepository.DeleteAsync(entity: profile, cancellationToken: cancellationToken);
    }
    public Task<CompanyProfile> GetCompanyProfileById(int id, CancellationToken cancellationToken)
    {
      return companyProfileRepository.GetAsync(
                                      predicate: x => x.Id == id,
                                      cancellationToken: cancellationToken,
                                      include: new Include<CompanyProfile>(q =>
                                      {
                                        q = q.Include(x => x.Profile);
                                        return q;
                                      }));
    }
    public async Task<PersonProfile> GetCurrentUserPersonProfile(CancellationToken cancellationToken, IInclude<PersonProfile> include = null)
    {
      var user = await userRepository.GetAsync(predicate: x => x.Id == workContext.GetCurrentUserId(),
                                               cancellationToken: cancellationToken,
                                               include: new Include<User>(query =>
     {
       query = query.Include(x => x.Profile)
       .ThenInclude(x => x.PersonProfile);
       return query;
     }));
      return user.Profile.PersonProfile;
    }
    public async Task<Profile> GetCurrentUserProfile(CancellationToken cancellationToken, IInclude<Profile> include = null)
    {
      var currentUserId = workContext.GetCurrentUserId();
      return await GetProfileByUserId(userId: currentUserId,
                                      cancellationToken: cancellationToken,
                                      include: include);
    }
    public Task<PersonProfile> GetPersonProfileById(int id, CancellationToken cancellationToken)
    {
      return personProfileRepository.GetAsync(
                              predicate: x => x.Id == id,
                              cancellationToken: cancellationToken,
                              include: new Include<PersonProfile>(q =>
                              {
                                q = q.Include(x => x.Profile);
                                return q;
                              }));
    }
    public Task<Profile> GetProfileById(int id, CancellationToken cancellationToken, IInclude<Profile> include = null)
    {
      return profileRepository.GetAsync(
                        predicate: x => x.Id == id,
                        cancellationToken: cancellationToken,
                        include: new Include<Profile>(q =>
                         {
                           q = q.Include(x => x.PersonProfile);
                           q = q.Include(x => x.CompanyProfile);
                           q = q.Include(x => x.City);
                           return q;
                         }));
    }
    public async Task<Profile> GetProfileByUserId(int userId, CancellationToken cancellationToken, IInclude<Profile> include = null)
    {
      var user = await userRepository.GetAsync(
                                      predicate: x => x.Id == userId,
                                      cancellationToken: cancellationToken);
      return await GetProfileById(
                   id: user.ProfileId,
                   cancellationToken: cancellationToken);
    }
    public Task<CompanyProfile> GetUserCompanyProfile(CancellationToken cancellationToken, IInclude<CompanyProfile> include = null)
    {
      // TODO: Check if this function is required
      // FIXME: unable to get company profile by user id
      throw new NotImplementedException();
    }
    public Task<CompanyProfile> InsertCompanyProfile(CompanyProfile companyProfile, Profile profile, City city, CancellationToken cancellationToken)
    {
      // TODO: Check if this function is required
      // companyProfile.Profile = profile;
      // companyProfileRepository.AddAsync(entity:companyProfile)
      // return companyProfile;
      throw new NotImplementedException();
    }
    public async Task<PersonProfile> InsertPersonProfile(PersonProfile personProfile, Profile profile, City city, CancellationToken cancellationToken)
    {
      await InsertProfile(profile: profile, city: city, cancellationToken: cancellationToken);
      personProfile.Profile = profile;
      await personProfileRepository.AddAsync(entity: personProfile, cancellationToken: cancellationToken);
      return personProfile;
    }
    public async Task<Profile> InsertProfile(Profile profile, City city, CancellationToken cancellationToken)
    {
      profile.City = city;
      await assertProfileDetails(profile);
      await profileRepository.AddAsync(entity: profile, cancellationToken: cancellationToken);
      return profile;
    }
    private async Task assertProfileDetails(Profile profile)
    {
      var profiles = profileRepository.GetQuery(include: new Include<Profile>(query =>
      {
        query = query.Include(x => x.PersonProfile);
        return query;
      }));
      var emailIsExist = await profiles.AnyAsync(x => (x.Email == profile.Email && profile.Email != null) && ((profile.Id == 0) || (profile.Id != 0 && x.Id != profile.Id)));
      var phoneIsExist = await profiles.AnyAsync(x => (x.Phone == profile.Phone && profile.Phone != null) && ((profile.Id == 0) || (profile.Id != 0 && x.Id != profile.Id)));
      #region national code validate 
      if (profile.PersonProfile?.NationalCode != null)
      {
        if (profile.PersonProfile.NationalCode.Length != 10 ||
        profile.PersonProfile.NationalCode == "0000000000" ||
        profile.PersonProfile.NationalCode == "1111111111" ||
        profile.PersonProfile.NationalCode == "2222222222" ||
        profile.PersonProfile.NationalCode == "3333333333" ||
        profile.PersonProfile.NationalCode == "4444444444" ||
        profile.PersonProfile.NationalCode == "5555555555" ||
        profile.PersonProfile.NationalCode == "6666666666" ||
        profile.PersonProfile.NationalCode == "7777777777" ||
        profile.PersonProfile.NationalCode == "8888888888" ||
        profile.PersonProfile.NationalCode == "9999999999")
        {
          throw errors.TheEnteredNationalCodeIsInvalid();
        }
        else
        {
          char[] charArray = profile.PersonProfile.NationalCode.ToCharArray();
          int[] numArray = new int[charArray.Length];
          for (int i = 0; i < charArray.Length; i++)
          {
            numArray[i] = (int)char.GetNumericValue(charArray[i]);
          }
          int tempA = numArray[9];
          int tempB = ((((((((numArray[0] * 10) + (numArray[1] * 9)) +
           (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) +
            (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
          int tempC = tempB - ((tempB / 11) * 11);
          bool codeIsValid = false;
          if ((((tempC == 0) && (tempA == tempC)) || ((tempC == 1) && (tempA == 1))) || ((tempC > 1) && (tempA == Math.Abs((int)(tempC - 11)))))
          {
            codeIsValid = true;
          }
          if (codeIsValid == false)
          {
            throw errors.TheEnteredNationalCodeIsInvalid();
          }
        }
      }
      #endregion
      if (emailIsExist)
      {
        throw errors.ProfileWithCurrentEmailExists();
      }
      if (phoneIsExist)
      {
        throw errors.ProfileWithCurrentPhoneExists();
      }
    }
    public Task UpdateCompanyProfile(CompanyProfile companyProfile, Profile profile, City city, CancellationToken cancellationToken)
    {
      //TODO: Check if this function required
      throw new NotImplementedException();
    }
    public async Task UpdatePersonProfile(PersonProfile personProfile, Profile profile, City city, CancellationToken cancellationToken)
    {
      await UpdateProfile(profile, city, cancellationToken);
      personProfile.Profile = profile;
      await personProfileRepository.UpdateAsync(entity: personProfile, cancellationToken: cancellationToken);
    }
    public async Task UpdateProfile(Profile profile, City city, CancellationToken cancellationToken)
    {
      profile.City = city;
      await assertProfileDetails(profile);
      await profileRepository.UpdateAsync(entity: profile, cancellationToken: cancellationToken);
    }
    public IQueryable<OrderItem> GetUserOrders(UserOrderSearchParameters parameter, IInclude<OrderItem> include = null)
    {
      var currentUserId = workContext.GetCurrentUserId();
      var query = orderItemService.GetOrderItems(include: include, userId: currentUserId);
      if (parameter.Status != null)
        query = query.Where(x => x.LatestStatus.Type == parameter.Status);
      return query;
    }
  }
}