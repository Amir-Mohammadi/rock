using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Files;
using rock.Core.Domains.Orders;
using rock.Core.Errors;
using rock.Core.Services.Files;
using rock.Core.Services.Orders;
using rock.Framework.StateManager;
using rock.Models.CustomerApi;

namespace rock.Core.Services.Common
{
  public class CommonService : ICommonService
  {
    #region Fields
    private readonly IFileService fileService;
    private readonly IRepository<Brand> brandRepository;
    private readonly IRepository<Color> colorRepository;
    private readonly IRepository<Currency> currencyRepository;
    private readonly IRepository<City> cityRepository;
    private readonly IRepository<Province> provinceRepository;
    private readonly IRepository<Static> staticRepository;
    private readonly IRepository<Coupon> couponRepository;
    private readonly IStateManagerService stateManagerService;
    private readonly IWorkContext workContext;
    private readonly IOrderService orderService;
    private readonly IOrderPaymentService orderPaymentService;
    private readonly IRepository<Transportation> transportationRepository;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public CommonService(IFileService fileService,
                         IRepository<Brand> brandRepository,
                         IRepository<Color> colorRepository,
                         IRepository<Currency> currencyRepository,
                         IRepository<City> cityRepository,
                         IRepository<Province> provinceRepository,
                         IRepository<Static> staticRepository,
                         IRepository<Coupon> couponRepository,
                         IStateManagerService stateManagerService,
                         IWorkContext workContext,
                         IOrderService orderService,
                         IOrderPaymentService orderPaymentService,
                         IErrorFactory errorFactory,
                         IRepository<Transportation> transportationRepository)
    {
      this.fileService = fileService;
      this.brandRepository = brandRepository;
      this.colorRepository = colorRepository;
      this.currencyRepository = currencyRepository;
      this.cityRepository = cityRepository;
      this.provinceRepository = provinceRepository;
      this.staticRepository = staticRepository;
      this.couponRepository = couponRepository;
      this.stateManagerService = stateManagerService;
      this.workContext = workContext;
      this.orderService = orderService;
      this.orderPaymentService = orderPaymentService;
      this.transportationRepository = transportationRepository;
      this.errorFactory = errorFactory;
    }
    #endregion
    #region Brand
    public async Task DeleteBrand(Brand brand, CancellationToken cancellationToken)
    {
      await brandRepository.DeleteAsync(brand, cancellationToken);
      var file = await fileService.GetFileById(id: brand.ImageId,
                                               cancellationToken: cancellationToken);
      await fileService.DeleteFile(file: file,
                                   cancellationToken: cancellationToken);
    }
    public async Task<Brand> GetBrandById(int id, CancellationToken cancellationToken, IInclude<Brand> include = null)
    {
      return await brandRepository.GetAsync(predicate: x => x.Id == id,
                                            cancellationToken: cancellationToken,
                                            include: include);
    }
    public IQueryable<Brand> GetBrands(IInclude<Brand> include = null)
    {
      return brandRepository.GetQuery(include: include);
    }
    public async Task UpdateBrand(Brand brand, CancellationToken cancellationToken)
    {
      var imageChanged = brandRepository.IsModified(entity: brand,
                                                    property: x => x.ImageId);
      var imageIdOrginalValue = brandRepository.OrginalValue(entity: brand,
                                                             property: x => x.ImageId);
      if (imageChanged)
      {
        var image = await insertBrandImage(imageId: brand.ImageId,
                                         cancellationToken: cancellationToken);
        brand.Image = image;
      }
      await brandRepository.UpdateAsync(entity: brand,
                                        cancellationToken: cancellationToken);
      if (imageChanged)
      {
        var oldFile = await fileService.GetFileById(id: imageIdOrginalValue,
                                                    cancellationToken: cancellationToken);
        await fileService.DeleteFile(file: oldFile,
                                     cancellationToken: cancellationToken);
      }
    }
    public async Task<Brand> InsertBrand(Brand brand, CancellationToken cancellationToken)
    {
      var image = await insertBrandImage(imageId: brand.ImageId,
                                         cancellationToken: cancellationToken);
      brand.Image = image;
      await brandRepository.AddAsync(brand, cancellationToken);
      return brand;
    }
    private async Task<File> insertBrandImage(Guid imageId, CancellationToken cancellationToken)
    {
      var image = new File();
      image.Id = imageId;
      image.Access = FileAccessType.Public;
      image.OwnerGroup = Domains.Users.UserRole.None;
      image = await fileService.InsertFile(file: image,
                                           cancellationToken: cancellationToken);
      return image;
    }
    #endregion
    #region Color
    public async Task<Color> GetColorById(int id, CancellationToken cancellationToken)
    {
      return await colorRepository.GetAsync(x => x.Id == id, cancellationToken);
    }
    public IQueryable<Color> GetColors()
    {
      return colorRepository.GetQuery();
    }
    public async Task UpdateColor(Color color, CancellationToken cancellationToken)
    {
      await colorRepository.UpdateAsync(color, cancellationToken);
    }
    public async Task<Color> InsertColor(Color color, CancellationToken cancellationToken)
    {
      await colorRepository.AddAsync(color, cancellationToken);
      return color;
    }
    public async Task DeleteColor(Color color, CancellationToken cancellationToken)
    {
      await colorRepository.DeleteAsync(color, cancellationToken);
    }
    #endregion
    #region Currency
    public async Task<Currency> InsertCurrency(Currency currency, CancellationToken cancellationToken)
    {
      await currencyRepository.AddAsync(currency, cancellationToken);
      return currency;
    }
    public async Task UpdateCurrency(Currency currency, CancellationToken cancellationToken)
    {
      await currencyRepository.UpdateAsync(currency, cancellationToken);
    }
    public async Task DeleteCurrency(Currency currency, CancellationToken cancellationToken)
    {
      await currencyRepository.DeleteAsync(currency, cancellationToken);
    }
    public async Task<Currency> GetCurrencyById(int id, CancellationToken cancellationToken, IInclude<Currency> include = null)
    {
      return await currencyRepository.GetAsync(x => x.Id == id, cancellationToken, include);
    }
    public IQueryable<Currency> GetCurrencies(IInclude<Currency> include = null)
    {
      return currencyRepository.GetQuery(include);
    }
    #endregion
    #region City
    public async Task<City> InsertCity(City city, Province province, CancellationToken cancellationToken)
    {
      city.Province = province;
      await cityRepository.AddAsync(city, cancellationToken);
      return city;
    }
    public async Task UpdateCity(City city, Province province, CancellationToken cancellationToken)
    {
      await cityRepository.UpdateAsync(city, cancellationToken);
    }
    public async Task DeleteCity(City city, CancellationToken cancellationToken)
    {
      await cityRepository.DeleteAsync(city, cancellationToken);
    }
    public async Task<City> GetCityById(int id, CancellationToken cancellationToken, IInclude<City> include = null)
    {
      return await cityRepository.GetAsync(x => x.Id == id, cancellationToken, include);
    }
    public IQueryable<City> GetCities(string name = null, int? provinceId = null, IInclude<City> include = null)
    {
      var query = cityRepository.GetQuery(include);
      if (!string.IsNullOrEmpty(name))
        query = query.Where(x => x.Name == name);
      if (provinceId != null)
        query = query.Where(x => x.ProvinceId == provinceId);
      return query;
    }
    #endregion
    #region Provice
    public async Task<Province> InsertProvince(Province province, CancellationToken cancellationToken)
    {
      await provinceRepository.AddAsync(province, cancellationToken);
      return province;
    }
    public async Task UpdateProvince(Province province, CancellationToken cancellationToken)
    {
      await provinceRepository.UpdateAsync(province, cancellationToken);
    }
    public async Task DeleteProvince(Province province, CancellationToken cancellationToken)
    {
      await provinceRepository.DeleteAsync(province, cancellationToken);
    }
    public async Task<Province> GetProvinceById(int id, CancellationToken cancellationToken, IInclude<Province> include = null)
    {
      return await provinceRepository.GetAsync(x => x.Id == id, cancellationToken, include);
    }
    public IQueryable<Province> GetProvinces(string name = null, IInclude<Province> include = null)
    {
      var query = provinceRepository.GetQuery(include);
      if (!string.IsNullOrEmpty(name))
        query = query.Where(x => x.Name == name);
      return query;
    }
    #endregion
    #region Static
    public async Task<Static> InsertStatic(Static @static, CancellationToken cancellationToken)
    {
      await staticRepository.AddAsync(@static, cancellationToken);
      return @static;
    }
    public async Task UpdateStatic(Static @static, CancellationToken cancellationToken)
    {
      await staticRepository.UpdateAsync(@static, cancellationToken);
    }
    public async Task DeleteStatic(Static @static, CancellationToken cancellationToken)
    {
      await staticRepository.DeleteAsync(@static, cancellationToken);
    }
    public async Task<Static> GetStaticByKey(string key, CancellationToken cancellationToken, IInclude<Static> include = null)
    {
      return await staticRepository.GetAsync(x => x.Key == key, cancellationToken, include);
    }
    public IQueryable<Static> GetStatics(IInclude<Static> include = null)
    {
      return staticRepository.GetQuery(include);
    }
    #endregion
    #region Coupon
    public async Task<Coupon> InsertCoupon(Coupon coupon, CancellationToken cancellationToken)
    {
      await CheckDuplicateCouponCode(coupon.CouponCode, cancellationToken);
      await couponRepository.AddAsync(coupon, cancellationToken);
      return coupon;
    }
    public async Task UpdateCoupon(Coupon coupon, CancellationToken cancellationToken)
    {
      await CheckDuplicateCouponCode(coupon.CouponCode, cancellationToken);
      await couponRepository.UpdateAsync(coupon, cancellationToken);
    }
    public async Task DeleteCoupon(Coupon coupon, CancellationToken cancellationToken)
    {
      await couponRepository.DeleteAsync(coupon, cancellationToken);
    }
    public async Task ActivateCoupon(Coupon coupon, CancellationToken cancellationToken)
    {
      coupon.Active = true;
      await couponRepository.UpdateAsync(coupon, cancellationToken);
    }
    public async Task DeactivateCoupon(Coupon coupon, CancellationToken cancellationToken)
    {
      coupon.Active = false;
      await couponRepository.UpdateAsync(coupon, cancellationToken);
    }
    public async Task<Coupon> GetCouponById(int id, CancellationToken cancellationToken, IInclude<Coupon> include = null)
    {
      var coupon = await couponRepository.GetAsync(x => x.Id == id, cancellationToken, include);
      return coupon;
    }
    public async Task<Coupon> GetCouponByCode(string couponCode, CancellationToken cancellationToken, IInclude<Coupon> include = null)
    {
      var coupon = await couponRepository.GetAsync(x => x.CouponCode == couponCode, cancellationToken, include);
      return coupon;
    }
    public IQueryable<Coupon> GetCoupons(string name = null, IInclude<Coupon> include = null)
    {
      var query = couponRepository.GetQuery(include);
      if (!string.IsNullOrEmpty(name))
        query = query.Where(x => x.CouponCode == name);
      return query;
    }

    #endregion

    #region  Transportation
    public async Task<Transportation> CreateTransportation(Transportation transportation, CancellationToken cancellationToken)
    {
      var oldTransportations = await getUnArchiveTransportations(transportation: transportation).ToListAsync(cancellationToken: cancellationToken);

      if (oldTransportations.Any())
      {
        foreach (var item in oldTransportations)
        {
          await deleteTransportation(transportation: item, cancellationToken: cancellationToken);
        }
      }

      transportation.CreatedAt = DateTime.UtcNow;
      await transportationRepository.AddAsync(entity: transportation, cancellationToken: cancellationToken);

      return transportation;
    }

    public async Task<Transportation> UpdateTransportation(Transportation transportation, CancellationToken cancellationToken)
    {
      await transportationRepository.UpdateAsync(entity: transportation, cancellationToken: cancellationToken);
      return transportation;
    }
    private async Task deleteTransportation(Transportation transportation, CancellationToken cancellationToken)
    {
      transportation.DeletedAt = DateTime.UtcNow;
      await transportationRepository.UpdateAsync(entity: transportation, cancellationToken: cancellationToken);
    }

    public IQueryable<Transportation> GetTransportations(int? fromCityId = null, int? toCityId = null, int[] fromCityIds = null, int[] toCityIds = null, IInclude<Transportation> include = null)
    {

      var transportation = transportationRepository.GetQuery(include: include).Where(m => m.DeletedAt == null);
      if (fromCityId != null)
        transportation = transportation.Where(m => m.FromCityId == fromCityId);
      if (toCityId != null)
        transportation = transportation.Where(m => m.ToCityId == toCityId);
      if (toCityIds != null)
        transportation = transportation.Where(m => toCityIds.Contains(m.ToCityId));
      if (fromCityIds != null)
        transportation = transportation.Where(m => fromCityIds.Contains(m.FromCityId));
      return transportation;
    }

    public async Task<Transportation> GetTransportationById(int id, CancellationToken cancellationToken, IInclude<Transportation> include = null)
    {
      return await transportationRepository.GetAsync(predicate: (x => x.Id == id && x.DeletedAt == null), cancellationToken, include);
    }

    private IQueryable<Transportation> getUnArchiveTransportations(Transportation transportation)
    {
      return transportationRepository.GetQuery(include: null).Where(x => x.FromCityId == transportation.FromCityId && x.ToCityId == transportation.ToCityId && x.DeletedAt == null);
    }

    public async Task<Transportation> GetAvailableTransportation(int fromCityId, int toCityId, CancellationToken cancellationToken)
    {
      return await transportationRepository.GetAsync(predicate: (x => x.FromCityId == fromCityId && x.ToCityId == toCityId && x.DeletedAt == null), cancellationToken: cancellationToken, include: null);
    }
    public async Task CheckDuplicateCouponCode(string couponCode, CancellationToken cancellationToken)
    {
      var coupone = await this.GetCouponByCode(couponCode, cancellationToken);
      if (coupone != null)
      {
        throw errorFactory.CouponDuplicateCode();
      }
    }
    #endregion
  }
}