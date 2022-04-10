using System.Collections.Generic;
using rock.Framework.Autofac;
using rock.Core.Domains.Products;
using rock.Core.Domains.Catalogs;
using rock.Core.Domains.Threads;
using rock.Core.Domains.Commons;
using rock.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
namespace rock.Core.Services.Products
{
  public interface IProductService : IScopedDependency
  {
    #region ProductCategory
    Task<ProductCategory> InsertProductCategory(ProductCategory productCategory, ProductCategory parentProductCategory, CancellationToken cancellationToken);
    Task UpdateProductCategory(ProductCategory productCategory, ProductCategory parentProductCategory, CancellationToken cancellationToken);
    Task DeleteProductCategory(ProductCategory productCategory, CancellationToken cancellationToken);
    Task ArchiveProductCategory(ProductCategory productCategory, CancellationToken cancellationToken);
    Task RestoreProductCategory(ProductCategory productCategory, CancellationToken cancellationToken);
    IQueryable<ProductCategory> GetProductCategories(int? brandId = null, string name = null, bool showArchive = true, bool? isPublished = null, IInclude<ProductCategory> include = null);
    IQueryable<ProductCategory> GetProductCategoryChildrenByParentId(int categoryId, bool showArchive = true, bool? isPublished = null, IInclude<ProductCategory> include = null);
    Task<ProductCategory> GetProductCategoryById(int id, CancellationToken cancellationToken, IInclude<ProductCategory> include = null);
    #endregion
    #region ProductCategoryProperty
    Task<CatalogItem> InsertProductCategoryProperty(CatalogItem catalogItem, CatalogItem reference, ProductCategory category, CancellationToken cancellationToken);
    Task UpdateProductCategoryProperty(CatalogItem catalogItem, CancellationToken cancellationToken);
    Task DeleteProductCategoryProperty(CatalogItem catalogItem, CancellationToken cancellationToken);
    IQueryable<CatalogItem> GetProductCategoryProperties(int productCategoryId, IInclude<CatalogItem> include = null);
    Task<CatalogItem> GetProductCategoryPropertyById(int id, int productCategoryId, CancellationToken cancellationToken, IInclude<CatalogItem> include = null);
    #endregion
    #region Product
    Task<Product> InsertProduct(Product product, ProductCategory category, Brand brand, CancellationToken cancellationToken);
    Task UpdateProduct(Product product, ProductCategory category, Brand brand, CancellationToken cancellationToken);
    Task DeleteProduct(Product product, CancellationToken cancellationToken);
    Task ArchiveProduct(Product product, CancellationToken cancellationToken);
    Task RestoreProduct(Product product, CancellationToken cancellationToken);
    Task<Product> GetProductById(int id, CancellationToken cancellationToken, IInclude<Product> include = null);
    bool CheckProductExistencById(int id);
    Task VisitProductById(int id, CancellationToken cancellationToken);
    IQueryable<Product> GetProducts(string name = null, int? threadId = null, int? brandId = null, int? productCategoryId = null, bool showArchive = true, IInclude<Product> include = null);
    IQueryable<Product> GetUserLikedProducts(int? productId = null, IInclude<Product> include = null);
    IQueryable<Product> GetUserRecentVisitProducts(IInclude<Product> include = null);
    #endregion
    #region ProductProperty
    Task<CatalogMemoryItem> InsertProductProperty(CatalogMemoryItem catalogMemoryItem, CatalogItem catalogItem, Product product, CancellationToken cancellationToken);
    Task UpdateProductProperty(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken);
    Task DeleteProductProperty(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken);
    IQueryable<CatalogMemoryItem> GetProductProperties(int? productId = null, bool? isMain = null, IInclude<CatalogMemoryItem> include = null);
    Task<CatalogMemoryItem> GetProductPropertyById(int productId, int propertyId, CancellationToken cancellationToken, IInclude<CatalogMemoryItem> include = null);
    #endregion
    #region ProductQuestion
    Task<ThreadActivity> AskProductQuestion(ThreadActivity question, Product product, CancellationToken cancellationToken);
    Task DeleteProductQuestion(ThreadActivity question, CancellationToken cancellationToken, bool checkOwner = false);
    Task UpdateProductQuestion(ThreadActivity question, CancellationToken cancellationToken, bool checkOwner = false);
    IQueryable<ThreadActivity> GetProductQuestions(int productId, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    Task<ThreadActivity> GetProductQuestionById(int productId, int questionId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    #endregion
    #region Answer
    Task<ThreadActivity> AnswerProductQuestion(ThreadActivity answer, ThreadActivity question, CancellationToken cancellationToken);
    Task DeleteProductAnswer(ThreadActivity answer, CancellationToken cancellationToken, bool checkOwner = false);
    Task UpdateProductAnswer(ThreadActivity answer, CancellationToken cancellationToken, bool checkOwner = false);
    Task<ThreadActivity> GetProductAnswer(int productId, int questionId, int answerId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    IQueryable<ThreadActivity> GetProductAnswers(int productId, int questionId, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    #endregion
    #region ProductRating and Rate
    Task<ThreadActivity> InsertProductRating(ThreadActivity rating, IList<ThreadActivity> conditions, Product product, CancellationToken cancellationToken);
    Task DeleteProductRating(ThreadActivity rating, CancellationToken cancellationToken);
    Task<ThreadActivity> GetProductRating(int productId, int ratingId, CancellationToken cancellationToken, IInclude<ThreadActivity> include = null);
    IQueryable<ThreadActivity> GetProductRatings(int productId, IInclude<ThreadActivity> include = null);
    #endregion
    #region Like and Rate
    Task<ThreadActivity> RateProduct(ThreadActivity ratingCondition, Product product, CancellationToken cancellationToken);
    Task LikeProduct(Product product, CancellationToken cancellationToken);
    Task UnlikeProduct(Product product, CancellationToken cancellationToken);
    #endregion
    #region ProductComment
    Task<ThreadActivity> InsertProductComment(ThreadActivity comment, Product product, CancellationToken cancellationToken);
    Task PublishProductCategory(ProductCategory productCategory, CancellationToken cancellationToken);
    Task UpdateProductComment(ThreadActivity comment, CancellationToken cancellationToken, bool checkOwner = false);
    Task<ThreadActivity> GetProductCommentById(int productId, int commentId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    IQueryable<ThreadActivity> GetProductComments(int productId, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    Task DeleteProductComment(ThreadActivity comment, CancellationToken cancellationToken, bool checkOwner = false);
    Task UnPublishProductCategory(ProductCategory productCategory, CancellationToken cancellationToken);
    Task LikeProductComment(ThreadActivity comment, CancellationToken cancellationToken);
    Task UnlikeProductComment(ThreadActivity comment, CancellationToken cancellationToken);
    #endregion
    #region ProductCommentReply
    Task<ThreadActivity> InsertProductCommentReply(ThreadActivity reply, ThreadActivity comment, CancellationToken cancellationToken);
    Task UpdateProductCommentReply(ThreadActivity reply, CancellationToken cancellationToken, bool checkOwner = false);
    IQueryable<ThreadActivity> GetProductCommentReplies(int productId, int commentId, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    Task<ThreadActivity> GetProductCommentReplyById(int productId, int commentId, int replyId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    Task DeleteProductCommentReply(ThreadActivity reply, CancellationToken cancellationToken, bool checkOwner = false);
    #endregion
    #region ProductColor
    Task<ProductColor> NewProductColor(Color color, Product product, CancellationToken cancellationToken);
    Task DeleteProductColor(ProductColor productColor, CancellationToken cancellationToken);
    Task<ProductColor> GetProductColorById(int productId, int colorId, CancellationToken cancellationToken, IInclude<ProductColor> include = null);
    IQueryable<ProductColor> GetProductColors(int productId, IInclude<ProductColor> include = null);
    Task SetDefaultProductColor(ProductColor productColor, Product product, CancellationToken cancellationToken);
    #endregion
    #region ProductImage
    Task<ProductImage> InsertProductImage(ProductImage productImage, Product product, CancellationToken cancellationToken);
    Task DeleteProductImage(ProductImage productImage, CancellationToken cancellationToken);
    Task SetPreviewProductImage(ProductImage productImage, Product product, CancellationToken cancellationToken);
    Task<ProductImage> GetProductImageById(int productId, int imageId, CancellationToken cancellationToken, IInclude<ProductImage> include = null);
    IQueryable<ProductImage> GetProductImages(int productId, IInclude<ProductImage> include = null);
    #endregion
    #region ProductPrice
    Task<ProductPrice> CreateProductPrice(ProductPrice productPrice, CancellationToken cancellationToken);
    Task DeleteProductPrice(ProductPrice productPrice, CancellationToken cancellationToken);
    Task<ProductPrice> GetProductPriceById(int id, CancellationToken cancellationToken, IInclude<ProductPrice> include = null);
    IQueryable<ProductPrice> GetProductPrices(int? ProductCategoryId = null, int? productId = null, int? cityId = null, int? provinceId = null, bool? isPublished = null, int? colorId = null, IInclude<ProductPrice> include = null);
    IQueryable<ProductPrice> GetCurrentProductPrices(int? ProductCategoryId = null, int? productId = null, IInclude<ProductPrice> include = null);
    Task<ProductPrice> GetCurrentProductPrice(int productId, int colorId, CancellationToken cancellationToken);
    Task ProductPricePublish(ProductPrice productPrice, CancellationToken cancellationToken);
    Task ProductPriceUnPublish(ProductPrice productPrice, CancellationToken cancellationToken);
    #endregion
    #region ProductBrochure
    Task<ProductBrochure> InsertProductBrochure(ProductBrochure productBrochure, Product product, CancellationToken cancellationToken);
    Task UpdateProductBrochure(ProductBrochure productBrochure, CancellationToken cancellationToken);
    Task DeleteProductBrochure(ProductBrochure productBrochure, CancellationToken cancellationToken);
    Task<ProductBrochure> GetProductBrochureByProductId(int productId, CancellationToken cancellationToken, IInclude<ProductBrochure> include = null);
    #endregion
    #region ProductBrochureAttachment
    Task<ProductBrochureAttachment> InsertProductBrochureAttachment(ProductBrochureAttachment productBrochureAttachment, CancellationToken cancellationToken);
    Task UpdateProductBrochureAttachment(ProductBrochureAttachment productBrochureAttachment, CancellationToken cancellationToken);
    Task DeleteProductBrochureAttachment(ProductBrochureAttachment productBrochureAttachment, CancellationToken cancellationToken);
    Task<ProductBrochureAttachment> GetProductBrochureAttachmentById(int id, CancellationToken cancellationToken, IInclude<ProductBrochureAttachment> include = null);
    IQueryable<ProductBrochureAttachment> GetProductBrochureAttachmentsByBrochureId(int brochureId, IInclude<ProductBrochureAttachment> include = null);
    #endregion
    #region Misc
    Task<Domains.Threads.Thread> GetThreadByProductId(int productId, CancellationToken cancellationToken, IInclude<Domains.Threads.Thread> include = null);
    IQueryable<ThreadActivity> GetUserComments(bool showUnPublished = true, IInclude<ThreadActivity> include = null);
    #endregion
    #region ProductTag
    Task UpdateProductTag(ThreadActivity tag, CancellationToken cancellationToken);
    Task<ThreadActivity> GetProductTagById(int productId, int tagId, CancellationToken cancellationToken, IInclude<ThreadActivity> include = null);
    Task DeleteProductTag(ThreadActivity tag, CancellationToken cancellationToken);
    Task<ThreadActivity> InsertProductTag(ThreadActivity tag, Product product, CancellationToken cancellationToken);
    IQueryable<ThreadActivity> GetProductTags(int productId, IInclude<ThreadActivity> include = null);
    #endregion
    #region ProductCategoryBrand
    Task<ProductCategoryBrand> NewProductCategoryBrand(Brand brand, ProductCategory productCategory, CancellationToken cancellationToken);
    Task DeleteProductCategoryBrand(ProductCategoryBrand productCategoryBrand, CancellationToken cancellationToken);
    Task<ProductCategoryBrand> GetProductCategoryBrandById(int productCategoryId, int brandId, CancellationToken cancellationToken, IInclude<ProductCategoryBrand> include = null);
    IQueryable<ProductCategoryBrand> GetProductCategoryBrands(int? productCategoryId = null, int? brandId = null, IInclude<ProductCategoryBrand> include = null);
    Task UpdateProductImage(ProductImage productImage, Product product, CancellationToken cancellationToken);
    #endregion
    #region  ProductShippingInfo
    Task InsertProductShippingInfo(ProductShippingInfo productShippingInfo, CancellationToken cancellationToken);
    Task UpdateProductShippingInfo(ProductShippingInfo productShippingInfo, CancellationToken cancellationToken);
    Task<ProductShippingInfo> GetProductShippingInfoByProductId(int productId, CancellationToken cancellationToken, IInclude<ProductShippingInfo> include = null);
    #endregion
  }
}