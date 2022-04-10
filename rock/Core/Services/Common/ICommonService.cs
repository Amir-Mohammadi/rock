using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Framework.Autofac;

namespace rock.Core.Services.Common
{
  public interface ICommonService : IScopedDependency
  {
    #region Brand
    Task<Brand> InsertBrand(Brand brand, CancellationToken cancellationToken);
    Task UpdateBrand(Brand brand, CancellationToken cancellationToken);
    Task DeleteBrand(Brand brand, CancellationToken cancellationToken);
    Task<Brand> GetBrandById(int id, CancellationToken cancellationToken, IInclude<Brand> include = null);
    IQueryable<Brand> GetBrands(IInclude<Brand> include = null);
    #endregion

    #region Color
    Task<Color> InsertColor(Color color, CancellationToken cancellationToken);
    Task UpdateColor(Color color, CancellationToken cancellationToken);
    Task DeleteColor(Color color, CancellationToken cancellationToken);
    Task<Color> GetColorById(int id, CancellationToken cancellationToken);
    IQueryable<Color> GetColors();
    #endregion

    #region Currency
    Task<Currency> InsertCurrency(Currency currency, CancellationToken cancellationToken);
    Task UpdateCurrency(Currency currency, CancellationToken cancellationToken);
    Task DeleteCurrency(Currency currency, CancellationToken cancellationToken);
    Task<Currency> GetCurrencyById(int id, CancellationToken cancellationToken, IInclude<Currency> include = null);
    IQueryable<Currency> GetCurrencies(IInclude<Currency> include = null);
    #endregion

    #region City
    Task<City> InsertCity(City city, Province province, CancellationToken cancellationToken);
    Task UpdateCity(City city, Province province, CancellationToken cancellationToken);
    Task DeleteCity(City city, CancellationToken cancellationToken);
    Task<City> GetCityById(int id, CancellationToken cancellationToken, IInclude<City> include = null);
    IQueryable<City> GetCities(string name = null, int? provinceId = null, IInclude<City> include = null);
    #endregion

    #region Province
    Task<Province> InsertProvince(Province province, CancellationToken cancellationToken);
    Task UpdateProvince(Province province, CancellationToken cancellationToken);
    Task DeleteProvince(Province province, CancellationToken cancellationToken);
    Task<Province> GetProvinceById(int id, CancellationToken cancellationToken, IInclude<Province> include = null);
    IQueryable<Province> GetProvinces(string name = null, IInclude<Province> include = null);
    #endregion

    #region Static
    Task<Static> InsertStatic(Static staticKeyValue, CancellationToken cancellationToken);
    Task UpdateStatic(Static staticKeyValue, CancellationToken cancellationToken);
    Task DeleteStatic(Static staticKeyValue, CancellationToken cancellationToken);
    Task<Static> GetStaticByKey(string key, CancellationToken cancellationToken, IInclude<Static> include = null);
    IQueryable<Static> GetStatics(IInclude<Static> include = null);
    #endregion

    #region Coupon
    Task<Coupon> InsertCoupon(Coupon coupon, CancellationToken cancellationToken);
    Task UpdateCoupon(Coupon coupon, CancellationToken cancellationToken);
    Task DeleteCoupon(Coupon coupon, CancellationToken cancellationToken);
    Task ActivateCoupon(Coupon coupon, CancellationToken cancellationToken);
    Task DeactivateCoupon(Coupon coupon, CancellationToken cancellationToken);
    Task<Coupon> GetCouponById(int couponId, CancellationToken cancellationToken, IInclude<Coupon> include = null);
    Task<Coupon> GetCouponByCode(string couponCode, CancellationToken cancellationToken, IInclude<Coupon> include = null);
    IQueryable<Coupon> GetCoupons(string name = null, IInclude<Coupon> include = null);
    #endregion

    #region  Transportation
    Task<Transportation> CreateTransportation(Transportation transportation, CancellationToken cancellationToken);
    Task<Transportation> UpdateTransportation(Transportation transportation, CancellationToken cancellationToken);
    Task<Transportation> GetTransportationById(int id, CancellationToken cancellationToken, IInclude<Transportation> include = null);
    IQueryable<Transportation> GetTransportations(int? fromCityId = null, int? toCityId = null, int[] fromCityIds = null, int[] toCityIds = null, IInclude<Transportation> include = null);
    Task<Transportation> GetAvailableTransportation(int fromCityId, int toCityId, CancellationToken cancellationToken);
    #endregion
  }
}