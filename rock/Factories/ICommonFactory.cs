using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Models.CommonApi;
namespace rock.Factories
{
  public interface ICommonFactory : IScopedDependency
  {
    #region Brand
    Task<BrandModel> PrepareBrandModel(int brandId, CancellationToken cancellationToken);
    Task<IList<BrandModel>> PrepareBrandListModel(CancellationToken cancellationToken);
    #endregion
    #region Currency
    Task<CurrencyModel> PrepareCurrencyModel(int currencyId, CancellationToken cancellationToken);
    Task<IList<CurrencyModel>> PrepareCurrencyListModel(CancellationToken cancellationToken);
    #endregion
    #region Color
    Task<ColorModel> PrepareColorModel(int colorId, CancellationToken cancellationToken);
    Task<IList<ColorModel>> PrepareColorListModel(CancellationToken cancellationToken);
    #endregion
    #region City
    Task<CityModel> PrepareCityModel(int cityId, CancellationToken cancellationToken);
    Task<IPagedList<CityModel>> PrepareCityListModel(CitySearchParameters parameters, CancellationToken cancellationToken);
    Task<IList<CityModel>> PrepareCityListModel(int provinceId, CancellationToken cancellationToken);
    #endregion
    #region Province
    Task<ProvinceModel> PrepareProvinceModel(int provinceId, CancellationToken cancellationToken);
    Task<IList<ProvinceModel>> PrepareProvinceListModel(ProvincesSearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region Static
    Task<StaticsModel> PrepareStaticModel(string staticKey, CancellationToken cancellationToken);
    Task<IList<StaticsModel>> PrepareStaticListModel(CancellationToken cancellationToken);
    #endregion
    #region Coupon
    Task<CouponModel> PrepareCouponModel(int couponId, CancellationToken cancellationToken);
    Task<CouponModel> PrepareCouponModel(string couponCode, CancellationToken cancellationToken);
    Task<IPagedList<CouponModel>> PrepareCouponListModel(CouponSearchParameters parameters, CancellationToken cancellationToken);
    #endregion

    #region  Transportation
    Task<TransportationModel> PrepareTransportationModel(int transportationId, CancellationToken cancellationToken);
    Task<IList<TransportationModel>> PrepareTransportationListModel(TransportationSearchParameters parameters, CancellationToken cancellationToken);
    #endregion
  }
}