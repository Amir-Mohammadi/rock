using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Catalogs;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Files;
using rock.Core.Domains.Products;
using rock.Core.Domains.Threads;
using rock.Core.Domains.Users;
using rock.Core.Services.Products;
using rock.Models.CommonApi;
using rock.Models.FileApi;
using rock.Models.ProductApi;
using rock.Models.UserApi;
using System.Linq;
namespace rock.Factories
{
  public class ProductFactory : BaseFactory, IProductFactory
  {
    #region Fields
    private readonly IProductService productService;
    private readonly Include<Product> productModelInclude;
    #endregion
    #region Constractor
    public ProductFactory(IProductService productService)
    {
      this.productService = productService;
      this.productModelInclude = new Include<Product>(query =>
     {
       query = query.Include(x => x.PreviewProductImage)
                    .Include(x => x.Brand)
                    .Include(x => x.DefaultProductColor)
                    .ThenInclude(x => x.Color);
       return query;
     });
    }
    #endregion
    #region ProductCategory
    public async Task<ProductCategoryModel> PrepareProductCategoryModel(int productCategoryId, CancellationToken cancellationToken)
    {
      var productCategory = await this.productService.GetProductCategoryById(productCategoryId, cancellationToken);
      return this.createProductCategoryModel(productCategory);
    }
    public async Task<IList<ProductCategoryModel>> PrepareProductCategoryModels(ProductCategorySearchParameters parameters, CancellationToken cancellationToken)
    {
      var productCategories = this.productService.GetProductCategories(name: parameters.Name,
                                                                       showArchive: false);
      return await this.CreateModelListAsync(source: productCategories,
                                             convertFunction: this.createProductCategoryModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<IList<ProductCategoryModel>> PrepareProductCategoryChildrenModels(int productCategoryId, CancellationToken cancellationToken)
    {
      var productCategories = this.productService.GetProductCategoryChildrenByParentId(categoryId: productCategoryId,
                                                                                       showArchive: false);
      return await this.CreateModelListAsync(productCategories, this.createProductCategoryModel, cancellationToken);
    }
    private ProductCategoryModel createProductCategoryModel(ProductCategory productCategory)
    {
      if (productCategory == null)
        return null;
      return new ProductCategoryModel()
      {
        Id = productCategory.Id,
        Name = productCategory.Name,
        UrlTitle = productCategory.UrlTitle,
        BrowserTitle = productCategory.BrowserTitle,
        MetaDescription = productCategory.MetaDescription,
        ParentId = productCategory.ParentId,
        Explanation = productCategory.Explanation,
        IsArchive = productCategory.DeletedAt != null,
        IsPublished = productCategory.IsPublished,
        RowVersion = productCategory.RowVersion,
      };
    }
    #endregion
    #region ProductCategoryProperty
    public async Task<IList<ProductCategoryPropertyModel>> PrepareProductCategoryPropertyListModel(int productCategoryId, CancellationToken cancellationToken)
    {
      var categoryProperties = this.productService.GetProductCategoryProperties(productCategoryId: productCategoryId);
      return await this.CreateModelListAsync(categoryProperties, this.createProductCategoryPropertyModel, cancellationToken);
    }
    public async Task<ProductCategoryPropertyModel> PrepareProductCategoryPropertyModel(int productCategoryId, int propertyId, CancellationToken cancellationToken)
    {
      var categoryProperty = await this.productService.GetProductCategoryPropertyById(id: propertyId,
                                                                            productCategoryId: productCategoryId,
                                                                            cancellationToken: cancellationToken);
      return this.createProductCategoryPropertyModel(categoryProperty);
    }
    private ProductCategoryPropertyModel createProductCategoryPropertyModel(CatalogItem productCategoryProperty)
    {
      if (productCategoryProperty == null)
        return null;
      return new ProductCategoryPropertyModel()
      {
        Id = productCategoryProperty.Id,
        ReferenceId = productCategoryProperty.ReferenceId,
        CatalogId = productCategoryProperty.CatalogId,
        Title = productCategoryProperty.Value,
        Type = productCategoryProperty.Type,
        ShowInFilter = productCategoryProperty.ShowInFilter,
        HasMultiple = productCategoryProperty.HasMultiple,
        IsMain = productCategoryProperty.IsMain,
        Order = productCategoryProperty.Order,
        RowVersion = productCategoryProperty.RowVersion,
      };
    }
    #endregion
    #region Product        
    public async Task<ProductModel> PrepareProductModel(int productId, CancellationToken cancellationToken)
    {
      var product = await this.productService.GetProductById(id: productId,
                                                             cancellationToken: cancellationToken,
                                                             include: productModelInclude);
      return this.createProductModel(product);
    }
    public async Task<IPagedList<ProductModel>> PrepareProductPagedListModel(ProductSearchParameters parameters, CancellationToken cancellationToken)
    {
      var products = this.productService.GetProducts(name: parameters.Name,
                                                     brandId: parameters.BrandId,
                                                     productCategoryId: parameters.ProductCategoryId,
                                                     showArchive: false,
                                                     include: productModelInclude);
      return await this.CreateModelPagedListAsync(source: products,
                                                  convertFunction: this.createProductModel,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  cancellationToken: cancellationToken);
    }
    private ProductModel createProductModel(Product product)
    {
      if (product == null)
        return null;
      ProductPrice price = null;
      price = productService.GetCurrentProductPrices(productId: product.Id)
                              .OrderBy(x => x.Price)
                              .FirstOrDefault();
      return new ProductModel()
      {
        Id = product.Id,
        Name = product.Name,
        UrlTitle = product.UrlTitle,
        BrowserTitle = product.BrowserTitle,
        MetaDescription = product.MetaDescription,
        BriefDescription = product.BriefDescription,
        BrandId = product.BrandId,
        CategoryId = product.ProductCategoryId,
        PreviewProductImage = createProductImageModel(product.PreviewProductImage),
        RowVersion = product.RowVersion,
        BrandName = product.Brand.Name,
        Price = price?.Price ?? 0,
        Discount = price?.Discount ?? 0,
        DefaultProductColor = createProductColorModel(product.DefaultProductColor),
      };
    }
    #endregion
    #region ProductImage
    public async Task<IList<ProductImageModel>> PrepareProductImageListModel(int productId, CancellationToken cancellationToken)
    {
      var productImages = this.productService.GetProductImages(productId: productId);
      return await this.CreateModelListAsync(productImages, this.createProductImageModel, cancellationToken);
    }
    public async Task<ProductImageModel> PrepareProductImageModel(int productId, int imageId, CancellationToken cancellationToken)
    {
      var productImage = await this.productService.GetProductImageById(productId: productId, imageId: imageId, cancellationToken: cancellationToken);
      return this.createProductImageModel(productImage: productImage);
    }
    #endregion
    #region ProductColor
    public async Task<IList<ProductColorModel>> PrepareProductColorListModel(int productId, CancellationToken cancellationToken)
    {
      var productColors = this.productService.GetProductColors(productId: productId,
                                                               include: new Include<ProductColor>(query =>
                                                               {
                                                                 query = query.Include(x => x.Color);
                                                                 return query;
                                                               }));
      return await this.CreateModelListAsync(productColors, this.createProductColorModel, cancellationToken);
    }
    public async Task<ProductColorModel> PrepareProductColorModel(int productId, int colorId, CancellationToken cancellationToken)
    {
      var productColor = await this.productService.GetProductColorById(productId: productId,
                                                                       colorId: colorId,
                                                                       cancellationToken: cancellationToken,
                                                                       include: new Include<ProductColor>(query =>
                                                                       {
                                                                         query = query.Include(x => x.Color);
                                                                         return query;
                                                                       }));
      return this.createProductColorModel(productColor);
    }
    private ProductColorModel createProductColorModel(ProductColor productColor)
    {
      if (productColor == null)
        return null;
      return new ProductColorModel()
      {
        ColorId = productColor.ColorId,
        ProductId = productColor.ProductId,
        Color = createColorModel(productColor.Color),
      };
    }
    private ColorModel createColorModel(Color color)
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
    #region ProductBrochure
    public async Task<ProductBrochureModel> PrepareProductBrochureModel(int productId, CancellationToken cancellationToken)
    {
      var productBrochure = await this.productService.GetProductBrochureByProductId(productId, cancellationToken);
      return this.createProductBrochureModel(productBrochure);
    }
    private ProductBrochureModel createProductBrochureModel(ProductBrochure productBrochure)
    {
      if (productBrochure == null)
        return null;
      return new ProductBrochureModel()
      {
        Id = productBrochure.Id,
        RowVersion = productBrochure.RowVersion,
        HTML = productBrochure.HTML,
        Attachments = createProductBrochureAttachmentListModel(productBrochure.Attachments)
      };
    }
    private IList<ProductBrochureAttachmentModel> createProductBrochureAttachmentListModel(ICollection<ProductBrochureAttachment> attachments)
    {
      if (attachments == null)
        return null;
      return CreateModelList(attachments, this.createProductBrochureAttachmentModel);
    }
    private ProductBrochureAttachmentModel createProductBrochureAttachmentModel(ProductBrochureAttachment productBrochureAttachment)
    {
      if (productBrochureAttachment == null)
        return null;
      return new ProductBrochureAttachmentModel()
      {
        Id = productBrochureAttachment.Id,
        File = this.createFileModel(productBrochureAttachment.File),
        ProductBrochurId = productBrochureAttachment.BrochurId,
        RowVersion = productBrochureAttachment.RowVersion
      };
    }
    private FileModel createFileModel(File file)
    {
      if (file == null)
        return null;
      return new FileModel()
      {
        Id = file.Id,
        Access = file.Access,
        Ext = file.FileType,
        Owners = file.OwnerGroup,
        RowVersion = file.RowVersion
      };
    }
    #endregion
    #region ProductRating
    public async Task<IList<ProductRatingModel>> PrepareProductRatingListModel(int productId, CancellationToken cancellationToken)
    {
      var productRatings = this.productService.GetProductRatings(productId: productId);
      return await this.CreateModelListAsync(productRatings, this.createProductRatingModel, cancellationToken);
    }
    public async Task<ProductRatingModel> PrepareProductRatingModel(int productId, int ratingId, CancellationToken cancellationToken)
    {
      var productRating = await this.productService.GetProductRating(productId: productId,
                                                                     ratingId: ratingId,
                                                                     cancellationToken: cancellationToken);
      return this.createProductRatingModel(productRating: productRating);
    }
    private ProductRatingModel createProductRatingModel(ThreadActivity productRating)
    {
      if (productRating == null)
        return null;
      return new ProductRatingModel()
      {
        RowVersion = productRating.RowVersion,
        Conditions = null,
        Id = productRating.Id,
        Title = productRating.Payload,
        User = this.createSimpleUserModel(productRating.User)
      };
    }
    private SimpleUserModel createSimpleUserModel(User user)
    {
      if (user == null)
        return null;
      return new SimpleUserModel()
      {
        Id = user.Id,
        FullName = user.Profile?.PersonProfile?.FirstName + " " + user.Profile?.PersonProfile?.LastName,
        ProfileId = user.ProfileId,
        Role = user.Role
      };
    }
    #endregion
    #region ProductQuestion
    public async Task<IList<ProductQuestionModel>> PrepareProductQuestionListModel(int productId, CancellationToken cancellationToken)
    {
      var productQuestins = this.productService.GetProductQuestions(productId: productId,
                                                                    include: new Include<ThreadActivity>(query =>
                                                                    {
                                                                      query = query.Include(x => x.User);
                                                                      return query;
                                                                    }));
      return await this.CreateModelListAsync(productQuestins, this.createProductQuestionModel, cancellationToken);
    }
    public async Task<ProductQuestionModel> PrepareProductQuestionModel(int productId, int questionId, CancellationToken cancellationToken)
    {
      var productQuestion = await this.productService.GetProductQuestionById(productId: productId, questionId: questionId, cancellationToken: cancellationToken);
      return this.createProductQuestionModel(productQuestion: productQuestion);
    }
    private ProductQuestionModel createProductQuestionModel(ThreadActivity productQuestion)
    {
      if (productQuestion == null)
        return null;
      return new ProductQuestionModel()
      {
        Id = productQuestion.Id,
        Question = productQuestion.Payload,
        User = this.createSimpleUserModel(productQuestion.User)
      };
    }
    #endregion
    #region ProductComment
    public async Task<ProductCommentModel> PrepareProductCommentModel(int productId, int commentId, CancellationToken cancellationToken)
    {
      var productComment = await this.productService.GetProductCommentById(productId: productId,
                                                                           commentId: commentId,
                                                                           cancellationToken: cancellationToken);
      return this.createProductCommentModel(productComment: productComment);
    }
    public async Task<IPagedList<ProductCommentModel>> PrepareProductCommentPagedListModel(int productId, ProductCommentSearchParameters parameters, CancellationToken cancellationToken)
    {
      var productComments = this.productService.GetProductComments(productId: productId);
      return await this.CreateModelPagedListAsync(source: productComments,
                                       this.createProductCommentModel,
                                       order: parameters.Order,
                                       pageIndex: parameters.PageIndex,
                                       pageSize: parameters.PageSize,
                                       sortBy: parameters.SortBy,
                                       cancellationToken: cancellationToken);
    }
    private ProductCommentModel createProductCommentModel(ThreadActivity productComment)
    {
      if (productComment == null)
        return null;
      Product product = null;
      product = productService.GetProducts(threadId: productComment.ThreadId, include: new Include<Product>(query =>
                                                                             {
                                                                               query = query.Include(x => x.PreviewProductImage)
                                                                               .Include(x => x.Brand)
                                                                               .Include(x => x.DefaultProductColor)
                                                                               .Include(x => x.DefaultProductColor).ThenInclude(x => x.Color);
                                                                               return query;
                                                                             })).FirstOrDefault();
      return new ProductCommentModel()
      {
        Author = this.createSimpleUserModel(productComment.User),
        CreatedAt = productComment.CreatedAt,
        Id = productComment.Id,
        Text = productComment.Payload,
        UpdateAt = productComment.UpdatedAt,
        ProductName = product?.Name ?? null,
        PreviewProductImage = createProductImageModel(product?.PreviewProductImage) ?? null,
        DefaultProductColor = createProductColorModel(product?.DefaultProductColor) ?? null,
        ProductId = product?.Id ?? null,
        BrandName = createBrandModel(product?.Brand)?.Name ?? null
      };
    }
    #endregion
    #region ProductCommentReply
    public async Task<ProductCommentReplyModel> PrepareProductCommentReplyModel(int productId, int commentId, int replyId, CancellationToken cancellationToken)
    {
      var productCommnetReply = await productService.GetProductCommentReplyById(productId, commentId, replyId, cancellationToken);
      return createProductCommentReplyModel(productCommentReply: productCommnetReply);
    }
    public async Task<IPagedList<ProductCommentReplyModel>> PrepareProductCommentReplyPagedListModel(int productId, int commentId, ProductCommentReplySearchParameters parameters, CancellationToken cancellationToken)
    {
      var productCommentReplies = this.productService.GetProductCommentReplies(productId: productId, commentId: commentId);
      return await this.CreateModelPagedListAsync(source: productCommentReplies,
                                       convertFunction: this.createProductCommentReplyModel,
                                       order: parameters.Order,
                                       pageIndex: parameters.PageIndex,
                                       pageSize: parameters.PageSize,
                                       sortBy: parameters.SortBy,
                                       cancellationToken: cancellationToken);
    }
    private ProductCommentReplyModel createProductCommentReplyModel(ThreadActivity productCommentReply)
    {
      if (productCommentReply == null)
        return null;
      return new ProductCommentReplyModel()
      {
        Author = this.createSimpleUserModel(productCommentReply.User),
        CreatedAt = productCommentReply.CreatedAt,
        Id = productCommentReply.Id,
        Text = productCommentReply.Payload,
        UpdateAt = productCommentReply.UpdatedAt
      };
    }
    #endregion
    #region ProductAnswer
    public async Task<ProductAnswerModel> PrepareProductAnswerModel(int productId, int questionId, int answerId, CancellationToken cancellationToken)
    {
      var answer = await this.productService.GetProductAnswer(productId: productId, questionId: questionId, answerId: answerId, cancellationToken);
      return this.createProductAnswerModel(answer);
    }
    public async Task<IPagedList<ProductAnswerModel>> PrepareProductAnswerPagedListModel(int productId, int questionId, ProductAnswerSearchParameters parameters, CancellationToken cancellationToken)
    {
      var answers = this.productService.GetProductAnswers(productId: productId,
                                                          questionId: questionId);
      return await this.CreateModelPagedListAsync(source: answers,
                                                  convertFunction: this.createProductAnswerModel,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  cancellationToken: cancellationToken);
    }
    private ProductAnswerModel createProductAnswerModel(ThreadActivity productAnswer)
    {
      if (productAnswer == null)
        return null;
      return new ProductAnswerModel()
      {
        Id = productAnswer.Id,
        Answer = productAnswer.Payload,
        User = this.createSimpleUserModel(productAnswer.User)
      };
    }
    #endregion
    #region ProductProperty
    public async Task<IList<ProductPropertyModel>> PrepareProductPropertyListModel(int productId, CancellationToken cancellationToken, bool? isMain = null)
    {
      var productProperties = this.productService.GetProductProperties(productId: productId,
                                                                        isMain: isMain,
                                                                        include: new Include<CatalogMemoryItem>(query =>
                                                                        {
                                                                          query = query.Include(x => x.CatalogItem);
                                                                          return query;
                                                                        }));
      return await this.CreateModelListAsync(source: productProperties,
                                             convertFunction: createProductPropertyModel,
                                             cancellationToken: cancellationToken);
    }
    private ProductPropertyModel createProductPropertyModel(CatalogMemoryItem productProperty)
    {
      if (productProperty == null)
        return null;
      return new ProductPropertyModel()
      {
        Id = productProperty.Id,
        ProductCategoryPropertyId = productProperty.CatalogItemId,
        ProductCategoryProperty = createProductCategoryPropertyModel(productCategoryProperty: productProperty.CatalogItem),
        Value = productProperty.Value,
        ExtraKey = productProperty.ExtraKey,
        RowVersion = productProperty.RowVersion
      };
    }
    #endregion
    #region ProductCategoryBrand
    public async Task<IList<ProductCategoryBrandModel>> PrepareProductCategoryBrandListModel(int productCategoryId, CancellationToken cancellationToken)
    {
      var productCategoryBrands = this.productService.GetProductCategoryBrands(productCategoryId: productCategoryId,
                                                               include: new Include<ProductCategoryBrand>(query =>
                                                               {
                                                                 query = query.Include(x => x.Brand);
                                                                 return query;
                                                               }));
      return await this.CreateModelListAsync(productCategoryBrands, this.createProductCategoryBrandModel, cancellationToken);
    }
    public async Task<ProductCategoryBrandModel> PrepareProductCategoryBrandModel(int productCategoryId, int BrandId, CancellationToken cancellationToken)
    {
      var productCategoryBrand = await this.productService.GetProductCategoryBrandById(productCategoryId: productCategoryId,
                                                                       brandId: BrandId,
                                                                       cancellationToken: cancellationToken,
                                                                       include: new Include<ProductCategoryBrand>(query =>
                                                                       {
                                                                         query = query.Include(x => x.Brand);
                                                                         return query;
                                                                       }));
      return this.createProductCategoryBrandModel(productCategoryBrand);
    }
    private ProductCategoryBrandModel createProductCategoryBrandModel(ProductCategoryBrand productCategoryBrand)
    {
      if (productCategoryBrand == null)
        return null;
      return new ProductCategoryBrandModel()
      {
        BrandId = productCategoryBrand.BrandId,
        ProductCategoryId = productCategoryBrand.ProductCategoryId,
        Brand = createBrandModel(productCategoryBrand.Brand),
      };
    }
    private BrandModel createBrandModel(Brand brand)
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
    #region ProductTag
    public async Task<ProductTagModel> PrepareProductTagModel(int productId, int tagId, CancellationToken cancellationToken)
    {
      var productTag = await this.productService.GetProductTagById(productId: productId,
                                                                   tagId: tagId,
                                                                   cancellationToken: cancellationToken);
      return this.createProductTagModel(productTag: productTag);
    }
    public async Task<IList<ProductTagModel>> PrepareProductTagListModel(int productId, CancellationToken cancellationToken)
    {
      var productTags = this.productService.GetProductTags(productId: productId);
      return await this.CreateModelListAsync(source: productTags,
                                             this.createProductTagModel,
                                             cancellationToken: cancellationToken);
    }
    private ProductTagModel createProductTagModel(ThreadActivity productTag)
    {
      if (productTag == null)
        return null;
      return new ProductTagModel()
      {
        CreatedAt = productTag.CreatedAt,
        Id = productTag.Id,
        Text = productTag.Payload,
        UpdateAt = productTag.UpdatedAt
      };
    }
    #endregion
    #region Misc
    public async Task<IList<ProductModel>> PrepareLikedProductListModel(CancellationToken cancellationToken)
    {
      var likedByMeProducts = this.productService.GetUserLikedProducts(include: productModelInclude);
      return await this.CreateModelListAsync(likedByMeProducts, this.createProductModel, cancellationToken);
    }
    public async Task<IList<ProductModel>> PrepareRecentVisitProductListModel(CancellationToken cancellationToken)
    {
      var recentVisitProducts = this.productService.GetUserRecentVisitProducts(include: productModelInclude);
      return await this.CreateModelListAsync(recentVisitProducts, this.createProductModel, cancellationToken);
    }
    public async Task<IList<ProductCommentModel>> PrepareUserCommnetListModel(CancellationToken cancellationToken)
    {
      var userComments = this.productService.GetUserComments();
      var query = await this.CreateModelListAsync(userComments, this.createProductCommentModel, cancellationToken);
      return query;
    }
    #endregion
    #region ProductShippingInfo
    public async Task<ProductShippingInfoModel> PrepareProductShippingInfoModel(int productId, CancellationToken cancellationToken)
    {
      var productShippingInfo = await this.productService.GetProductShippingInfoByProductId(productId: productId,
                                                             cancellationToken: cancellationToken);
      return this.createProductShippingInfoModel(productShippingInfo);
    }
    private ProductShippingInfoModel createProductShippingInfoModel(ProductShippingInfo productShippingInfo)
    {
      if (productShippingInfo == null)
        return null;
      return new ProductShippingInfoModel()
      {
        Length = productShippingInfo.Length,
        Height = productShippingInfo.Height,
        Width = productShippingInfo.Width,
        Weight = productShippingInfo.Weight,
        Rowversion = productShippingInfo.RowVersion
      };
    }
    #endregion
    #region  ProductPrice
    public async Task<IPagedList<ProductPriceModel>> PrepareProductPricePagedListModel(int productId, ProductPriceSearchParameter parameters, CancellationToken cancellationToken)
    {
      var productPrices = productService.GetProductPrices(productId: productId, include: new Include<ProductPrice>(query =>
        {
          query = query.Include(x => x.Product);
          query = query.Include(x => x.Color);
          query = query.Include(x => x.City).ThenInclude(x => x.Province);
          return query;
        }));
      return await this.CreateModelPagedListAsync(source: productPrices,
                                                  convertFunction: this.createProductPriceModel,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  cancellationToken: cancellationToken);
    }
    private ProductPriceModel createProductPriceModel(ProductPrice productPrice)
    {
      if (productPrice == null)
        return null;
      return new ProductPriceModel
      {
        Id = productPrice.Id,
        ProductName = productPrice.Product.Name,
        ProductId = productPrice.Product.Id,
        Price = productPrice.Price,
        MinPrice = productPrice.MinPrice,
        MaxPrice = productPrice.MaxPrice,
        Discount = productPrice.Discount,
        RowVersion = productPrice.RowVersion,
        Color = this.createColorModel(productPrice.Color),
        City = this.createCityModel(productPrice.City),
      };
    }
    #endregion
  }
}