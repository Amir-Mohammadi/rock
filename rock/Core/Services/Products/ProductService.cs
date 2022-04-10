using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Catalogs;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Files;
using rock.Core.Domains.Products;
using rock.Core.Domains.Threads;
using rock.Core.Services.Catalogs;
using rock.Core.Services.Files;
using rock.Core.Services.Threads;
using rock.Core.Services.Common;
using System.Linq.Dynamic.Core;
namespace rock.Core.Services.Products
{
  public class ProductService : IProductService
  {
    #region Fields 
    private readonly IWorkContext workContext;
    private readonly IFileService fileService;
    private readonly ICatalogService catalogService;
    private readonly IThreadService threadService;
    private readonly IRepository<Product> productRepository;
    private readonly IRepository<ProductShippingInfo> productShippingInfoRepository;
    private readonly IRepository<ProductBrochure> productBrochureRepository;
    private readonly IRepository<ProductBrochureAttachment> productBrochureAttachmentRepository;
    private readonly IRepository<ProductImage> productImageRepository;
    private readonly IRepository<ProductColor> productColorRepository;
    private readonly IRepository<ProductCategory> productCategoryRepository;
    private readonly IRepository<ProductCategoryBrand> productCategoryBrandRepository;
    private readonly IRepository<ProductPrice> productPriceRepository;
    #endregion
    #region Constractor
    public ProductService(IWorkContext workContext,
                          IFileService fileService,
                          ICatalogService catalogService,
                          IThreadService threadService,
                          IRepository<Product> productRepository,
                          IRepository<ProductBrochure> productBrochureRepository,
                          IRepository<ProductBrochureAttachment> productBrochureAttachmentRepository,
                          IRepository<ProductImage> productImageRepository,
                          IRepository<ProductColor> productColorRepository,
                          IRepository<ProductCategory> productCategoryRepository,
                          IRepository<ProductCategoryBrand> productCategoryBrandRepository,
                          IRepository<ProductPrice> productPriceRepository,
                          IRepository<ProductShippingInfo> productShippingInfoRepository)
    {
      this.workContext = workContext;
      this.fileService = fileService;
      this.productRepository = productRepository;
      this.catalogService = catalogService;
      this.threadService = threadService;
      this.productBrochureRepository = productBrochureRepository;
      this.productBrochureAttachmentRepository = productBrochureAttachmentRepository;
      this.productImageRepository = productImageRepository;
      this.productColorRepository = productColorRepository;
      this.productCategoryRepository = productCategoryRepository;
      this.productCategoryBrandRepository = productCategoryBrandRepository;
      this.productPriceRepository = productPriceRepository;
      this.productShippingInfoRepository = productShippingInfoRepository;
    }
    #endregion
    #region ProductCategory
    public async Task<ProductCategory> InsertProductCategory(ProductCategory productCategory, ProductCategory parentProductCategory, CancellationToken cancellationToken)
    {
      var catalog = await catalogService.NewCatalog(cancellationToken);
      productCategory.Parent = parentProductCategory;
      productCategory.CreatedAt = catalog.CreatedAt;
      productCategory.Catalog = catalog;
      productCategory.IsPublished = false;
      await productCategoryRepository.AddAsync(productCategory, cancellationToken);
      return productCategory;
    }
    public async Task ArchiveProductCategory(ProductCategory productCategory, CancellationToken cancellationToken)
    {
      productCategory.DeletedAt = DateTime.UtcNow;
      await productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
    }
    public async Task RestoreProductCategory(ProductCategory productCategory, CancellationToken cancellationToken)
    {
      productCategory.DeletedAt = null;
      await productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
    }
    public async Task PublishProductCategory(ProductCategory productCategory, CancellationToken cancellationToken)
    {
      productCategory.IsPublished = true;
      await productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
    }
    public async Task UnPublishProductCategory(ProductCategory productCategory, CancellationToken cancellationToken)
    {
      productCategory.IsPublished = false;
      await productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
    }
    public async Task DeleteProductCategory(ProductCategory productCategory, CancellationToken cancellationToken)
    {
      await productCategoryRepository.DeleteAsync(entity: productCategory, cancellationToken: cancellationToken);
      await catalogService.DeleteCatalog(productCategory.Catalog, cancellationToken);
    }
    public async Task UpdateProductCategory(ProductCategory productCategory, ProductCategory parentProductCategory, CancellationToken cancellationToken)
    {
      productCategory.Parent = parentProductCategory;
      productCategory.UpdatedAt = DateTime.UtcNow;
      await productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
    }
    public IQueryable<ProductCategory> GetProductCategories(int? brandId = null, string name = null, bool showArchive = true, bool? isPublished = null, IInclude<ProductCategory> include = null)
    {
      var query = productCategoryRepository.GetQuery(include: include);
      if (!string.IsNullOrEmpty(name))
        query = query.Where(x => x.Name == name);
      if (isPublished != null)
        query = query.Where(x => x.IsPublished == isPublished);
      if (showArchive == false)
        query = query.Where(x => x.DeletedAt == null);
      if (brandId != null)
      {
        var products = GetProducts().Where(x => x.BrandId == brandId);
        var categoryIds = products.Select(x => x.ProductCategoryId).Distinct().ToList();
        query = query.Where(x => categoryIds.Contains(x.Id));
      }
      return query;
    }
    public IQueryable<ProductCategory> GetProductCategoryChildrenByParentId(int categoryId, bool showArchive = true, bool? isPublished = true, IInclude<ProductCategory> include = null)
    {
      var query = GetProductCategories(name: null,
                                       showArchive: showArchive,
                                       isPublished: isPublished,
                                       include: include);
      query = query.Where(x => x.ParentId == categoryId);
      return query;
    }
    public async Task<ProductCategory> GetProductCategoryById(int id, CancellationToken cancellationToken, IInclude<ProductCategory> include = null)
    {
      return await productCategoryRepository.GetAsync(predicate: x => x.Id == id,
                                                      cancellationToken: cancellationToken,
                                                      include: include);
    }
    #endregion
    #region ProductCategoryProperty
    public async Task<CatalogItem> InsertProductCategoryProperty(CatalogItem catalogItem, CatalogItem reference, ProductCategory category, CancellationToken cancellationToken)
    {
      catalogItem.Reference = reference;
      catalogItem.Catalog = category.Catalog;
      return await catalogService.InsertCatalogItem(catalogItem, cancellationToken);
    }
    public async Task DeleteProductCategoryProperty(CatalogItem productCategoryProperty, CancellationToken cancellationToken)
    {
      await this.catalogService.DeleteCatalogItem(catalogItem: productCategoryProperty, cancellationToken);
    }
    public async Task<CatalogItem> GetProductCategoryPropertyById(int id, int productCategoryId, CancellationToken cancellationToken, IInclude<CatalogItem> include = null)
    {
      var productCategory = await GetProductCategoryById(id: productCategoryId,
                                                         cancellationToken: cancellationToken);
      return await catalogService.GetCatalogItemById(id: id,
                                                     catalogId: productCategory.CatalogId,
                                                     cancellationToken: cancellationToken,
                                                     include: include);
    }
    public async Task UpdateProductCategoryProperty(CatalogItem catalogItem, CancellationToken cancellationToken)
    {
      await catalogService.UpdateCatalogItem(catalogItem: catalogItem,
                                             cancellationToken: cancellationToken);
    }
    public IQueryable<CatalogItem> GetProductCategoryProperties(int productCategoryId, IInclude<CatalogItem> include = null)
    {
      var productCategory = productCategoryRepository.Get(x => x.Id == productCategoryId);
      return catalogService.GetCatalogItemsByCatalogId(productCategory.CatalogId, include);
    }
    #endregion
    #region Product
    public async Task<Product> GetProductById(int id, CancellationToken cancellationToken, IInclude<Product> include = null)
    {
      var product = await productRepository.GetAsync(predicate: x => x.Id == id,
                                                     cancellationToken: cancellationToken,
                                                     include: include);
      return product;
    }
    public bool CheckProductExistencById(int id)
    {
      return productRepository.GetQuery().Where(x => x.Id == id).Any();
    }
    public async Task VisitProductById(int id, CancellationToken cancellationToken)
    {
      if (workContext.IsAuthenticated())
      {
        var product = await productRepository.GetAsync(predicate: x => x.Id == id,
                                                       cancellationToken: cancellationToken,
                                                       include: new Include<Product>(query =>
                                                       {
                                                         query = query.Include(x => x.Thread);
                                                         return query;
                                                       }));
        await VisitProduct(product: product,
                           cancellationToken: cancellationToken);
      }
    }
    public async Task<Product> InsertProduct(Product product, ProductCategory productCategory, Brand brand, CancellationToken cancellationToken)
    {
      product.Brand = brand;
      product.ProductCategory = productCategory;
      var thread = await threadService.newThread(cancellationToken: cancellationToken);
      product.Thread = thread;
      var catalogMemory = new CatalogMemory();
      catalogMemory.CatalogId = productCategory.CatalogId;
      await catalogService.InsertCatalogMemory(catalogMemory: catalogMemory,
                                               cancellationToken: cancellationToken);
      product.CatalogMemory = catalogMemory;
      await productRepository.AddAsync(entity: product,
                                       cancellationToken);
      return product;
    }
    public IQueryable<Product> GetProducts(string name = null, int? threadId = null, int? brandId = null, int? productCategoryId = null, bool showArchive = true, IInclude<Product> include = null)
    {
      var query = productRepository.GetQuery(include);
      if (!string.IsNullOrEmpty(name))
        query = query.Where(x => x.Name.Contains(name));
      if (showArchive == false)
        query = query.Where(x => x.DeletedAt == null);
      if (brandId != null)
        query = query.Where(x => x.BrandId == brandId);
      if (productCategoryId != null)
        query = query.Where(x => x.ProductCategoryId == productCategoryId);
      if (threadId != null)
        query = query.Where(x => x.ThreadId == threadId);
      return query;
    }
    public async Task DeleteProduct(Product product, CancellationToken cancellationToken)
    {
      product.PreviewProductImageId = null;
      await productRepository.UpdateAsync(entity: product,
                                          cancellationToken: cancellationToken);
      foreach (var productImage in product.ProductImages)
      {
        await DeleteProductImage(productImage, cancellationToken);
      }
      foreach (var productColor in product.ProductColors)
      {
        await DeleteProductColor(productColor, cancellationToken);
      }
      await productRepository.DeleteAsync(product, cancellationToken);
      await threadService.DeleteThread(thread: product.Thread,
                                       cancellationToken: cancellationToken);
      await catalogService.DeleteCatalogMemory(catalogMemory: product.CatalogMemory,
                                               cancellationToken: cancellationToken);
    }
    public async Task ArchiveProduct(Product product, CancellationToken cancellationToken)
    {
      product.DeletedAt = DateTime.UtcNow;
      await productRepository.UpdateAsync(entity: product,
                                          cancellationToken: cancellationToken);
    }
    public async Task RestoreProduct(Product product, CancellationToken cancellationToken)
    {
      product.DeletedAt = null;
      await productRepository.UpdateAsync(entity: product,
                                          cancellationToken: cancellationToken);
    }
    public async Task UpdateProduct(Product product, ProductCategory category, Brand brand, CancellationToken cancellationToken)
    {
      product.ProductCategory = category;
      product.Brand = brand;
      await productRepository.UpdateAsync(product, cancellationToken);
    }
    public IQueryable<Product> GetUserLikedProducts(int? productId = null, IInclude<Product> include = null)
    {
      var query = productRepository.GetQuery(include: include);
      if (productId != null)
        query = query.Where(x => x.Id == productId);
      var userLikeThreadQuery = threadService.GetUserThreadActivities()
                                            .Where(x => x.Type == ThreadActivityType.Like)
                                            .Select(x => new { ThreadId = x.ThreadId })
                                            .Distinct();
      query = from product in query
              join likeActivity in userLikeThreadQuery on product.ThreadId equals likeActivity.ThreadId
              select product;
      return query;
    }
    public IQueryable<Product> GetUserRecentVisitProducts(IInclude<Product> include = null)
    {
      var query = productRepository.GetQuery(include: include);
      var userVisitThreadQuery = threadService.GetUserThreadActivities()
                                            .Where(x => x.Type == ThreadActivityType.Visit)
                                            .Select(x => new { ThreadId = x.ThreadId })
                                            .Distinct();
      query = from product in query
              join visitActivity in userVisitThreadQuery on product.ThreadId equals visitActivity.ThreadId
              select product;
      return query;
    }
    #endregion
    #region ProductProperty
    public async Task<CatalogMemoryItem> InsertProductProperty(CatalogMemoryItem catalogMemoryItem, CatalogItem catalogItem, Product product, CancellationToken cancellationToken)
    {
      catalogMemoryItem.CatalogItem = catalogItem;
      catalogMemoryItem.CatalogMemory = product.CatalogMemory;
      await catalogService.InsertCatalogMemoryItem(catalogMemoryItem: catalogMemoryItem,
                                                   cancellationToken: cancellationToken);
      return catalogMemoryItem;
    }
    public async Task UpdateProductProperty(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken)
    {
      await catalogService.UpdateCatalogMemoryItem(catalogMemoryItem: catalogMemoryItem,
                                                   cancellationToken: cancellationToken);
    }
    public async Task DeleteProductProperty(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken)
    {
      await catalogService.DeleteCatalogMemoryItem(catalogMemoryItem: catalogMemoryItem,
                                                   cancellationToken: cancellationToken);
    }
    public IQueryable<CatalogMemoryItem> GetProductProperties(int? productId = null, bool? isMain = null, IInclude<CatalogMemoryItem> include = null)
    {
      var query = getProductCatalogMemoryItems(productId: productId, include: include);
      if (isMain != null)
        query = query.Where(x => x.CatalogItem.IsMain == isMain);
      return query;
    }
    public async Task<CatalogMemoryItem> GetProductPropertyById(int productId, int propertyId, CancellationToken cancellationToken, IInclude<CatalogMemoryItem> include = null)
    {
      var query = getProductCatalogMemoryItems(productId: productId,
                                               include: include);
      query = query.Where(x => x.Id == propertyId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    private IQueryable<CatalogMemoryItem> getProductCatalogMemoryItems(int? productId = null, IInclude<CatalogMemoryItem> include = null)
    {
      var products = GetProducts();
      if (productId != null)
        products = products.Where(x => x.Id == productId);
      var query = from p in products
                  from ta in p.CatalogMemory.Items
                  select ta;
      if (include != null)
        query = include.Execute(query);
      return query;
    }
    #endregion
    #region ProductImage
    public async Task<ProductImage> InsertProductImage(ProductImage productImage, Product product, CancellationToken cancellationToken)
    {
      var image = new File();
      image.Id = productImage.ImageId;
      image.Access = FileAccessType.Public;
      image.OwnerGroup = Domains.Users.UserRole.None;
      image = await fileService.InsertFile(file: image,
                                           cancellationToken: cancellationToken);
      productImage.Image = image;
      productImage.Product = product;
      await productImageRepository.AddAsync(entity: productImage,
                                                  cancellationToken: cancellationToken);
      return productImage;
    }
    public async Task SetPreviewProductImage(ProductImage productImage, Product product, CancellationToken cancellationToken)
    {
      product.PreviewProductImage = productImage;
      await productRepository.UpdateAsync(entity: product,
                                          cancellationToken: cancellationToken);
    }
    public async Task DeleteProductImage(ProductImage productImage, CancellationToken cancellationToken)
    {
      await productImageRepository.DeleteAsync(productImage, cancellationToken);
      var file = await fileService.GetFileById(id: productImage.ImageId,
                                               cancellationToken: cancellationToken);
      await fileService.DeleteFile(file: file,
                                   cancellationToken: cancellationToken);
    }
    public async Task<ProductImage> GetProductImageById(int productId, int imageId, CancellationToken cancellationToken, IInclude<ProductImage> include = null)
    {
      return await productImageRepository.GetAsync(predicate: x => x.Id == imageId && x.ProductId == productId,
                                                   include: include,
                                                   cancellationToken: cancellationToken);
    }
    public IQueryable<ProductImage> GetProductImages(int productId, IInclude<ProductImage> include = null)
    {
      return productImageRepository.GetQuery(include: include)
        .Where(x => x.ProductId == productId)
        .OrderBy(x => x.Order);
    }
    #endregion
    #region ProductColor
    public async Task<ProductColor> NewProductColor(Color color, Product product, CancellationToken cancellationToken)
    {
      var productColor = new ProductColor();
      productColor.Product = product;
      productColor.Color = color;
      await productColorRepository.AddAsync(entity: productColor,
                                            cancellationToken: cancellationToken);
      return productColor;
    }
    public async Task DeleteProductColor(ProductColor productColor, CancellationToken cancellationToken)
    {
      await productColorRepository.DeleteAsync(entity: productColor,
                                               cancellationToken: cancellationToken);
    }
    public async Task<ProductColor> GetProductColorById(int productId, int colorId, CancellationToken cancellationToken, IInclude<ProductColor> include = null)
    {
      return await productColorRepository.GetAsync(predicate: x => x.ColorId == colorId && x.ProductId == productId,
                                                   cancellationToken: cancellationToken,
                                                   include: include);
    }
    public IQueryable<ProductColor> GetProductColors(int productId, IInclude<ProductColor> include = null)
    {
      return productColorRepository.GetQuery(include: include).Where(x => x.ProductId == productId);
    }
    public async Task SetDefaultProductColor(ProductColor productColor, Product product, CancellationToken cancellationToken)
    {
      product.DefaultProductColor = productColor;
      await productRepository.UpdateAsync(entity: product,
                                          cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductRating
    public async Task<ThreadActivity> InsertProductRating(ThreadActivity rating, IList<ThreadActivity> conditions, Product product, CancellationToken cancellationToken)
    {
      return await threadService.Rating(rating: rating,
                                        conditions: conditions,
                                        thread: product.Thread,
                                        cancellationToken: cancellationToken);
    }
    public async Task DeleteProductRating(ThreadActivity rating, CancellationToken cancellationToken)
    {
      await threadService.DeleteThreadActivity(threadActivity: rating,
                                               cancellationToken: cancellationToken);
    }
    public async Task<ThreadActivity> GetProductRating(int productId, int ratingId, CancellationToken cancellationToken, IInclude<ThreadActivity> include = null)
    {
      var query = GetProductRatings(productId: productId,
                                    include: include);
      query = query.Where(x => x.Id == ratingId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    public IQueryable<ThreadActivity> GetProductRatings(int productId, IInclude<ThreadActivity> include = null)
    {
      return getProductThreadActivities(productId: productId,
                                        type: ThreadActivityType.Rating,
                                        include: include);
    }
    #endregion
    #region ProductQuestion
    public async Task<ThreadActivity> AskProductQuestion(ThreadActivity question, Product product, CancellationToken cancellationToken)
    {
      return await threadService.AddQuestion(question: question,
                                             thread: product.Thread,
                                             cancellationToken: cancellationToken);
    }
    public async Task UpdateProductQuestion(ThreadActivity question, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.UpdateThreadActivity(threadActivity: question,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public async Task DeleteProductQuestion(ThreadActivity question, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.DeleteThreadActivity(threadActivity: question,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public IQueryable<ThreadActivity> GetProductQuestions(int productId, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      return getProductThreadActivities(productId: productId,
                                        type: ThreadActivityType.Question,
                                        showUnPublished: false,
                                        include: include);
    }
    public async Task<ThreadActivity> GetProductQuestionById(int productId, int questionId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var query = GetProductQuestions(productId: productId,
                                      include: include);
      query = query.Where(x => x.Id == questionId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductQuestionAnswer
    public async Task<ThreadActivity> AnswerProductQuestion(ThreadActivity answer, ThreadActivity question, CancellationToken cancellationToken)
    {
      return await threadService.AnswerQuestion(answer: answer,
                                                question: question,
                                                cancellationToken: cancellationToken);
    }
    public async Task DeleteProductAnswer(ThreadActivity answer, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.DeleteThreadActivity(threadActivity: answer,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public async Task UpdateProductAnswer(ThreadActivity answer, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.UpdateThreadActivity(threadActivity: answer,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public async Task<ThreadActivity> GetProductAnswer(int productId, int questionId, int answerId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var query = GetProductAnswers(productId: productId,
                                    questionId: questionId,
                                    include: include);
      query = query.Where(x => x.Id == answerId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    public IQueryable<ThreadActivity> GetProductAnswers(int productId, int questionId, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      return getProductThreadActivities(productId: productId,
                                        type: ThreadActivityType.Answer,
                                        referenceId: questionId,
                                        include: include);
    }
    #endregion
    #region Product Like , Rate , Visit
    public async Task<ThreadActivity> RateProduct(ThreadActivity ratingCondition, Product product, CancellationToken cancellationToken)
    {
      return await threadService.Rate(ratingCondition: ratingCondition,
                                      thread: product.Thread,
                                      cancellationToken: cancellationToken);
    }
    public async Task LikeProduct(Product product, CancellationToken cancellationToken)
    {
      await threadService.Like(thread: product.Thread,
                               cancellationToken: cancellationToken);
    }
    public async Task UnlikeProduct(Product product, CancellationToken cancellationToken)
    {
      await threadService.Unlike(thread: product.Thread,
                                 cancellationToken: cancellationToken);
    }
    public async Task VisitProduct(Product product, CancellationToken cancellationToken)
    {
      await threadService.Visit(thread: product.Thread,
                               cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductComment
    public async Task<ThreadActivity> InsertProductComment(ThreadActivity comment, Product product, CancellationToken cancellationToken)
    {
      return await threadService.AddComment(comment: comment,
                                            thread: product.Thread,
                                            cancellationToken: cancellationToken);
    }
    public async Task UpdateProductComment(ThreadActivity comment, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.UpdateThreadActivity(threadActivity: comment,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public async Task<ThreadActivity> GetProductCommentById(int productId, int commentId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var query = GetProductComments(productId: productId,
                                     include: include);
      query = query.Where(x => x.Id == commentId);
      return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public IQueryable<ThreadActivity> GetProductComments(int productId, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      return getProductThreadActivities(productId: productId,
                                        type: ThreadActivityType.Comment,
                                        include: include);
    }
    public async Task DeleteProductComment(ThreadActivity comment, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.DeleteThreadActivity(threadActivity: comment,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public async Task LikeProductComment(ThreadActivity comment, CancellationToken cancellationToken)
    {
      await threadService.Like(thread: comment.Thread,
                               cancellationToken: cancellationToken,
                               activity: comment);
    }
    public async Task UnlikeProductComment(ThreadActivity comment, CancellationToken cancellationToken)
    {
      await threadService.Unlike(thread: comment.Thread,
                                 cancellationToken: cancellationToken,
                                 activity: comment);
    }
    #endregion
    #region ProductCommentReply
    public async Task<ThreadActivity> InsertProductCommentReply(ThreadActivity reply, ThreadActivity comment, CancellationToken cancellationToken)
    {
      return await threadService.InsertCommentReply(reply: reply,
                                                    comment: comment,
                                                    cancellationToken: cancellationToken);
    }
    public IQueryable<ThreadActivity> GetProductCommentReplies(int productId, int commentId, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var query = getProductThreadActivities(type: ThreadActivityType.Comment,
                                             productId: productId,
                                             include: include,
                                             showUnPublished: showUnPublished);
      query = query.Where(x => x.ReferenceId == commentId);
      return query;
    }
    public async Task UpdateProductCommentReply(ThreadActivity reply, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.UpdateThreadActivity(threadActivity: reply,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    public async Task<ThreadActivity> GetProductCommentReplyById(int productId, int commentId, int replyId, CancellationToken cancellationToken, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var query = GetProductCommentReplies(productId: productId,
                                           commentId: commentId,
                                           include: include);
      query = query.Where(x => x.Id == replyId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    public async Task DeleteProductCommentReply(ThreadActivity reply, CancellationToken cancellationToken, bool checkOwner = false)
    {
      await threadService.DeleteThreadActivity(threadActivity: reply,
                                               cancellationToken: cancellationToken,
                                               checkOwner: checkOwner);
    }
    #endregion
    #region ProductPrice
    public async Task<ProductPrice> CreateProductPrice(ProductPrice productPrice, CancellationToken cancellationToken)
    {
      if (productPrice.IsPublished == true)
      {
        var productPrices = await GetProductPrices(productId: productPrice.ProductId,
                                                  colorId: productPrice.ColorId,
                                                  cityId: productPrice.CityId,
                                                  isPublished: true).ToListAsync(cancellationToken: cancellationToken);
        foreach (var item in productPrices)
        {
          item.IsPublished = false;
          await productPriceRepository.UpdateAsync(entity: item, cancellationToken: cancellationToken);
        }
      }
      await productPriceRepository.AddAsync(entity: productPrice,
                                            cancellationToken: cancellationToken);
      return productPrice;
    }
    public async Task DeleteProductPrice(ProductPrice productPrice, CancellationToken cancellationToken)
    {
      await productPriceRepository.DeleteAsync(entity: productPrice,
                                               cancellationToken: cancellationToken);
    }
    public async Task<ProductPrice> GetProductPriceById(int id, CancellationToken cancellationToken, IInclude<ProductPrice> include = null)
    {
      return await productPriceRepository.GetAsync(predicate: x => x.Id == id,
                                                   cancellationToken: cancellationToken,
                                                   include: include);
    }
    public IQueryable<ProductPrice> GetProductPrices(int? productCategoryId = null,
                                                     int? productId = null,
                                                     int? cityId = null,
                                                     int? provinceId = null,
                                                     bool? isPublished = null,
                                                     int? colorId = null,
                                                     IInclude<ProductPrice> include = null)
    {
      var query = productPriceRepository.GetQuery(include: include);
      if (productCategoryId != null)
        query = query.Where(x => x.Product.ProductCategoryId == productCategoryId);
      if (productId != null)
        query = query.Where(x => x.ProductId == productId);
      if (cityId != null)
        query = query.Where(x => x.CityId == cityId);
      if (provinceId != null)
        query = query.Where(x => x.City.ProvinceId == provinceId);
      if (isPublished != null)
        query = query.Where(x => x.IsPublished == isPublished);
      if (colorId != null)
        query = query.Where(x => x.ColorId == colorId);
      return query;
    }
    public IQueryable<ProductPrice> GetCurrentProductPrices(int? productCategoryId = null, int? productId = null, IInclude<ProductPrice> include = null)
    {
      var currentCityId = workContext.GetCurrentCityId();
      return GetProductPrices(productCategoryId: productCategoryId,
                              productId: productId,
                              cityId: currentCityId,
                              provinceId: null,
                              colorId: null,
                              isPublished: true,
                              include: include);
    }
    public async Task<ProductPrice> GetCurrentProductPrice(int productId, int colorId, CancellationToken cancellationToken)
    {
      var currentCityId = workContext.GetCurrentCityId();
      return await GetProductPrices(
        productId: productId,
        cityId: currentCityId,
        colorId: colorId,
        productCategoryId: null,
        provinceId: null,
        isPublished: true,
        include: null)
        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    public async Task ProductPricePublish(ProductPrice productPrice, CancellationToken cancellationToken)
    {
      var productPrices = await GetProductPrices(productId: productPrice.ProductId,
                                           colorId: productPrice.ColorId,
                                           cityId: productPrice.CityId,
                                           isPublished: true).ToListAsync(cancellationToken: cancellationToken);
      foreach (var item in productPrices)
      {
        item.IsPublished = false;
        await productPriceRepository.UpdateAsync(entity: item, cancellationToken: cancellationToken);
      }
      productPrice.IsPublished = true;
      await productPriceRepository.UpdateAsync(entity: productPrice, cancellationToken: cancellationToken);
    }
    public async Task ProductPriceUnPublish(ProductPrice productPrice, CancellationToken cancellationToken)
    {
      productPrice.IsPublished = false;
      await productPriceRepository.UpdateAsync(entity: productPrice, cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductBrochure
    public async Task<ProductBrochure> InsertProductBrochure(ProductBrochure productBrochure, Product product, CancellationToken cancellationToken)
    {
      productBrochure.Product = product;
      await productBrochureRepository.AddAsync(entity: productBrochure,
                                               cancellationToken: cancellationToken);
      return productBrochure;
    }
    public async Task UpdateProductBrochure(ProductBrochure productBrochure, CancellationToken cancellationToken)
    {
      await productBrochureRepository.UpdateAsync(entity: productBrochure,
                                                  cancellationToken: cancellationToken);
    }
    public async Task DeleteProductBrochure(ProductBrochure productBrochure, CancellationToken cancellationToken)
    {
      await productBrochureRepository.DeleteAsync(entity: productBrochure,
                                                  cancellationToken: cancellationToken);
    }
    public async Task<ProductBrochure> GetProductBrochureByProductId(int productId, CancellationToken cancellationToken, IInclude<ProductBrochure> include = null)
    {
      return await productBrochureRepository.GetAsync(predicate: x => x.ProductId == productId,
                                                      cancellationToken: cancellationToken,
                                                      include: include);
    }
    #endregion
    #region ProductBrochureAttachment 
    public async Task<ProductBrochureAttachment> InsertProductBrochureAttachment(ProductBrochureAttachment productBrochureAttachment, CancellationToken cancellationToken)
    {
      await productBrochureAttachmentRepository.AddAsync(entity: productBrochureAttachment,
                                                         cancellationToken: cancellationToken);
      return productBrochureAttachment;
    }
    public async Task UpdateProductBrochureAttachment(ProductBrochureAttachment productBrochureAttachment, CancellationToken cancellationToken)
    {
      await productBrochureAttachmentRepository.UpdateAsync(entity: productBrochureAttachment,
                                                            cancellationToken: cancellationToken);
    }
    public async Task DeleteProductBrochureAttachment(ProductBrochureAttachment productBrochureAttachment, CancellationToken cancellationToken)
    {
      await productBrochureAttachmentRepository.DeleteAsync(entity: productBrochureAttachment,
                                                            cancellationToken: cancellationToken);
    }
    public async Task<ProductBrochureAttachment> GetProductBrochureAttachmentById(int id, CancellationToken cancellationToken, IInclude<ProductBrochureAttachment> include = null)
    {
      return await productBrochureAttachmentRepository.GetAsync(predicate: x => x.Id == id,
                                                                cancellationToken: cancellationToken);
    }
    public IQueryable<ProductBrochureAttachment> GetProductBrochureAttachmentsByBrochureId(int brochureId, IInclude<ProductBrochureAttachment> include = null)
    {
      var query = productBrochureAttachmentRepository.GetQuery(include: include);
      query = query.Where(x => x.BrochurId == brochureId);
      return query;
    }
    #endregion
    #region Misc
    public async Task<Domains.Threads.Thread> GetThreadByProductId(int productId, CancellationToken cancellationToken, IInclude<Domains.Threads.Thread> include = null)
    {
      var product = await GetProductById(productId, cancellationToken: cancellationToken);
      return await threadService.GetThreadById(id: product.ThreadId,
                                         cancellationToken: cancellationToken,
                                         include: include);
    }
    public IQueryable<ThreadActivity> GetUserComments(bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      return threadService.GetUserThreadActivities(showUnPublished: showUnPublished,
                                                   include: include)
                                                   .Where(x => x.Type == ThreadActivityType.Comment);
    }
    #endregion
    #region ProductTreadActivity    
    private IQueryable<ThreadActivity> getProductThreadActivities(ThreadActivityType type, int? productId = null, int? referenceId = null, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var products = GetProducts();
      if (productId != null)
        products = products.Where(x => x.Id == productId);
      var query = from p in products
                  from ta in p.Thread.Activities
                  where ta.Type == type
                  select ta;
      if (referenceId != null)
        query = query.Where(x => x.ReferenceId == referenceId);
      if (showUnPublished == false)
        query = query.Where(x => x.PublishAt != null);
      if (include != null)
        query = include.Execute(query);
      return query;
    }
    #endregion
    #region ProductTag
    public async Task<ThreadActivity> InsertProductTag(ThreadActivity tag, Product product, CancellationToken cancellationToken)
    {
      return await threadService.AddTag(tag: tag,
                                        thread: product.Thread,
                                        cancellationToken: cancellationToken);
    }
    public async Task UpdateProductTag(ThreadActivity tag, CancellationToken cancellationToken)
    {
      await threadService.UpdateThreadActivity(threadActivity: tag,
                                               cancellationToken: cancellationToken);
    }
    public async Task<ThreadActivity> GetProductTagById(int productId, int tagId, CancellationToken cancellationToken, IInclude<ThreadActivity> include = null)
    {
      var query = GetProductTags(productId: productId,
                                 include: include);
      query = query.Where(x => x.Id == tagId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    public IQueryable<ThreadActivity> GetProductTags(int productId, IInclude<ThreadActivity> include = null)
    {
      return getProductThreadActivities(productId: productId,
                                        type: ThreadActivityType.Tag,
                                        include: include);
    }
    public async Task DeleteProductTag(ThreadActivity tag, CancellationToken cancellationToken)
    {
      await threadService.DeleteThreadActivity(threadActivity: tag,
                                               cancellationToken: cancellationToken);
    }
    #endregion
    #region ProductCategoryBrand
    public async Task<ProductCategoryBrand> NewProductCategoryBrand(Brand brand, ProductCategory productCategory, CancellationToken cancellationToken)
    {
      var productCategoryBrand = new ProductCategoryBrand
      {
        ProductCategory = productCategory,
        Brand = brand
      };
      await productCategoryBrandRepository.AddAsync(entity: productCategoryBrand,
                                                    cancellationToken: cancellationToken);
      return productCategoryBrand;
    }
    public async Task DeleteProductCategoryBrand(ProductCategoryBrand productCategoryBrand, CancellationToken cancellationToken)
    {
      await productCategoryBrandRepository.DeleteAsync(entity: productCategoryBrand,
                                                       cancellationToken: cancellationToken);
    }
    public async Task<ProductCategoryBrand> GetProductCategoryBrandById(int productCategoryId, int brandId, CancellationToken cancellationToken, IInclude<ProductCategoryBrand> include = null)
    {
      return await productCategoryBrandRepository.GetAsync(predicate: x => x.BrandId == brandId && x.ProductCategoryId == productCategoryId,
                                                   cancellationToken: cancellationToken,
                                                   include: include);
    }
    public IQueryable<ProductCategoryBrand> GetProductCategoryBrands(int? productCategoryId = null, int? brandId = null, IInclude<ProductCategoryBrand> include = null)
    {
      var query = productCategoryBrandRepository.GetQuery(include: include);
      if (productCategoryId != null)
        query = query.Where(x => x.ProductCategoryId == productCategoryId);
      if (brandId != null)
        query = query.Where(x => x.BrandId == brandId);
      return query;
    }
    public async Task UpdateProductImage(ProductImage productImage, Product product, CancellationToken cancellationToken)
    {
      productImage.Product = product;
      await productImageRepository.UpdateAsync(productImage, cancellationToken);
    }
    #endregion
    #region  ProductShippingInfo
    public async Task InsertProductShippingInfo(ProductShippingInfo productShippingInfo, CancellationToken cancellationToken)
    {
      productShippingInfo.CreatedAt = DateTime.UtcNow;
      await productShippingInfoRepository.AddAsync(entity: productShippingInfo, cancellationToken: cancellationToken);
    }
    public async Task UpdateProductShippingInfo(ProductShippingInfo productShippingInfo, CancellationToken cancellationToken)
    {
      productShippingInfo.UpdatedAt = DateTime.UtcNow;
      await productShippingInfoRepository.UpdateAsync(entity: productShippingInfo, cancellationToken: cancellationToken);
    }
    public async Task<ProductShippingInfo> GetProductShippingInfoByProductId(int productId, CancellationToken cancellationToken, IInclude<ProductShippingInfo> include = null)
    {
      var productShippingInfo = await productShippingInfoRepository.GetAsync(predicate: x => x.ProductId == productId,
                                                     cancellationToken: cancellationToken,
                                                     include: include);
      return productShippingInfo;
    }
    #endregion
  }
}