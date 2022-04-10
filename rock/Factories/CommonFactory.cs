using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Core.Services.Common;
using rock.Filters;
using rock.Models.CommonApi;
namespace rock.Factories
{
  public class CommonFactory : BaseFactory, ICommonFactory
  {
    #region Fields
    private readonly ICommonService commonService;
    #endregion
    #region Constractor
    public CommonFactory(ICommonService commonService)
    {
      this.commonService = commonService;
    }
    #endregion
    #region Brand
    public async Task<IList<BrandModel>> PrepareBrandListModel(CancellationToken cancellationToken)
    {
      var brands = commonService.GetBrands();
      return await this.CreateModelListAsync(source: brands,
                                             convertFunction: this.CreateBrandModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<BrandModel> PrepareBrandModel(int brandId, CancellationToken cancellationToken)
    {
      var brand = await this.commonService.GetBrandById(id: brandId, cancellationToken);
      return this.CreateBrandModel(brand: brand);
    }
    private BrandModel CreateBrandModel(Brand brand)
    {
      if (brand == null)
        return null;
      return new BrandModel()
      {
        Id = brand.Id,
        Name = brand.Name,
        UrlTitle = brand.UrlTitle,
        BrowserTitle = brand.BrowserTitle,
        MetaDescription = brand.MetaDescription,
        Description = brand.Description,
        ImageAlt = brand.ImageAlt,
        ImageTitle = brand.ImageTitle,
        ImageId = brand.ImageId,
        RowVersion = brand.RowVersion,
      };
    }
    #endregion
    #region Currency
    public async Task<IList<CurrencyModel>> PrepareCurrencyListModel(CancellationToken cancellationToken)
    {
      var currencies = this.commonService.GetCurrencies();
      return await this.CreateModelListAsync(source: currencies,
                                             convertFunction: this.CreateCurrencyModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<CurrencyModel> PrepareCurrencyModel(int currencyId, CancellationToken cancellationToken)
    {
      var currency = await this.commonService.GetCurrencyById(id: currencyId,
                                                              cancellationToken: cancellationToken);
      return this.CreateCurrencyModel(currency: currency);
    }
    #endregion
    #region Color
    public async Task<IList<ColorModel>> PrepareColorListModel(CancellationToken cancellationToken)
    {
      var colors = this.commonService.GetColors();
      return await this.CreateModelListAsync(source: colors,
                                             convertFunction: this.CreateColorModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<ColorModel> PrepareColorModel(int colorId, CancellationToken cancellationToken)
    {
      var color = await this.commonService.GetColorById(id: colorId,
                                                        cancellationToken: cancellationToken);
      return this.CreateColorModel(color: color);
    }
    private ColorModel CreateColorModel(Color color)
    {
      if (color == null)
        return null;
      return new ColorModel()
      {
        Id = color.Id,
        Name = color.Name,
        Code = color.Code,
        RowVersion = color.RowVersion
      };
    }
    #endregion
    #region City
    public async Task<IPagedList<CityModel>> PrepareCityListModel(CitySearchParameters parameters,
                                                                  CancellationToken cancellationToken)
    {
      var cities = this.commonService.GetCities(name: parameters.Name,
                                                include: new Include<City>(query =>
                                                {
                                                  query = query.Include(x => x.Province);
                                                  return query;
                                                }));
      return await this.CreateModelPagedListAsync(source: cities,
                                                  convertFunction: createCityModel,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  cancellationToken: cancellationToken);
    }
    public async Task<IList<CityModel>> PrepareCityListModel(int provinceId, CancellationToken cancellationToken)
    {
      var cities = this.commonService.GetCities(provinceId: provinceId,
                                                include: new Include<City>(query =>
                                                {
                                                  query = query.Include(x => x.Province);
                                                  return query;
                                                }));
      return await this.CreateModelListAsync(source: cities,
                                             convertFunction: createCityModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<CityModel> PrepareCityModel(int cityId, CancellationToken cancellationToken)
    {
      var city = await this.commonService.GetCityById(id: cityId,
                                                      include: new Include<City>(query =>
                                                      {
                                                        query = query.Include(x => x.Province);
                                                        return query;
                                                      }),
                                                      cancellationToken: cancellationToken);
      return this.createCityModel(city: city);
    }
    #endregion
    #region Province
    public async Task<IList<ProvinceModel>> PrepareProvinceListModel(ProvincesSearchParameters parameters,
                                                                     CancellationToken cancellationToken)
    {
      var provinces = this.commonService.GetProvinces(parameters.Name);
      return await this.CreateModelListAsync(source: provinces,
                                             convertFunction: this.CreateProvinceModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<ProvinceModel> PrepareProvinceModel(int provinceId,
                                                          CancellationToken cancellationToken)
    {
      var province = await this.commonService.GetProvinceById(id: provinceId,
                                                              cancellationToken: cancellationToken);
      return this.CreateProvinceModel(province: province);
    }
    #endregion
    #region Static
    public async Task<StaticsModel> PrepareStaticModel(string staticKey, CancellationToken cancellationToken)
    {
      var @static = await this.commonService.GetStaticByKey(key: staticKey,
                                                            cancellationToken: cancellationToken);
      return CreateStaticModel(@static: @static);
    }
    public async Task<IList<StaticsModel>> PrepareStaticListModel(CancellationToken cancellationToken)
    {
      var statics = this.commonService.GetStatics();
      return await this.CreateModelListAsync(source: statics,
                                             convertFunction: this.CreateStaticModel,
                                             cancellationToken: cancellationToken);
    }
    private StaticsModel CreateStaticModel(Static @static)
    {
      if (@static == null)
        return null;
      return new StaticsModel()
      {
        Id = @static.Id,
        Key = @static.Key,
        Value = @static.Value,
        StaticType = @static.StaticType,
        RowVersion = @static.RowVersion,
      };
    }
    #endregion
    #region Coupon
    public async Task<IPagedList<CouponModel>> PrepareCouponListModel(CouponSearchParameters parameters,
                                                                      CancellationToken cancellationToken)
    {
      var coupons = this.commonService.GetCoupons(name: parameters.Name);
      var result = await this.CreateModelPagedListAsync(source: coupons,
                                                        convertFunction: this.CreateCouponModel,
                                                        order: parameters.Order,
                                                        pageIndex: parameters.PageIndex,
                                                        pageSize: parameters.PageSize,
                                                        sortBy: parameters.SortBy,
                                                        cancellationToken: cancellationToken);
      return result;
    }
    public async Task<CouponModel> PrepareCouponModel(int couponId, CancellationToken cancellationToken)
    {
      var coupone = await this.commonService.GetCouponById(couponId: couponId, cancellationToken: cancellationToken);
      return this.CreateCouponModel(coupone);
    }
    public async Task<CouponModel> PrepareCouponModel(string couponCode, CancellationToken cancellationToken)
    {
      var coupone =
          await this.commonService.GetCouponByCode(couponCode: couponCode, cancellationToken: cancellationToken);
      return this.CreateCouponModel(coupone);
    }
    private CouponModel CreateCouponModel(Coupon coupon)
    {
      if (coupon == null)
        return null;
      return new CouponModel()
      {
        Id = coupon.Id,
        Active = coupon.Active,
        CouponCode = coupon.CouponCode,
        ExpiryDate = coupon.ExpiryDate,
        MaxQuantities = coupon.MaxQuantities,
        Value = coupon.Value,
        RowVersion = coupon.RowVersion,
        MaxQuantitiesPerUser = coupon.MaxQuantitiesPerUser
      };
    }
    #endregion
    #region Transportation
    public async Task<IList<TransportationModel>> PrepareTransportationListModel(
      TransportationSearchParameters parameters,
      CancellationToken cancellationToken)
    {
      var transportations = commonService.GetTransportations(fromCityId: parameters.FromCityId,
                                                             toCityId: parameters.ToCityId,
                                                             include: new Include<Transportation>(query =>
                                                             {
                                                               query = query.Include(x => x.FromCity)
                                                                            .ThenInclude(x => x.Province);
                                                               query = query.Include(x => x.ToCity)
                                                                            .ThenInclude(x => x.Province);
                                                               return query;
                                                             }));
      return await this.CreateModelPagedListAsync(source: transportations,
                                                  convertFunction: this.createTransportationModel,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  cancellationToken: cancellationToken);
    }
    public async Task<TransportationModel> PrepareTransportationModel(int transportationId,
                                                                      CancellationToken cancellationToken)
    {
      var transportation = await this.commonService.GetTransportationById(id: transportationId,
                                                                          include: new Include<Transportation>(query =>
                                                                          {
                                                                            query = query.Include(x => x.FromCity)
                                                                                .ThenInclude(x => x.Province);
                                                                            query = query.Include(x => x.ToCity)
                                                                                .ThenInclude(x => x.Province);
                                                                            return query;
                                                                          }),
                                                                          cancellationToken: cancellationToken);
      return this.createTransportationModel(transportation: transportation);
    }
    private TransportationModel createTransportationModel(Transportation transportation)
    {
      if (transportation == null)
        return null;
      return new TransportationModel
      {
        Id = transportation.Id,
        Cost = transportation.Cost,
        Description = transportation.Description,
        Distance = transportation.Distance,
        FromCity = this.createCityModel(transportation.FromCity),
        ToCity = this.createCityModel(transportation.ToCity),
        RowVersion = transportation.RowVersion
      };
    }
    #endregion
  }
}