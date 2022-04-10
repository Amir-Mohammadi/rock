
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Users;
using rock.Core.Services.Profiles;
using rock.Core.Services.Users;
using rock.Models.UserApi;
using rock.Core.Data;
using Microsoft.EntityFrameworkCore;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Models.CommonApi;
using System.Linq;
namespace rock.Factories
{
  public class UserFactory : BaseFactory, IUserFactory
  {
    #region Fields
    private readonly Include<User> fullUserInclude = new Include<User>(x =>
    {
      x = x.Include(x => x.Profile);
      x = x.Include(x => x.Profile.PersonProfile);
      x = x.Include(x => x.Profile.City);
      return x;
    });
    private readonly IUserService userService;
    private readonly IAuthService authService;
    private readonly IProfileService profileService;
    private readonly IProfileAddressService profileAddressService;
    #endregion
    #region Constractor
    public UserFactory(IUserService userService,
                       IAuthService authService,
                       IProfileService profileService,
                       IProfileAddressService profileAddressService)
    {
      this.userService = userService;
      this.authService = authService;
      this.profileService = profileService;
      this.profileAddressService = profileAddressService;
    }
    #endregion
    #region CurrentUser
    public async Task<UserModel> PrepareCurrentUserModel(CancellationToken cancellationToken)
    {
      var user = await this.userService.GetCurrentUser(cancellationToken: cancellationToken, include: fullUserInclude);
      return this.createUserModel(user: user);
    }
    #endregion
    #region UserOrders
    public async Task<IList<UserOrderModel>> PrepareUserOrdersListModel(UserOrderSearchParameters parameter, CancellationToken cancellationToken)
    {
      var orders = this.profileService.GetUserOrders(parameter: parameter, include: new Include<OrderItem>(query =>
                                                                      {
                                                                        query = query.Include(x => x.Order);
                                                                        query = query.Include(x => x.Order).ThenInclude(x => x.LatestOrderStatus);
                                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart).ThenInclude(x => x.ProfileAddress);
                                                                        query = query.Include(x => x.Order).ThenInclude(x => x.Cart).ThenInclude(x => x.User);
                                                                        query = query.Include(x => x.CartItem).ThenInclude(x => x.Product).ThenInclude(x => x.PreviewProductImage);
                                                                        query = query.Include(x => x.CartItem).ThenInclude(x => x.Product).ThenInclude(x => x.ProductCategory);
                                                                        query = query.Include(x => x.CartItem).ThenInclude(x => x.Product).ThenInclude(x => x.Brand);
                                                                        query = query.Include(x => x.CartItem).ThenInclude(x => x.Color);
                                                                        query = query.Include(x => x.CartItem).ThenInclude(x => x.ProductPrice);
                                                                        query = query.Include(x => x.LatestStatus);
                                                                        query = query.Include(x => x.Transport).ThenInclude(x => x.ToCity);
                                                                        return query;
                                                                      }));
      return await this.CreateModelListAsync(orders, this.createUserOrderModel, cancellationToken);
    }
    #endregion
    #region User
    public async Task<IList<UserAddressModel>> PrepareUserAddressListModel(int profileId, CancellationToken cancellationToken)
    {
      var addresses = this.profileAddressService.GetProfileAddresses(profileId: profileId,
                                                                     include: new Include<ProfileAddress>(query =>
                                                                     {
                                                                       query = query.Include(x => x.City).ThenInclude(x => x.Province);
                                                                       return query;
                                                                     }))
                                                 .Where(x => x.DeletedAt == null)
                                                 .OrderBy(x => x.IsDefault);
      return await this.CreateModelListAsync(addresses, this.createUserAddressModel, cancellationToken);
    }
    public async Task<UserAddressModel> PrepareUserAddressModel(int userId, int profileAddressId, CancellationToken cancellationToken)
    {
      var user = await this.userService.GetUserById(id: userId, cancellationToken);
      var userAddress = await this.profileAddressService.GetProfileAddressById(profileId: user.ProfileId, profileAddressId: profileAddressId, cancellationToken);
      return this.createUserAddressModel(profileAddress: userAddress);
    }
    public async Task<UserModel> PrepareUserModel(int userId, CancellationToken cancellationToken)
    {
      var user = await this.userService.GetUserById(id: userId, cancellationToken, include: fullUserInclude);
      return this.createUserModel(user: user);
    }
    public async Task<IPagedList<UserModel>> PrepareUserPagedListModel(UserSearchParameters parameters, CancellationToken cancellationToken)
    {
      var users = userService.GetUsers(cityId: parameters.CityId,
      roles: parameters.Roles,
      phone: parameters.Phone,
      email: parameters.Email,
      include: new Include<User>(query =>
      {
        query = query.Include(x => x.Profile).ThenInclude(x => x.PersonProfile)
        .Include(x => x.Profile.City).ThenInclude(x => x.Province);
        return query;
      }));
      return await CreateModelPagedListAsync(source: users,
                                             convertFunction: createUserModel,
                                             sortBy: parameters.SortBy,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             cancellationToken: cancellationToken);
    }
    private UserAddressModel createUserAddressModel(ProfileAddress profileAddress)
    {
      if (profileAddress == null)
        return null;
      return new UserAddressModel()
      {
        AddressOwnerName = profileAddress.AddressOwnerName,
        City = this.createCityModel(profileAddress.City),
        Description = profileAddress.Description,
        Id = profileAddress.Id,
        Phone = profileAddress.Phone,
        PostalCode = profileAddress.PostalCode,
        IsDefault = profileAddress.IsDefault,
        RowVersion = profileAddress.RowVersion
      };
    }
    private UserOrderModel createUserOrderModel(OrderItem orderItem)
    {
      if (orderItem == null)
        return null;
      return new UserOrderModel()
      {
        Id = orderItem.Id,
        PreviewProductImage = createProductImageModel(orderItem?.CartItem?.Product?.PreviewProductImage),
        ProductName = orderItem?.CartItem?.Product?.Name,
        ProductCategoryName = orderItem?.CartItem?.Product?.ProductCategory?.Name,
        BrandName = orderItem?.CartItem?.Product?.Brand?.Name,
        OrderedColor = this.createColorModel(orderItem?.CartItem?.Color),
        ProductPrice = orderItem?.CartItem?.ProductPrice?.Price.ToString(),
        Status = orderItem.LatestStatus.Type,
        City = orderItem?.Transport?.ToCity?.Name,
        RowVersion = orderItem.RowVersion
      };
    }
    #region Color
    private ColorModel createColorModel(Color color)
    {
      if (color == null)
        return null;
      return new ColorModel()
      {
        Id = color.Id,
        Name = color.Name,
        Code = color.Code
      };
    }
    #endregion
    #endregion
  }
}