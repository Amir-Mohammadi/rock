using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Common;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Core.Errors;
using rock.Core.Services.Common;
using rock.Factories;
using rock.Models;
using rock.Models.CommonApi;
using rock.OAuth;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  // [Authorize(Scopes.MARKET_MANAGER)]
  public class CommonApiController : BaseController
  {
    #region Fields
    private readonly ICommonFactory factory;
    private readonly ICommonService commonService;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public CommonApiController(ICommonService commonService,
                               ICommonFactory factory,
                               IErrorFactory errorFactory)
    {
      this.commonService = commonService;
      this.factory = factory;
      this.errorFactory = errorFactory;
    }
    #endregion
    #region Brand
    [HttpPost("brands")]
    public async Task<Key<int>> CreateBrand([FromBody] BrandModel brandModel, CancellationToken cancellationToken)
    {
      var brand = new Brand();
      this.mapBrand(brand, brandModel);
      var result = await commonService.InsertBrand(brand, cancellationToken);
      return new Key<int>(result.Id);
    }
    [HttpPut("brands/{brandId}")]
    public async Task EditBrand([FromRoute] int brandId, [FromBody] BrandModel brandModel, CancellationToken cancellationToken)
    {
      var brand = await commonService.GetBrandById(id: brandId,
                                                   cancellationToken: cancellationToken);
      this.mapBrand(brand, brandModel);
      await commonService.UpdateBrand(brand: brand,
                                      cancellationToken: cancellationToken);
    }
    [HttpGet("brands/{brandId}")]
    public async Task<BrandModel> GetBrand([FromRoute] int brandId, CancellationToken cancellationToken)
    {
      return await factory.PrepareBrandModel(brandId, cancellationToken);
    }
    [HttpDelete("brands/{brandId}")]
    public async Task DeleteBrand([FromRoute] int brandId, CancellationToken cancellationToken)
    {
      var brand = await commonService.GetBrandById(id: brandId,
                                                   cancellationToken: cancellationToken);
      await commonService.DeleteBrand(brand, cancellationToken);
    }
    [HttpGet("brands")]
    public async Task<IList<BrandModel>> GetBrands(CancellationToken cancellationToken)
    {
      return await factory.PrepareBrandListModel(cancellationToken);
    }
    private void mapBrand(Brand brand, BrandModel model)
    {
      brand.Name = model.Name;
      brand.UrlTitle = model.UrlTitle;
      brand.BrowserTitle = model.BrowserTitle;
      brand.MetaDescription = model.MetaDescription;
      brand.Description = model.Description;
      brand.ImageAlt = model.ImageAlt;
      brand.ImageTitle = model.ImageTitle;
      brand.ImageId = model.ImageId;
      brand.RowVersion = model.RowVersion;
    }
    #endregion
    #region Color
    [HttpPost("colors")]
    public async Task<ColorModel> CreateColor([FromBody] ColorModel colorModel, CancellationToken cancellationToken)
    {
      var color = new Color();
      this.mapColor(color, colorModel);
      await commonService.InsertColor(color, cancellationToken);
      return await factory.PrepareColorModel(color.Id, cancellationToken);
    }
    [HttpPut("colors/{colorId}")]
    public async Task EditColor([FromRoute] int colorId, CancellationToken cancellationToken, [FromBody] ColorModel updatedColor)
    {
      var color = await commonService.GetColorById(colorId, cancellationToken);
      this.mapColor(color, updatedColor);
      await commonService.UpdateColor(color, cancellationToken);
    }
    [HttpGet("colors/{colorId}")]
    public async Task<ColorModel> GetColor([FromRoute] int colorId, CancellationToken cancellationToken)
    {
      return await factory.PrepareColorModel(colorId, cancellationToken);
    }
    [HttpDelete("colors/{colorId}")]
    public async Task DeleteColor([FromRoute] int colorId, CancellationToken cancellationToken)
    {
      var color = await commonService.GetColorById(colorId, cancellationToken);
      await commonService.DeleteColor(color, cancellationToken);
    }
    [HttpGet("colors")]
    public async Task<IList<ColorModel>> GetColors(CancellationToken cancellationToken)
    {
      return await factory.PrepareColorListModel(cancellationToken);
    }
    private void mapColor(Color color, ColorModel colorModel)
    {
      color.Code = colorModel.Code;
      color.Name = colorModel.Name;
      color.RowVersion = colorModel.RowVersion;
    }
    #endregion
    #region Currency
    [HttpPost("currencies")]
    public async Task CreateCurrency([FromBody] CurrencyModel newCurrency, CancellationToken cancellationToken)
    {
      var currency = new Currency();
      this.mapCurrency(currency, newCurrency);
      await commonService.InsertCurrency(currency, cancellationToken);
    }
    [HttpPut("currencies/{currencyId}")]
    public async Task EditCurrency([FromRoute] int currencyId, [FromBody] CurrencyModel updatedCurrency, CancellationToken cancellationToken)
    {
      var currency = await commonService.GetCurrencyById(currencyId, cancellationToken);
      this.mapCurrency(currency, updatedCurrency);
      await commonService.UpdateCurrency(currency, cancellationToken);
    }
    [HttpGet("currencies/{currencyId}")]
    public async Task<CurrencyModel> GetCurrency([FromRoute] int currencyId, CancellationToken cancellationToken)
    {
      return await this.factory.PrepareCurrencyModel(currencyId, cancellationToken);
    }
    [HttpDelete("currencies/{currencyId}")]
    public async Task DeleteCurrency([FromRoute] int currencyId, CancellationToken cancellationToken)
    {
      var currency = await commonService.GetCurrencyById(currencyId, cancellationToken);
      await commonService.DeleteCurrency(currency, cancellationToken);
    }
    [HttpGet("currencies")]
    public async Task<IList<CurrencyModel>> GetCurrencies(CancellationToken cancellationToken)
    {
      return await factory.PrepareCurrencyListModel(cancellationToken);
    }
    private void mapCurrency(Currency currency, CurrencyModel model)
    {
      currency.Name = model.Name;
      currency.Ratio = model.Ratio;
      currency.Symbol = model.Symbol;
    }
    #endregion
    #region City
    [HttpPost("cities")]
    public async Task CreateCity([FromBody] CityModel model, CancellationToken cancellationToken)
    {
      var province = await commonService.GetProvinceById(id: model.ProvinceId,
                                                         cancellationToken: cancellationToken);
      var city = new City();
      this.mapCity(city, model);
      await commonService.InsertCity(city: city,
                                     province: province,
                                     cancellationToken: cancellationToken);
    }
    [HttpPut("cities/{cityId}")]
    public async Task EditCity([FromRoute] int cityId, [FromBody] CityModel model, CancellationToken cancellationToken)
    {
      var province = await commonService.GetProvinceById(id: model.ProvinceId,
                                                         cancellationToken: cancellationToken);
      var city = await commonService.GetCityById(id: cityId,
                                                 cancellationToken: cancellationToken);
      this.mapCity(city, model);
      await commonService.UpdateCity(city: city,
                                     province: province,
                                     cancellationToken);
    }
    [HttpGet("cities/{cityId}")]
    public async Task<CityModel> GetCity([FromRoute] int cityId, CancellationToken cancellationToken)
    {
      return await factory.PrepareCityModel(cityId, cancellationToken);
    }
    [HttpDelete("cities/{cityId}")]
    public async Task DeleteCity([FromRoute] int cityId, CancellationToken cancellationToken)
    {
      var city = await commonService.GetCityById(cityId, cancellationToken);
      await commonService.DeleteCity(city, cancellationToken);
    }
    [HttpGet("cities")]
    public async Task<IPagedList<CityModel>> GetCities([FromQuery] CitySearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareCityListModel(parameters, cancellationToken);
    }
    private void mapCity(City city, CityModel model)
    {
      city.Name = model.Name;
    }
    #endregion
    #region Province
    [HttpPost("Provinces")]
    public async Task CreateProvince([FromBody] ProvinceModel newProvince, CancellationToken cancellationToken)
    {
      var province = new Province();
      this.mapProvince(province, newProvince);
      await commonService.InsertProvince(province, cancellationToken);
    }
    [HttpGet("Provinces/{provinceId}/cities")]
    public async Task<IList<CityModel>> GetCities([FromRoute] int provinceId, CancellationToken cancellationToken)
    {
      return await factory.PrepareCityListModel(provinceId, cancellationToken);
    }
    [HttpPut("Provinces/{provinceId}")]
    public async Task EditProvince([FromRoute] int provinceId, [FromBody] ProvinceModel updatedProvince, CancellationToken cancellationToken)
    {
      var province = await commonService.GetProvinceById(provinceId, cancellationToken);
      this.mapProvince(province, updatedProvince);
      await commonService.UpdateProvince(province, cancellationToken);
    }
    [HttpGet("Provinces/{provinceId}")]
    public async Task<ProvinceModel> GetProvince([FromRoute] int provinceId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProvinceModel(provinceId, cancellationToken);
    }
    [HttpDelete("Provinces/{provinceId}")]
    public async Task DeleteProvince([FromRoute] int provinceId, CancellationToken cancellationToken)
    {
      var province = await commonService.GetProvinceById(provinceId, cancellationToken);
      await commonService.DeleteProvince(province, cancellationToken);
    }
    [HttpGet("Provinces")]
    public async Task<IList<ProvinceModel>> GetProvinces([FromQuery] ProvincesSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareProvinceListModel(parameters, cancellationToken);
    }
    private void mapProvince(Province province, ProvinceModel model)
    {
      province.AreaCode = model.AreaCode;
      province.Name = model.Name;
    }
    #endregion
    #region Static
    [HttpPost("statics")]
    public async Task CreateStatic([FromBody] StaticsModel newStatic, CancellationToken cancellationToken)
    {
      var staticKeyValue = new Static();
      this.mapStatic(staticKeyValue, newStatic);
      await commonService.InsertStatic(staticKeyValue, cancellationToken);
    }
    [HttpGet("statics")]
    public async Task<IList<StaticsModel>> GetStatics(CancellationToken cancellationToken)
    {
      return await factory.PrepareStaticListModel(cancellationToken);
    }
    [HttpGet("statics/{staticsKey}")]
    public async Task<StaticsModel> GetStatic([FromRoute] string staticKey, CancellationToken cancellationToken)
    {
      return await factory.PrepareStaticModel(staticKey, cancellationToken);
    }
    [HttpDelete("statics/{staticsKey}")]
    public async Task DeleteStatic([FromRoute] string staticKey, CancellationToken cancellationToken)
    {
      var staticKeyValue = await commonService.GetStaticByKey(staticKey, cancellationToken);
      await commonService.DeleteStatic(staticKeyValue, cancellationToken);
    }
    [HttpPost("statics/{staticsKey}/change-value")]
    public async Task EditStatic([FromRoute] string staticKey, [FromBody] string value, CancellationToken cancellationToken)
    {
      var staticKeyValue = await commonService.GetStaticByKey(staticKey, cancellationToken);
      staticKeyValue.Value = value;
      await commonService.UpdateStatic(staticKeyValue, cancellationToken);
    }
    private void mapStatic(Static staticKeyValue, StaticsModel model)
    {
      staticKeyValue.Key = model.Key;
      staticKeyValue.Value = model.Value;
      staticKeyValue.StaticType = model.StaticType;
    }
    #endregion
    #region Coupons
    [HttpPost("coupons")]
    public async Task CreateCoupon([FromBody] CouponModel newCoupon, CancellationToken cancellationToken)
    {
      var coupon = new Coupon();
      newCoupon.Active = true;
      this.mapCoupon(coupon, newCoupon);
      coupon.RowVersion = null;
      await this.commonService.InsertCoupon(coupon, cancellationToken);
    }
    [HttpGet("coupons")]
    public async Task<IPagedList<CouponModel>> GetDiscountCoupons([FromQuery] CouponSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareCouponListModel(parameters, cancellationToken);
    }
    [HttpGet("coupons/code:{couponCode}")]
    public async Task<CouponModel> GetDiscountCoupon([FromRoute] string couponCode, CancellationToken cancellationToken)
    {
      return await this.factory.PrepareCouponModel(couponCode: couponCode, cancellationToken);
    }
    [HttpGet("coupons/{couponId}")]
    public async Task<CouponModel> GetDiscountCoupon([FromRoute] int couponId, CancellationToken cancellationToken)
    {
      return await this.factory.PrepareCouponModel(couponId: couponId, cancellationToken);
    }
    [HttpPut("coupons/{couponId}")]
    public async Task EditCoupon([FromRoute] int couponId, [FromBody] CouponModel newCoupon, CancellationToken cancellationToken)
    {
      var coupon = await commonService.GetCouponById(couponId, cancellationToken);
      this.mapCoupon(coupon, newCoupon);
      await commonService.UpdateCoupon(coupon, cancellationToken);
    }
    [HttpDelete("coupons/{couponId}")]
    public async Task DeleteCoupon([FromRoute] int couponId, CancellationToken cancellationToken)
    {
      var coupon = await commonService.GetCouponById(couponId, cancellationToken);
      await commonService.DeleteCoupon(coupon, cancellationToken);
    }
    [HttpPost("coupons/{couponId}/activate")]
    public async Task ActivateCoupon([FromRoute] int couponId, CancellationToken cancellationToken)
    {
      var coupon = await commonService.GetCouponById(couponId, cancellationToken);
      coupon.Active = true;
      await commonService.UpdateCoupon(coupon, cancellationToken);
    }
    [HttpPost("coupons/{couponId}/deactivate")]
    public async Task DeactivateCoupon([FromRoute] int couponId, CancellationToken cancellationToken)
    {
      var coupon = await commonService.GetCouponById(couponId, cancellationToken);
      coupon.Active = false;
      await commonService.UpdateCoupon(coupon, cancellationToken);
    }
    private void mapCoupon(Coupon coupon, CouponModel model)
    {
      coupon.Value = model.Value;
      coupon.CouponCode = model.CouponCode;
      coupon.ExpiryDate = model.ExpiryDate;
      coupon.MaxQuantities = model.MaxQuantities;
      coupon.Active = model.Active;
      coupon.MaxQuantitiesPerUser = model.MaxQuantitiesPerUser;
      coupon.RowVersion = model.RowVersion;
    }
    #endregion
    #region Transportation
    [HttpPost("transportations")]
    [AllowAnonymous]
    public async Task CreateTransportation([FromBody] CreateTransportationModel transportationModel, CancellationToken cancellationToken)
    {
      var transportation = this.mapCreateTransportation(transportationModel);
      await this.commonService.CreateTransportation(transportation: transportation, cancellationToken: cancellationToken);
    }
    [HttpPatch("transportations/{transportationId}")]
    public async Task UpdateTransportation([FromRoute] int transportationId, [FromBody] UpdateTransportationModel transportationModel, CancellationToken cancellationToken)
    {
      var transportation = await commonService.GetTransportationById(id: transportationId, cancellationToken: cancellationToken);
      if (transportation == null)
        throw errorFactory.ResourceNotFound(id: transportationId);
      this.mapUpdateTransportation(transportation, transportationModel);
      await this.commonService.UpdateTransportation(transportation: transportation, cancellationToken: cancellationToken);
    }
    [HttpGet("transportations/{transportationId}")]
    public async Task<TransportationModel> GetTransportation([FromRoute] int transportationId, CancellationToken cancellationToken)
    {
      var transportation = await factory.PrepareTransportationModel(transportationId: transportationId, cancellationToken);
      if (transportation == null)
        throw errorFactory.ResourceNotFound(id: transportationId);
      return transportation;
    }
    [HttpGet("transportations")]
    public async Task<IList<TransportationModel>> GetTransportations([FromQuery] TransportationSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareTransportationListModel(parameters: parameters, cancellationToken: cancellationToken);
    }
    private Transportation mapCreateTransportation(CreateTransportationModel transportationModel)
    {
      var transportation = new Transportation();
      transportation.FromCityId = transportationModel.FromCityId;
      transportation.ToCityId = transportationModel.ToCityId;
      transportation.Cost = transportationModel.Cost;
      transportation.Distance = transportationModel.Distance;
      transportation.Description = transportationModel.Description;
      return transportation;
    }
    private Transportation mapUpdateTransportation(Transportation transportation, UpdateTransportationModel transportationModel)
    {
      transportation.Description = transportationModel.Description;
      transportation.RowVersion = transportationModel.RowVersion;
      return transportation;
    }
    #endregion
  }
}