using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Common;
using rock.Factories;
using rock.Filters;
using rock.Models.MarketApi;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [AllowAnonymous]
  public class MarketApiController : BaseController
  {
    #region Fields
    private readonly IMarketStuffFactory marketStuffFactory;
    #endregion
    #region Constractor
    public MarketApiController(IMarketStuffFactory marketStuffFactory)
    {
      this.marketStuffFactory = marketStuffFactory;
    }
    #endregion
    #region MarketStuff    
    [HttpGet("market-stuffs/{marketStuffId}")]
    public async Task<MarketStuffModel> GetMarketStuff([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffModel(marketStuffId, cancellationToken);
    }
    #endregion
    #region MarketBriefStuff
    [HttpGet("market-brief-stuffs/{marketBriefStuffId}")]
    public async Task<MarketBriefStuffModel> GetMarketBriefStuff([FromRoute] int marketBriefStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketBriefStuffModel(marketBriefStuffId, cancellationToken);
    }
    [HttpGet("market-brief-stuffs")]
    public async Task<IPagedList<MarketBriefStuffModel>> GetMarketBriefStuffs([FromQuery] MarketBriefStuffSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketBriefStuffListModel(parameters: parameters,
                                                                       cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("market-brief-stuffs/last-visited")]
    public async Task<IPagedList<MarketBriefStuffModel>> GetMarketBriefLastVisitedStuffs(CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketBriefLastVisitedStuffListModel(cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffCategory
    [HttpGet("market-stuff-categories")]
    public async Task<IList<MarketStuffCategoryModel>> GetMarketStuffCategories(CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCategoryListModel(cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketBrands
    [HttpGet("market-brands/{brandId}")]
    public async Task<MarketBrandModel> GetMarketBrand([FromRoute] int brandId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketBrandModel(brandId, cancellationToken);
    }
    [HttpGet("market-brands")]
    public async Task<IList<MarketBrandModel>> GetMarketBrands(CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketBrandListModel(cancellationToken: cancellationToken);
    }
    [HttpGet("market-brands/{brandId}/stuff-categories")]
    public async Task<IList<MarketStuffCategoryModel>> GetMarketBrandStuffCategories([FromRoute] int brandId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketBrandStuffCategoryListModel(brandId: brandId, cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffProperty
    [HttpGet("market-stuffs/{marketStuffId}/properties")]
    public async Task<IList<MarketStuffPropertyModel>> GetMarketStuffProperties([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffPropertyListModel(marketStuffId: marketStuffId,
                                                                          cancellationToken: cancellationToken);
    }
    [HttpGet("market-stuffs/{marketStuffId}/main-properties")]
    public async Task<IList<MarketStuffPropertyModel>> GetMainMarketStuffProperties([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffPropertyListModel(marketStuffId: marketStuffId,
                                                                          cancellationToken: cancellationToken,
                                                                          isMain: true);
    }
    #endregion
    #region MarketStuffImage
    [HttpGet("market-stuffs/{marketStuffId}/images/{imageId}")]
    public async Task<MarketStuffImageModel> GetMarketStuffImage([FromRoute] int marketStuffId, [FromRoute] int imageId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffImageModel(marketStuffId, imageId, cancellationToken);
    }
    [HttpGet("market-stuffs/{marketStuffId}/images")]
    public async Task<IList<MarketStuffImageModel>> GetMarketStuffImages([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffImageListModel(marketStuffId, cancellationToken);
    }
    #endregion
    #region MarketStuffPrice
    [HttpGet("market-stuffs/{marketStuffId}/prices")]
    public async Task<IList<MarketStuffPriceModel>> GetMarketStuffPrices([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffPriceListModel(marketStuffId, cancellationToken);
    }
    #endregion
    #region MarketStuffColor
    [HttpGet("market-stuffs/{marketStuffId}/colors/{colorId}")]
    public async Task<MarketStuffColorModel> GetMarketStuffColor([FromRoute] int marketStuffId, [FromRoute] int colorId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffColorModel(marketStuffId, colorId, cancellationToken);
    }
    [HttpGet("market-stuffs/{marketStuffId}/colors")]
    public async Task<IList<MarketStuffColorModel>> GetMarketStuffColors([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffColorListModel(marketStuffId, cancellationToken);
    }
    #endregion
    #region MarketStuffBrochure
    [HttpGet("market-stuffs/{marketStuffId}/brochure")]
    public async Task<MarketStuffBrochureModel> GetMarketStuffBrochure([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffBrochureModel(marketStuffId: marketStuffId,
                                                            cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffTags
    [HttpGet("market-stuffs/{marketStuffId}/tags/{tagId}")]
    public async Task<MarketStuffTagModel> GetMarketStuffTag([FromRoute] int marketStuffId, [FromRoute] int tagId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffTagModel(marketStuffId, tagId, cancellationToken);
    }
    [HttpGet("market-stuffs/{marketStuffId}/tags")]
    public async Task<IList<MarketStuffTagModel>> GetPagedMarketStuffTags([FromRoute] int marketStuffId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffTagListModel(marketStuffId, cancellationToken);
    }
    #endregion
    #region SetCurrentCity
    #endregion
    #region MarketStuffCategoryProperty
    [HttpGet("market-stuff-categories/{marketStuffCategoryId}/properties")]
    public async Task<IList<MarketStuffCategoryPropertyModel>> GetMarketStuffCategoryProperties([FromRoute] int marketStuffCategoryId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCategoryPropertyListModel(marketStuffCategoryId: marketStuffCategoryId, cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffCategoryStuffPrices 
    [HttpGet("market-stuff-categories/{marketStuffCategoryId}/stuff-prices")]
    public async Task<IList<double>> GetMarketStuffCategoryStuffPrices([FromRoute] int marketStuffCategoryId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCategoryStuffPriceListModel(marketStuffCategoryId: marketStuffCategoryId, cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffCategoryBrand
    [HttpGet("market-stuff-categories/{marketStuffCategoryId}/brands")]
    public async Task<IList<MarketBrandModel>> GetMarketStuffCategoryBrands([FromRoute] int marketStuffCategoryId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCategoryBrandListModel(marketStuffCategoryId: marketStuffCategoryId, cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffCategoryPropertyValues
    [HttpGet("market-stuff-categories/{marketStuffCategoryId}/properties/{propertyId}/values")]
    public async Task<IList<MarketStuffCategoryPropertyValueModel>> GetMarketStuffCategoryPropertyValues([FromRoute] int marketStuffCategoryId, [FromRoute] int propertyId, CancellationToken cancellationToken)
    {
      return await marketStuffFactory.PrepareMarketStuffCategoryPropertyValueListModel(marketStuffCategoryId: marketStuffCategoryId,
                                                                                       propertyId: propertyId,
                                                                                       cancellationToken: cancellationToken);
    }
    #endregion
  }
}