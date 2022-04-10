using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Core.Domains.Users;
using rock.Filters;
using rock.Framework.Autofac;
using rock.Models.MarketApi;
using rock.Models.ProductApi;
namespace rock.Factories
{
  public interface IMarketStuffFactory : IScopedDependency
  {
    #region MarketBriefStuff
    Task<IPagedList<MarketBriefStuffModel>> PrepareMarketBriefStuffListModel(MarketBriefStuffSearchParameters parameters, CancellationToken cancellationToken);
    Task<MarketBriefStuffModel> PrepareMarketBriefStuffModel(int stuffId, CancellationToken cancellationToken);
    Task<IPagedList<MarketBriefStuffModel>> PrepareMarketBriefLastVisitedStuffListModel(CancellationToken cancellationToken);
    #endregion
    #region MarketStuff
    Task<MarketStuffModel> PrepareMarketStuffModel(int marketStuffId, CancellationToken cancellationToken);
    #endregion
    #region MarketProductCategory
    Task<IList<MarketStuffCategoryModel>> PrepareMarketStuffCategoryListModel(CancellationToken cancellationToken);
    #endregion
    #region MarketStuff
    Task<IList<MarketBrandModel>> PrepareMarketBrandListModel(CancellationToken cancellationToken);
    Task<MarketBrandModel> PrepareMarketBrandModel(int marketBrandId, CancellationToken cancellationToken);
    Task<IList<MarketStuffCategoryModel>> PrepareMarketBrandStuffCategoryListModel(int brandId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffQuestion
    Task<IPagedList<MarketStuffQuestionModel>> PrepareMarketStuffQuestionListModel(int marketStuffId, PagedListFilter pagedListFilter, CancellationToken cancellationToken);
    Task<MarketStuffQuestionModel> PrepareMarketStuffQuestionModel(int marketStuffId, int questionId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffProperty
    Task<IList<MarketStuffPropertyModel>> PrepareMarketStuffPropertyListModel(int marketStuffId, CancellationToken cancellationToken, bool? isMain = null);
    Task<MarketStuffImageModel> PrepareMarketStuffImageModel(int marketStuffId, int imageId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffImage
    Task<IList<MarketStuffImageModel>> PrepareMarketStuffImageListModel(int marketStuffId, CancellationToken cancellationToken);
    Task<MarketStuffColorModel> PrepareMarketStuffColorModel(int marketStuffId, int colorId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffPrice
    Task<IList<MarketStuffPriceModel>> PrepareMarketStuffPriceListModel(int marketStuffId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffColor
    Task<IList<MarketStuffColorModel>> PrepareMarketStuffColorListModel(int marketStuffId, CancellationToken cancellationToken);
    Task<MarketStuffBrochureModel> PrepareMarketStuffBrochureModel(int marketStuffId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffTag
    Task<MarketStuffTagModel> PrepareMarketStuffTagModel(int marketStuffId, int tagId, CancellationToken cancellationToken);
    Task<IList<MarketStuffTagModel>> PrepareMarketStuffTagListModel(int marketStuffId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffAnswer
    Task<IPagedList<MarketStuffAnswerModel>> PrepareMarketStuffAnswerPagedListModel(int marketStuffId, int questionId, ProductAnswerSearchParameters parameters, CancellationToken cancellationToken);
    Task<MarketStuffAnswerModel> PrepareMarketStuffAnswerModel(int marketStuffId, int questionId, int answerId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffComment
    Task<MarketStuffCommentModel> PrepareMarketStuffCommentModel(int marketStuffId, int commentId, CancellationToken cancellationToken);
    Task<IPagedList<MarketStuffCommentModel>> PrepareMarketStuffCommentPagedListModel(int marketStuffId, ProductCommentSearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffCommentReply
    Task<MarketStuffCommentReplyModel> PrepareMarketStuffCommentReplyModel(int marketStuffId, int commentId, int replyId, CancellationToken cancellationToken);
    Task<IPagedList<MarketStuffCommentReplyModel>> PrepareMarketStuffCommentReplyPagedListModel(int marketStuffId, int commentId, ProductCommentReplySearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffCategoryProperty
    Task<IList<MarketStuffCategoryPropertyModel>> PrepareMarketStuffCategoryPropertyListModel(int marketStuffCategoryId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffCategoryPropertyValue
    Task<IList<MarketStuffCategoryPropertyValueModel>> PrepareMarketStuffCategoryPropertyValueListModel(int marketStuffCategoryId, int propertyId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffCategoryBrand
    Task<IList<MarketBrandModel>> PrepareMarketStuffCategoryBrandListModel(int marketStuffCategoryId, CancellationToken cancellationToken);
    #endregion
    #region MarketStuffCategoryStuffPrice
    Task<IList<double>> PrepareMarketStuffCategoryStuffPriceListModel(int marketStuffCategoryId, CancellationToken cancellationToken);
    #endregion
  }
}