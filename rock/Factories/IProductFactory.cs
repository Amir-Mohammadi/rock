using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Models.ProductApi;
namespace rock.Factories
{
  public interface IProductFactory : IScopedDependency
  {
    #region ProductCategory
    Task<ProductCategoryModel> PrepareProductCategoryModel(int productCategoryId, CancellationToken cancellationToken);
    Task<IList<ProductCategoryModel>> PrepareProductCategoryModels(ProductCategorySearchParameters parameters, CancellationToken cancellationToken);
    Task<IList<ProductCategoryModel>> PrepareProductCategoryChildrenModels(int productCategoryId, CancellationToken cancellationToken);
    #endregion
    #region ProductCategoryProperty
    Task<IList<ProductCategoryPropertyModel>> PrepareProductCategoryPropertyListModel(int productCategoryId, CancellationToken cancellationToken);
    Task<ProductCategoryPropertyModel> PrepareProductCategoryPropertyModel(int productCategoryId, int propertyId, CancellationToken cancellationToken);
    #endregion
    #region Product        
    Task<ProductModel> PrepareProductModel(int productId, CancellationToken cancellationToken);
    Task<IList<ProductModel>> PrepareRecentVisitProductListModel(CancellationToken cancellationToken);
    Task<IList<ProductModel>> PrepareLikedProductListModel(CancellationToken cancellationToken);
    Task<IPagedList<ProductModel>> PrepareProductPagedListModel(ProductSearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region ProductImage
    Task<ProductImageModel> PrepareProductImageModel(int productId, int imageId, CancellationToken cancellationToken);
    Task<IList<ProductImageModel>> PrepareProductImageListModel(int productId, CancellationToken cancellationToken);
    #endregion
    #region ProductColor
    Task<ProductColorModel> PrepareProductColorModel(int productId, int colorId, CancellationToken cancellationToken);
    Task<IList<ProductColorModel>> PrepareProductColorListModel(int productId, CancellationToken cancellationToken);
    #endregion
    #region ProductBrochure
    Task<ProductBrochureModel> PrepareProductBrochureModel(int productId, CancellationToken cancellationToken);
    #endregion
    #region ProductRating
    Task<ProductRatingModel> PrepareProductRatingModel(int productId, int ratingId, CancellationToken cancellationToken);
    Task<IList<ProductRatingModel>> PrepareProductRatingListModel(int productId, CancellationToken cancellationToken);
    #endregion
    #region ProductQuestion
    Task<ProductQuestionModel> PrepareProductQuestionModel(int productId, int questionId, CancellationToken cancellationToken);
    Task<IList<ProductQuestionModel>> PrepareProductQuestionListModel(int productId, CancellationToken cancellationToken);
    #endregion
    #region ProductComment
    Task<ProductCommentModel> PrepareProductCommentModel(int productId, int commentId, CancellationToken cancellationToken);
    Task<IPagedList<ProductCommentModel>> PrepareProductCommentPagedListModel(int productId, ProductCommentSearchParameters parameters, CancellationToken cancellationToken);
    Task<IList<ProductCommentModel>> PrepareUserCommnetListModel(CancellationToken cancellationToken);
    #endregion
    #region ProductCommentReply
    Task<ProductCommentReplyModel> PrepareProductCommentReplyModel(int productId, int commentId, int replyId, CancellationToken cancellationToken);
    Task<IPagedList<ProductCommentReplyModel>> PrepareProductCommentReplyPagedListModel(int productId, int commentId, ProductCommentReplySearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region ProductAnswer
    Task<IPagedList<ProductAnswerModel>> PrepareProductAnswerPagedListModel(int productId, int questionId, ProductAnswerSearchParameters parameters, CancellationToken cancellationToken);
    Task<ProductAnswerModel> PrepareProductAnswerModel(int productId, int questionId, int answerId, CancellationToken cancellationToken);
    #endregion
    #region ProductProperty
    Task<IList<ProductPropertyModel>> PrepareProductPropertyListModel(int productId, CancellationToken cancellationToken, bool? isMain = null);
    #endregion
    #region ProductTag
    Task<ProductTagModel> PrepareProductTagModel(int productId, int tagId, CancellationToken cancellationToken);
    Task<IList<ProductTagModel>> PrepareProductTagListModel(int productId, CancellationToken cancellationToken);
    #endregion
    #region ProductCategoryBrand
    Task<ProductCategoryBrandModel> PrepareProductCategoryBrandModel(int productCategoryId, int brandId, CancellationToken cancellationToken);
    Task<IList<ProductCategoryBrandModel>> PrepareProductCategoryBrandListModel(int productCategoryId, CancellationToken cancellationToken);
    #endregion

    #region  ProductShippingInfo
    Task<ProductShippingInfoModel> PrepareProductShippingInfoModel(int productId, CancellationToken cancellationToken);
    #endregion

    #region  ProductPrice
    Task<IPagedList<ProductPriceModel>> PrepareProductPricePagedListModel(int productId, ProductPriceSearchParameter parameters, CancellationToken cancellationToken);
    #endregion

  }
}