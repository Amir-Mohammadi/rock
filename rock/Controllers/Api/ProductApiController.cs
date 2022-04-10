using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Catalogs;
using rock.Core.Domains.Products;
using rock.Core.Domains.Threads;
using rock.Core.Errors;
using rock.Core.Services.Catalogs;
using rock.Core.Services.Common;
using rock.Core.Services.Products;
using rock.Core.Services.Threads;
using rock.Factories;
using rock.Models;
using rock.Models.ProductApi;
using rock.OAuth;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [Authorize]
  public class ProductApiController : BaseController
  {
    #region Fields
    private readonly IProductService productService;
    private readonly ICatalogService catalogService;
    private readonly IThreadService threadService;
    private readonly ICommonService commonService;
    private readonly IProductFactory factory;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public ProductApiController(IProductService productService,
                                IProductFactory factory,
                                ICatalogService catalogService,
                                ICommonService commonService,
                                IThreadService threadService,
                                IErrorFactory errorFactory)
    {
      this.productService = productService;
      this.catalogService = catalogService;
      this.commonService = commonService;
      this.factory = factory;
      this.threadService = threadService;
      this.errorFactory = errorFactory;
    }
    #endregion
    #region ProductCategory
    [HttpPost("product-categories")]
    public async Task<Key<int>> CreateProductCategory([FromBody] ProductCategoryModel model, CancellationToken cancellationToken)
    {
      var productCatergory = new ProductCategory();
      mapCategory(productCatergory, model);
      ProductCategory parentProductCategory = null;
      if (model.ParentId != null)
        parentProductCategory = await productService.GetProductCategoryById(model.ParentId.Value, cancellationToken);
      var result = await productService.InsertProductCategory(productCategory: productCatergory,
                                                              parentProductCategory: parentProductCategory,
                                                              cancellationToken: cancellationToken);
      return new Key<int>(result.Id);
    }
    [HttpPut("product-categories/{productCategoryId}")]
    public async Task EditProductCategory([FromRoute] int productCategoryId, [FromBody] ProductCategoryModel model, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(productCategoryId, cancellationToken);
      ProductCategory parentProductCategory = null;
      if (model.ParentId != null)
        parentProductCategory = await productService.GetProductCategoryById(model.ParentId.Value, cancellationToken);
      mapCategory(productCategory, model);
      await productService.UpdateProductCategory(productCategory: productCategory, parentProductCategory: parentProductCategory, cancellationToken);
    }
    [HttpGet("product-categories/{productCategoryId}")]
    public async Task<ProductCategoryModel> GetProductCategory([FromRoute] int productCategoryId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCategoryModel(productCategoryId, cancellationToken);
    }
    [HttpGet("product-categories/{productCategoryId}/children")]
    public async Task<IList<ProductCategoryModel>> GetProductCategoryChildren([FromRoute] int productCategoryId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCategoryChildrenModels(productCategoryId, cancellationToken);
    }
    [HttpDelete("product-categories/{productCategoryId}")]
    public async Task DeleteProductCategory([FromRoute] int productCategoryId, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(
        id: productCategoryId,
        cancellationToken: cancellationToken,
        include: new Include<ProductCategory>(query =>
        {
          query = query.Include(x => x.Children)
          .Include(x => x.Catalog)
          .Include(x => x.Catalog.Items)
          .Include(x => x.Catalog.Items).ThenInclude(x => x.Children);
          return query;
        }));
      await productService.DeleteProductCategory(productCategory, cancellationToken);
    }
    [HttpPost("product-categories/{productCategoryId}/archive")]
    public async Task ArchiveProductCategory([FromRoute] int productCategoryId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(id: productCategoryId,
                                                                        cancellationToken: cancellationToken);
      productCategory.RowVersion = rowVersion;
      await productService.ArchiveProductCategory(productCategory: productCategory,
                                                  cancellationToken: cancellationToken);
    }
    [HttpPost("product-categories/{productCategoryId}/restore")]
    public async Task RestoreProductCategory([FromRoute] int productCategoryId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(id: productCategoryId,
                                                                        cancellationToken: cancellationToken);
      productCategory.RowVersion = rowVersion;
      await productService.RestoreProductCategory(productCategory: productCategory,
                                                  cancellationToken: cancellationToken);
    }
    [HttpPost("product-categories/{productCategoryId}/published")]
    public async Task PublishProductCategory([FromRoute] int productCategoryId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(id: productCategoryId,
                                                                        cancellationToken: cancellationToken);
      productCategory.RowVersion = rowVersion;
      await productService.PublishProductCategory(productCategory: productCategory,
                                                  cancellationToken: cancellationToken);
    }
    [HttpPost("product-categories/{productCategoryId}/un-publish")]
    public async Task UnPublishProductCategory([FromRoute] int productCategoryId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(id: productCategoryId,
                                                                        cancellationToken: cancellationToken);
      productCategory.RowVersion = rowVersion;
      await productService.UnPublishProductCategory(productCategory: productCategory,
                                                    cancellationToken: cancellationToken);
    }
    [HttpGet("product-categories")]
    public async Task<IList<ProductCategoryModel>> GetProductCategories([FromQuery] ProductCategorySearchParameters parameters, CancellationToken cancellationToken)
    {
      var result = await factory.PrepareProductCategoryModels(parameters, cancellationToken);
      return result;
    }
    private void mapCategory(ProductCategory productCategory, ProductCategoryModel productCategoryModel)
    {
      productCategory.Name = productCategoryModel.Name;
      productCategory.UrlTitle = productCategoryModel.UrlTitle;
      productCategory.BrowserTitle = productCategoryModel.BrowserTitle;
      productCategory.MetaDescription = productCategoryModel.MetaDescription;
      productCategory.Explanation = productCategoryModel.Explanation;
      productCategory.RowVersion = productCategoryModel.RowVersion;
    }
    #endregion
    #region ProductCategoryProperty
    [HttpPost("product-categories/{productCategoryId}/properties")]
    public async Task<Key<int>> CreateProductCategoryProperty([FromRoute] int productCategoryId, [FromBody] ProductCategoryPropertyModel model, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(id: productCategoryId,
      cancellationToken: cancellationToken,
      include: new Include<ProductCategory>(query =>
         {
           query = query.Include(x => x.Catalog);
           return query;
         })
       );
      var catalogItem = new CatalogItem();
      CatalogItem reference = null;
      if (model.ReferenceId.HasValue)
        reference = await catalogService.GetCatalogItemById(id: model.ReferenceId.Value, catalogId: productCategory.CatalogId, cancellationToken: cancellationToken);
      mapProductCategoryProperty(catalogItem, model);
      var result = await productService.InsertProductCategoryProperty(catalogItem, reference, productCategory, cancellationToken);
      return new Key<int>(result.Id);
    }
    [HttpPut("product-categories/{productCategoryId}/properties/{propertyId}")]
    public async Task EditProductCategoryProperty([FromRoute] int productCategoryId, [FromRoute] int propertyId, [FromBody] ProductCategoryPropertyModel model, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(productCategoryId, cancellationToken);
      var catalogItem = await catalogService.GetCatalogItemById(id: propertyId, catalogId: productCategory.CatalogId, cancellationToken);
      mapProductCategoryProperty(catalogItem, model);
      await productService.UpdateProductCategoryProperty(catalogItem: catalogItem,
                                                         cancellationToken: cancellationToken);
    }
    [HttpDelete("product-categories/{productCategoryId}/properties/{propertyId}")]
    public async Task DeleteProductCategoryProperty([FromRoute] int productCategoryId, [FromRoute] int propertyId, CancellationToken cancellationToken)
    {
      var productCategoryProperty = await productService.GetProductCategoryPropertyById(id: propertyId,
                                                                                        productCategoryId: productCategoryId,
                                                                                        cancellationToken: cancellationToken);
      await productService.DeleteProductCategoryProperty(catalogItem: productCategoryProperty,
                                                         cancellationToken: cancellationToken);
    }
    [HttpGet("product-categories/{productCategoryId}/properties/{propertyId}")]
    public async Task<ProductCategoryPropertyModel> GetProductCategoryProperty([FromRoute] int productCategoryId, [FromRoute] int propertyId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCategoryPropertyModel(productCategoryId, propertyId, cancellationToken);
    }
    [HttpGet("product-categories/{productCategoryId}/properties")]
    public async Task<IList<ProductCategoryPropertyModel>> GetProductCategoryProperties([FromRoute] int productCategoryId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCategoryPropertyListModel(productCategoryId, cancellationToken);
    }
    #endregion
    #region Products
    [HttpPost("products")]
    public async Task<Key<int>> CreateProduct([FromBody] ProductModel model, CancellationToken cancellationToken)
    {
      var product = new Product();
      var category = await productService.GetProductCategoryById(id: model.CategoryId, cancellationToken);
      var brand = await commonService.GetBrandById(model.BrandId, cancellationToken);
      this.mapProduct(product, model);
      var result = await productService.InsertProduct(product, category, brand, cancellationToken);
      return new Key<int>(result.Id);
    }
    [HttpPut("products/{productId}")]
    public async Task EditProduct([FromRoute] int productId, [FromBody] ProductModel model, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(productId, cancellationToken);
      var category = await productService.GetProductCategoryById(model.CategoryId, cancellationToken);
      var brand = await commonService.GetBrandById(model.BrandId, cancellationToken);
      this.mapProduct(product, model);
      await productService.UpdateProduct(product, category, brand, cancellationToken);
    }
    [HttpGet("products/{productId}")]
    public async Task<ProductModel> GetProduct([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductModel(productId, cancellationToken);
    }
    [HttpDelete("products/{productId}")]
    public async Task DeleteProduct([FromRoute] int productId, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        include: new Include<Product>(query =>
                                                        {
                                                          query = query.Include(x => x.ProductPrices)
                                                            .Include(x => x.ProductImages)
                                                            .Include(x => x.ProductColors)
                                                            .Include(x => x.Thread)
                                                            .Include(x => x.CatalogMemory);
                                                          return query;
                                                        }),
                                                        cancellationToken: cancellationToken);
      await productService.DeleteProduct(product: product,
                                         cancellationToken: cancellationToken);
    }
    [HttpPost("product/{productId}/archive")]
    public async Task ArchiveProduct([FromRoute] int productId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                         cancellationToken: cancellationToken);
      product.RowVersion = rowVersion;
      await productService.ArchiveProduct(product: product,
                                          cancellationToken: cancellationToken);
    }
    [HttpPost("product/{productId}/restore")]
    public async Task RestoreProduct([FromRoute] int productId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                         cancellationToken: cancellationToken);
      product.RowVersion = rowVersion;
      await productService.RestoreProduct(product: product,
                                          cancellationToken: cancellationToken);
    }
    [HttpGet("products")]
    public async Task<IPagedList<ProductModel>> GetPagedProducts([FromQuery] ProductSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductPagedListModel(parameters: parameters,
                                                        cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductProperty
    [HttpPost("products/{productId}/properties")]
    public async Task<Key<int>> CreateProductProperty([FromRoute] int productId, [FromBody] ProductPropertyModel model, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        include: new Include<Product>(query =>
                                                        {
                                                          query = query.Include(x => x.CatalogMemory)
                                                                       .Include(x => x.ProductCategory);
                                                          return query;
                                                        }),
                                                        cancellationToken: cancellationToken);
      var catalogItem = await catalogService.GetCatalogItemById(id: model.ProductCategoryPropertyId,
                                                                catalogId: product.ProductCategory.CatalogId,
                                                                cancellationToken: cancellationToken);
      var catalogMemoryItem = new CatalogMemoryItem();
      mapCatalogMemoryItem(catalogMemoryItem, model);
      var result = await productService.InsertProductProperty(catalogMemoryItem, catalogItem, product, cancellationToken);
      return new Key<int>(result.Id);
    }
    private void mapCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, ProductPropertyModel model)
    {
      catalogMemoryItem.ExtraKey = model.ExtraKey;
      catalogMemoryItem.Value = model.Value;
      catalogMemoryItem.RowVersion = model.RowVersion;
    }
    [HttpPut("products/{productId}/properties/{propertyId}")]
    public async Task EditProductProperty([FromRoute] int productId, [FromRoute] int propertyId, [FromBody] ProductPropertyModel model, CancellationToken cancellationToken)
    {
      var catalogMemoryItem = await productService.GetProductPropertyById(productId: productId,
                                                                          propertyId: propertyId,
                                                                          cancellationToken: cancellationToken);
      mapCatalogMemoryItem(catalogMemoryItem: catalogMemoryItem,
                           model: model);
      await productService.UpdateProductProperty(catalogMemoryItem: catalogMemoryItem,
                                                 cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/properties")]
    public async Task DeleteProductProperty([FromRoute] int productId, [FromQuery] int propertyId, CancellationToken cancellationToken)
    {
      var catalogMemoryItem = await productService.GetProductPropertyById(productId: productId,
                                                                          propertyId: propertyId,
                                                                          cancellationToken: cancellationToken);
      await productService.DeleteProductProperty(catalogMemoryItem: catalogMemoryItem,
                                                 cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/properties")]
    public async Task<IList<ProductPropertyModel>> GetProductProperties([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductPropertyListModel(productId: productId,
                                                           cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/main-properties")]
    public async Task<IList<ProductPropertyModel>> GetMainProductProperties([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductPropertyListModel(productId: productId,
                                                           cancellationToken: cancellationToken,
                                                           isMain: true);
    }
    #endregion
    #region ProductImage
    [HttpPost("products/{productId}/images")]
    public async Task<Key<int>> CreateProductImage([FromRoute] int productId, [FromBody] ProductImageModel newProductImage, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(productId, cancellationToken);
      var productImage = new ProductImage();
      productImage.ImageId = newProductImage.ImageId;
      productImage.Order = newProductImage.Order;
      productImage.ImageAlt = newProductImage.ImageAlt;
      productImage.ImageTitle = newProductImage.ImageTitle;
      var result = await productService.InsertProductImage(productImage: productImage,
                                                           product: product,
                                                           cancellationToken: cancellationToken);
      return new Key<int>(result.Id);
    }
    [HttpPatch("products/{productId}/images/{imageId}")]
    public async void EditProductImage([FromRoute] int productId, [FromRoute] int imageId, [FromBody] EditProductImageModel editProductImage, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(productId, cancellationToken);
      var productImage = await productService.GetProductImageById(productId: productId, imageId: imageId, cancellationToken);
      productImage.ImageAlt = editProductImage.ImageAlt;
      productImage.ImageTitle = editProductImage.ImageTitle;
      productImage.RowVersion = editProductImage.RowVersion;
      await productService.UpdateProductImage(productImage: productImage,
                                                            product: product,
                                                            cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/images/{imageId}")]
    public async Task DeleteProductImage([FromRoute] int productId, [FromRoute] int imageId, CancellationToken cancellationToken)
    {
      var productImage = await productService.GetProductImageById(productId: productId, imageId: imageId,
                                                                  cancellationToken: cancellationToken);
      await productService.DeleteProductImage(productImage: productImage,
                                              cancellationToken: cancellationToken);
    }
    [HttpPost("products/{productId}/images/{productImageId}/set-preview")]
    public async Task SetPreviewProductImage([FromRoute] int productId, [FromRoute] int productImageId, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken);
      var productImage = await productService.GetProductImageById(productId: productId, imageId: productImageId,
                                                                  cancellationToken: cancellationToken);
      await productService.SetPreviewProductImage(productImage: productImage,
                                                  product: product,
                                                  cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/images/{imageId}")]
    public async Task<ProductImageModel> GetProductImage([FromRoute] int productId, [FromRoute] int imageId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductImageModel(productId, imageId, cancellationToken);
    }
    [HttpGet("products/{productId}/images")]
    public async Task<IList<ProductImageModel>> GetProductImages([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductImageListModel(productId, cancellationToken);
    }
    #endregion
    #region ProductColor
    [HttpPost("products/{productId}/colors")]
    public async Task CreateProductColor([FromRoute] int productId, [FromBody] ProductColorModel productColor, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken);
      var color = await commonService.GetColorById(id: productColor.ColorId,
                                                   cancellationToken: cancellationToken);
      await productService.NewProductColor(color: color,
                                              product: product,
                                              cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/colors/{colorId}")]
    public async Task DeleteProductColor([FromRoute] int productId, [FromRoute] int colorId, CancellationToken cancellationToken)
    {
      var productColor = await productService.GetProductColorById(productId: productId,
                                                                  colorId: colorId,
                                                                  cancellationToken: cancellationToken);
      await productService.DeleteProductColor(productColor: productColor,
                                              cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/colors/{colorId}")]
    public async Task<ProductColorModel> GetProductColor([FromRoute] int productId, [FromRoute] int colorId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductColorModel(productId, colorId, cancellationToken);
    }
    [HttpGet("products/{productId}/colors")]
    public async Task<IList<ProductColorModel>> GetProductColors([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductColorListModel(productId, cancellationToken);
    }
    [HttpPost("products/{productId}/colors/{colorId}/set-default")]
    public async Task SetDefaultProductColor([FromRoute] int productId, [FromRoute] int colorId, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken);
      var productColor = await productService.GetProductColorById(productId: productId,
                                                                  colorId: colorId,
                                                                  cancellationToken: cancellationToken);
      await productService.SetDefaultProductColor(productColor: productColor,
                                                  product: product,
                                                  cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductRating
    [HttpPost("products/{productId}/ratings/start")]
    public async Task<Key<int>> StartProductRating([FromRoute] int productId, [FromBody] StartProductRatingModel newRating, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken,
                                                        include: new Include<Product>(query =>
                                                        {
                                                          query = query.Include(x => x.Thread);
                                                          return query;
                                                        }));
      var rating = new ThreadActivity();
      var ratingConditions = new List<ThreadActivity>();
      rating.Payload = newRating.Title;
      foreach (var condition in newRating.Conditions)
      {
        var activity = new ThreadActivity();
        activity.Payload = condition;
        ratingConditions.Add(activity);
      }
      await productService.InsertProductRating(rating: rating,
                                               conditions: ratingConditions,
                                               product: product,
                                               cancellationToken: cancellationToken);
      return new Key<int>(product.Id);
    }
    [HttpDelete("products/{productId}/ratings/{ratingId}")]
    public async Task DeleteProductRating([FromRoute] int productId, [FromRoute] int ratingId, CancellationToken cancellationToken)
    {
      var rating = await productService.GetProductRating(productId: productId,
                                                         ratingId: ratingId,
                                                         cancellationToken: cancellationToken);
      await productService.DeleteProductRating(rating: rating,
                                               cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/ratings/{ratingId}")]
    public async Task<ProductRatingModel> GetProductRating([FromRoute] int productId, [FromRoute] int ratingId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductRatingModel(productId, ratingId, cancellationToken);
    }
    [HttpGet("products/{productId}/ratings")]
    public async Task<IList<ProductRatingModel>> GetProductRatings([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductRatingListModel(productId, cancellationToken);
    }
    #endregion
    #region ProductQuestion
    [HttpPost("products/{productId}/questions/ask")]
    public async Task<Key<int>> AskProductQuestion([FromRoute] int productId, [FromBody] AskProductQuestionModel newQuestion, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken,
                                                        include: new Include<Product>(query =>
                                                        {
                                                          query = query.Include(x => x.Thread);
                                                          return query;
                                                        }));
      var question = new ThreadActivity();
      question.Payload = newQuestion.Question;
      await productService.AskProductQuestion(question: question, product: product, cancellationToken: cancellationToken);
      return new Key<int>(question.Id);
    }
    [HttpDelete("products/{productId}/questions/{questionId}")]
    public async Task DeleteProductQuestion([FromRoute] int productId, [FromRoute] int questionId, CancellationToken cancellationToken)
    {
      var question = await productService.GetProductQuestionById(productId: productId,
                                                                   questionId: questionId,
                                                                   cancellationToken: cancellationToken);
      await productService.DeleteProductQuestion(question: question,
                                                 cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/questions")]
    public async Task<IList<ProductQuestionModel>> GetProductQuestions([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductQuestionListModel(productId, cancellationToken);
    }
    #endregion
    #region ProductQuestionAnswer
    [HttpPost("products/{productId}/questions/{questionId}/answer")]
    public async Task AnswerProductQuestion([FromRoute] int productId, [FromRoute] int questionId, [FromBody] AnswerProductQuestionModel newAnswer, CancellationToken cancellationToken)
    {
      var question = await threadService.GetThreadActivityById(id: questionId,
                                                               cancellationToken: cancellationToken,
                                                               include: new Include<ThreadActivity>(query =>
                                                               {
                                                                 query.Include(x => x.Thread);
                                                                 return query;
                                                               }));
      var answer = new ThreadActivity();
      answer.Payload = newAnswer.Answer;
      await productService.AnswerProductQuestion(answer: answer,
                                                 question: question,
                                                 cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/questions/{questionId}/answers/{answerId}")]
    public async Task DeleteProductQuestionAnswer([FromRoute] int productId, [FromRoute] int questionId, [FromBody] int answerId, CancellationToken cancellationToken)
    {
      var answer = await productService.GetProductAnswer(productId: productId,
                                                        questionId: questionId,
                                                        answerId: answerId,
                                                        cancellationToken: cancellationToken);
      await productService.DeleteProductAnswer(answer: answer, cancellationToken);
    }
    [HttpGet("products/{productId}/questions/{questionId}/answers")]
    public async Task<IPagedList<ProductAnswerModel>> GetPagedProductQuestionAnswers([FromRoute] int productId, [FromRoute] int questionId, [FromQuery] ProductAnswerSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductAnswerPagedListModel(productId: productId,
                                                              questionId: questionId,
                                                              parameters: parameters,
                                                              cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductComments
    [HttpPost("products/{productId}/comments/write")]
    public async Task<Key<int>> WriteProductComment([FromRoute] int productId, [FromBody] WriteCommentModel newComment, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(productId, cancellationToken);
      var comment = new ThreadActivity();
      comment.Payload = newComment.Text;
      await productService.InsertProductComment(comment: comment, product: product, cancellationToken);
      return new Key<int>(comment.Id);
    }
    [HttpPatch("products/{productId}/comments/{commentId}")]
    public async Task EditProductComment([FromRoute] int productId, [FromRoute] int commentId, [FromBody] WriteCommentModel updatedComment, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: productId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken);
      comment.Payload = updatedComment.Text;
      await productService.UpdateProductComment(comment: comment,
                                                cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/comments/{commentId}")]
    public async Task DeleteProductComment([FromRoute] int productId, [FromRoute] int commentId, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: productId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken);
      await productService.DeleteProductComment(comment: comment,
                                                cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/comments/{commentId}")]
    public async Task<ProductCommentModel> GetProductComment([FromRoute] int productId, [FromRoute] int commentId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCommentModel(productId: productId,
                                                      commentId: commentId,
                                                      cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/comments")]
    public async Task<IPagedList<ProductCommentModel>> GetPagedProductComments([FromRoute] int productId, [FromQuery] ProductCommentSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCommentPagedListModel(productId: productId,
                                                               parameters: parameters,
                                                               cancellationToken: cancellationToken);
    }
    #endregion    
    #region ProductCommentReply
    [HttpPost("products/{productId}/comments/{commentId}/reply")]
    public async Task<Key<int>> CreateProductCommentReply([FromRoute] int productId, [FromRoute] int commentId, [FromBody] WriteCommentModel newReply, CancellationToken cancellationToken)
    {
      var comment = await productService.GetProductCommentById(productId: productId,
                                                               commentId: commentId,
                                                               cancellationToken: cancellationToken,
                                                               include: new Include<ThreadActivity>(query =>
                                                               {
                                                                 query = query.Include(x => x.Thread);
                                                                 return query;
                                                               }));
      var reply = new ThreadActivity();
      reply.Payload = newReply.Text;
      await productService.InsertProductCommentReply(reply: reply, comment: comment, cancellationToken);
      return new Key<int>(reply.Id);
    }
    [HttpPatch("products/{productId}/comments/{commentId}/replies/{replyId}")]
    public async Task EditProductCommentReply([FromRoute] int productId, [FromRoute] int commentId, [FromRoute] int replyId, [FromBody] WriteCommentModel updatedReply, CancellationToken cancellationToken)
    {
      var reply = await productService.GetProductCommentReplyById(productId: productId,
                                                                  commentId: commentId,
                                                                  replyId: replyId,
                                                                  cancellationToken: cancellationToken);
      reply.Payload = updatedReply.Text;
      await productService.UpdateProductCommentReply(reply: reply,
                                                     cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/comments/{commentId}/replies/{replyId}")]
    public async Task DeleteProductCommentReply([FromRoute] int productId, [FromRoute] int commentId, [FromRoute] int replyId, CancellationToken cancellationToken)
    {
      var reply = await productService.GetProductCommentReplyById(productId: productId,
                                                                  commentId: commentId,
                                                                  replyId: replyId,
                                                                  cancellationToken: cancellationToken);
      await productService.DeleteProductCommentReply(reply: reply, cancellationToken);
    }
    [HttpGet("products/{productId}/comments/{commentId}/replies/{replyId}")]
    public async Task<ProductCommentReplyModel> GetProductCommentReply([FromRoute] int productId, [FromRoute] int commentId, [FromRoute] int replyId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCommentReplyModel(productId: productId,
                                                           commentId: commentId,
                                                           replyId: replyId,
                                                           cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/comments/{commentId}/replies")]
    public async Task<IPagedList<ProductCommentReplyModel>> GetPagedProductCommentReplies([FromRoute] int productId, [FromRoute] int commentId, [FromQuery] ProductCommentReplySearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCommentReplyPagedListModel(productId: productId,
                                                                    commentId: commentId,
                                                                    parameters: parameters,
                                                                    cancellationToken: cancellationToken);
    }
    #endregion    
    #region ProductBrochure
    [HttpPost("products/{productId}/brochure")]
    public async Task<Key<int>> CreateProductBrochure([FromRoute] int productId, [FromBody] ProductBrochureModel newProductBrochure, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken);
      var brochure = new ProductBrochure();
      brochure.HTML = newProductBrochure.HTML;
      await productService.InsertProductBrochure(productBrochure: brochure,
                                                 product: product,
                                                 cancellationToken: cancellationToken);
      return new Key<int>(brochure.Id);
    }
    [HttpPut("products/{productId}/brochure")]
    public async Task EditProductBrochure([FromRoute] int productId, [FromBody] ProductBrochureModel updatedProductBrochure, CancellationToken cancellationToken)
    {
      var brochure = await productService.GetProductBrochureByProductId(productId: productId,
                                                                        cancellationToken: cancellationToken);
      brochure.HTML = updatedProductBrochure.HTML;
      await productService.UpdateProductBrochure(brochure, cancellationToken);
    }
    [HttpGet("products/{productId}/brochure")]
    public async Task<ProductBrochureModel> GetProductBrochure([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await this.factory.PrepareProductBrochureModel(productId: productId,
                                                            cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/brochure")]
    public async Task DeleteProductBrochure([FromRoute] int productId, CancellationToken cancellationToken)
    {
      var productBrochure = await productService.GetProductBrochureByProductId(productId, cancellationToken);
      await productService.DeleteProductBrochure(productBrochure, cancellationToken);
    }
    #endregion
    #region ProductTags
    [HttpPost("products/{productId}/tags")]
    public async Task<Key<int>> CreateProductTag([FromRoute] int productId, [FromBody] WriteProductTagModel newTag, CancellationToken cancellationToken)
    {
      var product = await productService.GetProductById(id: productId,
                                                        cancellationToken: cancellationToken,
                                                        include: new Include<Product>(query =>
                                                        {
                                                          query = query.Include(x => x.Thread);
                                                          return query;
                                                        }));
      var tag = new ThreadActivity();
      tag.Payload = newTag.Text;
      await productService.InsertProductTag(tag: tag, product: product, cancellationToken);
      return new Key<int>(tag.Id);
    }
    [HttpPatch("products/{productId}/tags/{tagId}")]
    public async Task EditProductTag([FromRoute] int productId, [FromRoute] int tagId, [FromBody] WriteProductTagModel updatedTag, CancellationToken cancellationToken)
    {
      var tag = await productService.GetProductTagById(productId: productId,
                                                       tagId: tagId,
                                                       cancellationToken: cancellationToken);
      tag.Payload = updatedTag.Text;
      await productService.UpdateProductTag(tag: tag,
                                            cancellationToken: cancellationToken);
    }
    [HttpDelete("products/{productId}/tags/{tagId}")]
    public async Task DeleteProductTag([FromRoute] int productId, [FromRoute] int tagId, CancellationToken cancellationToken)
    {
      var tag = await productService.GetProductTagById(productId: productId,
                                                       tagId: tagId,
                                                       cancellationToken: cancellationToken);
      await productService.DeleteProductTag(tag: tag,
                                            cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/tags/{tagId}")]
    public async Task<ProductTagModel> GetProductTag([FromRoute] int productId, [FromRoute] int tagId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductTagModel(productId: productId,
                                                  tagId: tagId,
                                                  cancellationToken: cancellationToken);
    }
    [HttpGet("products/{productId}/tags")]
    public async Task<IList<ProductTagModel>> GetPagedProductTags([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductTagListModel(productId: productId,
                                                      cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductCategoryBrand
    [HttpPost("product-categories/{productCategoryId}/brands")]
    public async Task CreateProductCategoryBrand([FromRoute] int productCategoryId, [FromBody] ProductCategoryBrandModel productCategoryBrand, CancellationToken cancellationToken)
    {
      var productCategory = await productService.GetProductCategoryById(id: productCategoryId,
                                                                        cancellationToken: cancellationToken);
      var brand = await commonService.GetBrandById(id: productCategoryBrand.BrandId,
                                                   cancellationToken: cancellationToken);
      await productService.NewProductCategoryBrand(brand: brand,
                                                   productCategory: productCategory,
                                                   cancellationToken: cancellationToken);
    }
    [HttpDelete("product-categories/{productCategoryId}/brands/{brandId}")]
    public async Task DeleteProductCategoryBrand([FromRoute] int productCategoryId, [FromRoute] int brandId, CancellationToken cancellationToken)
    {
      var productCategoryBrand = await productService.GetProductCategoryBrandById(productCategoryId: productCategoryId,
                                                                                  brandId: brandId,
                                                                                  cancellationToken: cancellationToken);
      await productService.DeleteProductCategoryBrand(productCategoryBrand: productCategoryBrand,
                                                      cancellationToken: cancellationToken);
    }
    [HttpGet("product-categories/{productCategoryId}/brands/{brandId}")]
    public async Task<ProductCategoryBrandModel> GetProductCategoryBrand([FromRoute] int productCategoryId, [FromRoute] int brandId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCategoryBrandModel(productCategoryId, brandId, cancellationToken);
    }
    [HttpGet("product-categories/{productCategoryId}/brands")]
    public async Task<IList<ProductCategoryBrandModel>> GetProductCategoryBrands([FromRoute] int productCategoryId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductCategoryBrandListModel(productCategoryId, cancellationToken);
    }
    #endregion
    #region  ProductShippingInfo
    [HttpPost("products/{productId}/product-shipping-info")]
    public async Task CreateProductShippingInfo([FromBody] ProductShippingInfoModel model, [FromRoute] int productId, CancellationToken cancellationToken)
    {
      var productShippingInfo = new ProductShippingInfo();
      this.mapProductShippingInfo(productShippingInfo, model, productId);
      await productService.InsertProductShippingInfo(productShippingInfo: productShippingInfo, cancellationToken);
    }
    [HttpPut("products/{productId}/product-shipping-info")]
    public async Task UpdateProductShippingInfo([FromBody] ProductShippingInfoModel model, [FromRoute] int productId, CancellationToken cancellationToken)
    {
      var productShippingInfo = new ProductShippingInfo();
      this.mapProductShippingInfo(productShippingInfo, model, productId);
      await productService.UpdateProductShippingInfo(productShippingInfo: productShippingInfo, cancellationToken);
    }
    [HttpGet("products/{productId}/product-shipping-info")]
    public async Task<ProductShippingInfoModel> GetProductShippingInfo([FromRoute] int productId, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductShippingInfoModel(productId: productId, cancellationToken: cancellationToken);
    }
    #endregion
    #region  ProductPrice
    [HttpGet("products/{productId}/prices")]
    public async Task<IPagedList<ProductPriceModel>> GetProductPrices([FromRoute] int productId, [FromQuery] ProductPriceSearchParameter parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareProductPricePagedListModel(productId: productId, parameters: parameters, cancellationToken: cancellationToken);
    }
    [HttpPost("products/{productId}/prices")]
    public async Task CreateProductPrice([FromRoute] int productId, [FromBody] CreateProductPriceModel model, CancellationToken cancellationToken)
    {
      var productPrice = this.createProductPriceModel(model: model, productId: productId);
      await productService.CreateProductPrice(productPrice: productPrice, cancellationToken: cancellationToken);
    }
    [HttpPost("products/{productId}/prices/{productPriceId}/publish")]
    public async Task ProductPricePublish([FromRoute] int productId, [FromRoute] int productPriceId, CancellationToken cancellationToken)
    {
      var productPrice = await productService.GetProductPriceById(id: productPriceId, cancellationToken: cancellationToken);
      if (productPrice == null || productPrice.ProductId != productId)
        throw errorFactory.ResourceNotFound(id: productPriceId);
      await productService.ProductPricePublish(productPrice: productPrice, cancellationToken: cancellationToken);
    }
    [HttpPost("products/{productId}/prices/{productPriceId}/unpublish")]
    public async Task ProductPriceUnPublish([FromRoute] int productId, [FromRoute] int productPriceId, CancellationToken cancellationToken)
    {
      var productPrice = await productService.GetProductPriceById(id: productPriceId, cancellationToken: cancellationToken);
      if (productPrice == null || productPrice.ProductId != productId)
        throw errorFactory.ResourceNotFound(id: productPriceId);
      await productService.ProductPricePublish(productPrice: productPrice, cancellationToken: cancellationToken);
    }
    #endregion
    #region Private
    private ProductPrice createProductPriceModel(CreateProductPriceModel model, int productId)
    {
      return new ProductPrice
      {
        CityId = model.CityId,
        ProductId = productId,
        ColorId = model.ColorId,
        Price = model.Price,
        MinPrice = model.MinPrice,
        MaxPrice = model.MaxPrice,
        Discount = model.Discount,
        IsPublished = model.IsPublished
      };
    }
    private void mapProductCategoryProperty(CatalogItem catalogItem, ProductCategoryPropertyModel model)
    {
      catalogItem.Id = model.Id;
      catalogItem.Order = model.Order;
      catalogItem.Value = model.Title;
      catalogItem.Type = model.Type;
      catalogItem.ShowInFilter = model.ShowInFilter;
      catalogItem.HasMultiple = model.HasMultiple;
      catalogItem.IsMain = model.IsMain;
      catalogItem.RowVersion = model.RowVersion;
    }
    private void mapProduct(Product product, ProductModel model)
    {
      product.Name = model.Name;
      product.BriefDescription = model.BriefDescription;
      product.UrlTitle = model.UrlTitle;
      product.BrowserTitle = model.BrowserTitle;
      product.MetaDescription = model.MetaDescription;
    }
    private void mapProductShippingInfo(ProductShippingInfo productShippingInfo, ProductShippingInfoModel model, int productId)
    {
      productShippingInfo.ProductId = productId;
      productShippingInfo.Length = model.Length;
      productShippingInfo.Height = model.Height;
      productShippingInfo.Weight = model.Weight;
      productShippingInfo.Width = model.Width;
      productShippingInfo.RowVersion = model.Rowversion;
    }
    #endregion
  }
}