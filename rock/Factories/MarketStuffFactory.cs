using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Catalogs;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Products;
using rock.Core.Domains.Threads;
using rock.Core.Errors;
using rock.Core.Extensions;
using rock.Core.Services.Common;
using rock.Core.Services.Products;
using rock.Core.Services.Threads;
using rock.Filters;
using rock.Models.CommonApi;
using rock.Models.MarketApi;
using rock.Models.ProductApi;
namespace rock.Factories
{
  public class MarketStuffFactory : BaseFactory, IMarketStuffFactory
  {
    #region Fields
    private readonly IProductService productService;
    private readonly ICommonService commonService;
    private readonly IWorkContext workContext;
    private readonly IErrorFactory errorFactory;
    private readonly IInclude<Product> marketBriefStuffInclude;
    #endregion
    #region Constractor
    public MarketStuffFactory(IProductService productService,
                              ICommonService commonService,
                              IWorkContext workContext,
                              IErrorFactory errorFactory)
    {
      this.productService = productService;
      this.commonService = commonService;
      this.workContext = workContext;
      this.errorFactory = errorFactory;
      marketBriefStuffInclude = new Include<Product>(query =>
      {
        query = query.Include(x => x.Brand);
        query = query.Include(x => x.PreviewProductImage);
        return query;
      });
    }
    #endregion
    #region MarketBriefStuff
    public async Task<IPagedList<MarketBriefStuffModel>> PrepareMarketBriefStuffListModel(MarketBriefStuffSearchParameters parameters, CancellationToken cancellationToken)
    {
      var includeCollections = true;
      var currentCityId = workContext.GetCurrentCityId();
      var query = productService.GetProducts(name: null,
                                             showArchive: false);
      #region Filter
      if (!string.IsNullOrEmpty(parameters.Q))
      {
        query = query.Where(
          x => x.BrowserTitle.Contains(parameters.Q) ||
          x.Name.Contains(parameters.Q) ||
          x.Brand.Name.Contains(parameters.Q) ||
          x.ProductCategory.Name.Contains(parameters.Q)
          );
      }
      if (parameters.Brands != null)
      {
        query = query.Where(x => parameters.Brands.Contains(x.BrandId));
      }
      if (parameters.Categories != null)
      {
        query = query.Where(x => parameters.Categories.Contains(x.ProductCategoryId));
      }
      if (parameters.HasSellingStock == true)
      {
        //TODO: WTF?!
      }
      if (parameters.ExtraSearchParameters != null && parameters.ExtraSearchParameters.Any())
      {
        var productProperties = productService.GetProductProperties();
        foreach (var esp in parameters.ExtraSearchParameters)
        {
          var selectedCatalogMemoryIds = await productProperties.Where(x => x.CatalogItemId.ToString() == esp.Key && esp.Values.Contains(x.Value))
                                                                .Select(x => x.CatalogMemoryId)
                                                                .Distinct()
                                                                .ToListAsync(cancellationToken);
          query = query.Where(x => selectedCatalogMemoryIds.Contains(x.CatalogMemoryId));
        }
      }
      if (parameters.MinmumPrice != null || parameters.MaximumPrice != null)
      {
        includeCollections = false;
        var productPrices = from p in query
                            from pp in p.ProductPrices
                            where pp.CityId == currentCityId && pp.IsPublished == true
                            select new { ProductId = p.Id, pp.Price };
        var prices = productPrices.GroupBy(x => x.ProductId)
                                  .Select(x => new { ProductId = x.Key, Price = x.Min(x => x.Price) });
        if (parameters.MaximumPrice != null)
          prices = prices.Where(x => x.Price <= parameters.MaximumPrice);
        if (parameters.MinmumPrice != null)
          prices = prices.Where(x => x.Price >= parameters.MinmumPrice);
        var productsAndPrices = from p in query
                                join pp in prices on p.Id equals pp.ProductId
                                select new { Product = p, pp.Price };
        query = productsAndPrices.Select(x => x.Product);
      }
      if (parameters.Discounted == true)
      {
        includeCollections = false;
        var productsHasDiscount = query.SelectMany(x => x.ProductPrices)
                                       .Where(x => x.CityId == currentCityId
                                                   && x.IsPublished == true
                                                   && x.Discount > 0)
                                       .Select(x => new { x.ProductId })
                                       .Distinct();
        query = from product in query
                join item in productsHasDiscount on product.Id equals item.ProductId
                orderby product.Id
                select product;
      }
      #endregion
      #region Sort
      var sortBy = Extensions.GetSortBy(parameters.SortBy);
      var order = parameters.Order;
      if (sortBy?.ToLower() == "price")
      {
        includeCollections = false;
        var productPrices = from pp in query.SelectMany(x => x.ProductPrices)
                            where pp.CityId == workContext.GetCurrentCityId() && pp.IsPublished == true
                            select new { pp.Id, pp.ProductId, pp.Price };
        var prices = productPrices.GroupBy(x => x.ProductId)
                                  .Select(x => new { ProductId = x.Key, Price = x.Min(x => x.Price) });
        var productsAndPrices = from p in query
                                join pp in prices on p.Id equals pp.ProductId
                                select new { p.Id, Product = p, pp.Price };
        if (Extensions.IsDescending(order))
          productsAndPrices = productsAndPrices.OrderByDescending(x => x.Price);
        else
          productsAndPrices = productsAndPrices.OrderBy(x => x.Price);
        query = productsAndPrices.Select(x => x.Product);
        order = null;
        sortBy = null;
      }
      if (sortBy?.ToLower() == "visits")
      {
        includeCollections = false;
        var totalVisits = from p in query
                          from a in p.Thread.Activities
                          where a.Type == ThreadActivityType.Visit && a.ReferenceId == null
                          select p.Id;
        var visits = totalVisits.GroupBy(x => x).Select(x => new { ProductId = x.Key, Total = x.Count() });
        query = from p in query
                join tv in visits on p.Id equals tv.ProductId into tVisits
                from v in tVisits.DefaultIfEmpty()
                orderby v.Total descending
                select p;
        order = null;
        sortBy = null;
      }
      if (sortBy?.ToLower() == "sales")
      {
        //TODO: Selling financial documents is not completed yet.
        order = null;
        sortBy = null;
      }
      if (sortBy?.ToLower() == "popular")
      {
        includeCollections = false;
        var totalLikes = from p in query
                         from a in p.Thread.Activities
                         where a.Type == ThreadActivityType.Like && a.ReferenceId == null
                         select p.Id;
        var likes = totalLikes.GroupBy(x => x).Select(x => new { ProductId = x.Key, Total = x.Count() });
        query = from p in query
                join tl in likes on p.ThreadId equals tl.ProductId into tLikes
                from l in tLikes.DefaultIfEmpty()
                orderby l.Total descending
                select p;
        order = null;
        sortBy = null;
      }
      #endregion
      query = marketBriefStuffInclude.Execute(query);
      if (includeCollections)
        query = query.Include(x => x.ProductPrices.Where(x => x.CityId == currentCityId && x.IsPublished == true));
      return await CreateModelPagedListAsync(source: query,
                                             convertFunction: createMarketBriefStuffModel,
                                             sortBy: sortBy,
                                             order: order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             cancellationToken: cancellationToken);
    }
    public async Task<MarketBriefStuffModel> PrepareMarketBriefStuffModel(int marketStuffId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId);
      var product = await productService.GetProductById(id: marketStuffId,
                                                        cancellationToken: cancellationToken,
                                                        include: marketBriefStuffInclude);
      return createMarketBriefStuffModel(product);
    }
    public async Task<IPagedList<MarketBriefStuffModel>> PrepareMarketBriefLastVisitedStuffListModel(CancellationToken cancellationToken)
    {
      var currentUserId = workContext.GetCurrentUserId();
      var currentCityId = workContext.GetCurrentCityId();
      var query = productService.GetProducts(name: null,
                                             showArchive: false);
      #region Filter      
      var visits = from p in query
                   from a in p.Thread.Activities
                   where a.Type == ThreadActivityType.Visit
                         && a.ReferenceId == null
                         && a.UserId == currentUserId
                   select new { ProductId = p.Id, a.CreatedAt };
      var productLastVisits = from item in visits
                              group item by item.ProductId into gItems
                              select new
                              {
                                ProductId = gItems.Key,
                                LatestVisitDateTime = gItems.Max(x => x.CreatedAt)
                              };
      query = from p in query
              join v in productLastVisits on p.Id equals v.ProductId
              orderby v.LatestVisitDateTime descending
              select p;
      #endregion
      query = marketBriefStuffInclude.Execute(query);
      return await CreateModelPagedListAsync(source: query,
                                             convertFunction: createMarketBriefStuffModel,
                                             sortBy: null,
                                             order: null,
                                             pageIndex: 0,
                                             pageSize: 40,
                                             cancellationToken: cancellationToken);
    }
    private MarketBriefStuffModel createMarketBriefStuffModel(Product product)
    {
      if (product == null)
        return null;
      ProductPrice price = null;
      if (product.ProductPrices != null)
        price = product.ProductPrices.OrderBy(x => x.Price).FirstOrDefault();
      else
        price = productService.GetCurrentProductPrices(productId: product.Id)
                              .OrderBy(x => x.Price)
                              .FirstOrDefault();
      return new MarketBriefStuffModel()
      {
        Id = product.Id,
        Name = product.Name,
        UrlTitle = product.UrlTitle,
        BrowserTitle = product.BrowserTitle,
        MetaDescription = product.MetaDescription,
        BriefDescription = product.BriefDescription,
        BrandName = product.Brand.Name,
        Price = price?.Price ?? 0,
        Discount = price?.Discount ?? 0,
        PreviewMarketStuffImage = createMarketStuffImageModel(product.PreviewProductImage)
      };
    }
    #endregion
    #region MarketStuff
    public async Task<MarketStuffModel> PrepareMarketStuffModel(int marketStuffId, CancellationToken cancellationToken)
    {
      CheckMarketStuffExistence(marketStuffId);
      var product = await productService.GetProductById(id: marketStuffId,
                                                          cancellationToken: cancellationToken,
                                                          include: new Include<Product>(query =>
                                                          {
                                                            query = query.Include(x => x.Thread)
                                                            .Include(x => x.Brand)
                                                            .Include(x => x.PreviewProductImage)
                                                            .Include(x => x.ProductCategory);
                                                            return query;
                                                          }));
      var result = createMarketStuffModel(product);
      var likedByMe = await productService.GetUserLikedProducts(productId: marketStuffId)
                                          .AnyAsync(cancellationToken);
      result.LikedByMe = likedByMe;
      return result;
    }
    private MarketStuffPriceModel createMarketStuffPriceModel(ProductPrice productPrice)
    {
      if (productPrice == null)
        return null;
      return new MarketStuffPriceModel
      {
        Id = productPrice.Id,
        ProductId = productPrice.ProductId,
        ColorId = productPrice.ColorId,
        Price = productPrice.Price,
        MaxPrice = productPrice.MaxPrice,
        MinPrice = productPrice.MinPrice,
        Discount = productPrice.Discount,
        DiscountedPrice = productPrice.Price - (productPrice.Price * productPrice.Discount) / 100,
        City = this.createCityModel(productPrice.City)
      };
    }
    private MarketStuffModel createMarketStuffModel(Product product)
    {
      if (product == null)
        return null;
      return new MarketStuffModel
      {
        Id = product.Id,
        Name = product.Name,
        DefaultColorId = product.DefaultColorId,
        Brand = this.createMarketBrandModel(product.Brand),
        ProductCategory = this.createMarketProductCategoryModel(product.ProductCategory),
        PreviewProductImage = this.createMarketStuffImageModel(product.PreviewProductImage),
        RowVersion = product.RowVersion,
        BriefDescription = product.BriefDescription
      };
    }
    #endregion
    #region Color
    private ColorModel CreateColorModel(Color color)
    {
      if (color == null)
        return null;
      return new ColorModel()
      {
        Id = color.Id,
        Name = color.Name,
        Code = color.Code
      };
    }
    #endregion
    #region MarketProductCategory
    public async Task<IList<MarketStuffCategoryModel>> PrepareMarketStuffCategoryListModel(CancellationToken cancellationToken)
    {
      var productCategories = productService.GetProductCategories(showArchive: false, isPublished: true);
      var result = await CreateModelListAsync(source: productCategories,
                                              convertFunction: createMarketProductCategoryModel,
                                              cancellationToken: cancellationToken);
      return result;
    }
    private MarketStuffCategoryModel createMarketProductCategoryModel(ProductCategory productCategory)
    {
      if (productCategory == null)
        return null;
      return new MarketStuffCategoryModel
      {
        Id = productCategory.Id,
        Name = productCategory.Name,
        UrlTitle = productCategory.UrlTitle,
        BrowserTitle = productCategory.BrowserTitle,
        MetaDescription = productCategory.MetaDescription,
        Explanation = productCategory.Explanation,
        ParentId = productCategory.ParentId,
        RowVersion = productCategory.RowVersion
      };
    }
    #endregion
    #region MarketStuff
    public async Task<IList<MarketBrandModel>> PrepareMarketBrandListModel(CancellationToken cancellationToken)
    {
      var brands = commonService.GetBrands();
      var result = await CreateModelListAsync(source: brands,
                                              convertFunction: createMarketBrandModel,
                                              cancellationToken: cancellationToken);
      return result;
    }
    public async Task<MarketBrandModel> PrepareMarketBrandModel(int brandId, CancellationToken cancellationToken)
    {
      var brand = await commonService.GetBrandById(id: brandId, cancellationToken);
      var result = createMarketBrandModel(brand);
      return result;
    }
    public async Task<IList<MarketStuffCategoryModel>> PrepareMarketBrandStuffCategoryListModel(int brandId, CancellationToken cancellationToken)
    {
      var productCategories = productService.GetProductCategories(brandId: brandId, showArchive: false, isPublished: true);
      var result = await CreateModelListAsync(source: productCategories,
                                              convertFunction: createMarketProductCategoryModel,
                                              cancellationToken: cancellationToken);
      return result;
    }
    private MarketBrandModel createMarketBrandModel(Brand brand)
    {
      if (brand == null)
        return null;
      var result = new MarketBrandModel
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
        ProfileId = brand.ProfileId,
        RowVersion = brand.RowVersion
      };
      return result;
    }
    #endregion
    #region MarketStuffQuestion
    IInclude<ThreadActivity> questionInclude = new Include<ThreadActivity>(query =>
                                                               {
                                                                 query = query.Include(x => x.User);
                                                                 query = query.Include(x => x.User.Profile);
                                                                 query = query.Include(x => x.User.Profile.PersonProfile);
                                                                 query = query.Include(x => x.ThreadActivityItems.Where(x => x.Type == ThreadActivityType.Answer && x.PublishAt != null))
                                                                 .ThenInclude(x => x.User)
                                                                 .ThenInclude(x => x.Profile)
                                                                 .ThenInclude(x => x.PersonProfile);
                                                                 return query;
                                                               });
    public async Task<IPagedList<MarketStuffQuestionModel>> PrepareMarketStuffQuestionListModel(int marketStuffId, PagedListFilter pagedListFilter, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var query = productService.GetProductQuestions(productId: marketStuffId,
                                                     showUnPublished: false,
                                                     include: questionInclude);
      var result = await CreateModelPagedListAsync(source: query,
                                                   convertFunction: createMarketStuffQuestionModel,
                                                   sortBy: pagedListFilter.SortBy,
                                                   order: pagedListFilter.Order,
                                                   pageIndex: pagedListFilter.PageIndex,
                                                   pageSize: pagedListFilter.PageSize,
                                                   cancellationToken: cancellationToken
                                                   );
      return result;
    }
    public async Task<MarketStuffQuestionModel> PrepareMarketStuffQuestionModel(int marketStuffId, int questionId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var question = await productService.GetProductQuestionById(productId: marketStuffId,
                                                                 questionId: questionId,
                                                                 cancellationToken: cancellationToken,
                                                                 showUnPublished: false,
                                                                 include: questionInclude);
      var result = createMarketStuffQuestionModel(question);
      return result;
    }
    private MarketStuffQuestionModel createMarketStuffQuestionModel(ThreadActivity question)
    {
      if (question == null)
        return null;
      return new MarketStuffQuestionModel
      {
        Id = question.Id,
        UserId = question.UserId,
        FirstName = question.User.Profile?.PersonProfile?.FirstName,
        LastName = question.User.Profile?.PersonProfile?.LastName,
        ProfileId = question.User.ProfileId,
        Payload = question.Payload,
        CreatedAt = question.CreatedAt,
        Answers = question.ThreadActivityItems.Select(x => createMarketStuffAnswerModel(x)).ToArray()
      };
    }
    #endregion
    #region MarketStuffProperty
    public async Task<IList<MarketStuffPropertyModel>> PrepareMarketStuffPropertyListModel(int marketStuffId, CancellationToken cancellationToken, bool? isMain = null)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productProperties = this.productService.GetProductProperties(productId: marketStuffId,
                                                                       isMain: isMain,
                                                                       include: new Include<CatalogMemoryItem>(query =>
                                                                       {
                                                                         query = query.Include(x => x.CatalogItem);
                                                                         query = query.Include(x => x.CatalogItem).ThenInclude(x => x.Reference);
                                                                         query = query.Include(x => x.CatalogItem).ThenInclude(x => x.Children);
                                                                         return query;
                                                                       }));
      return await this.CreateModelListAsync(source: productProperties,
                                             convertFunction: createMarketStuffPropertyModel,
                                             cancellationToken: cancellationToken);
    }
    private MarketStuffPropertyModel createMarketStuffPropertyModel(CatalogMemoryItem catalogMemoryItem)
    {
      if (catalogMemoryItem == null)
        return null;
      var value = catalogMemoryItem.Value;
      if (catalogMemoryItem.CatalogItem.Type == CatalogItemType.List)
        value = catalogMemoryItem.CatalogItem.Children.FirstOrDefault(x => x.Id.ToString() == value)?.Value;
      return new MarketStuffPropertyModel()
      {
        Id = catalogMemoryItem.Id,
        Value = value,
        ExtraKeyName = catalogMemoryItem.ExtraKey,
        CatalogItemKeyName = catalogMemoryItem.CatalogItem.Value,
        ShowInFilter = catalogMemoryItem.CatalogItem.ShowInFilter,
        CatalogItemId = catalogMemoryItem.CatalogItemId,
        Order = catalogMemoryItem.CatalogItem.Order,
        ReferenceId = catalogMemoryItem.CatalogItem.ReferenceId,
        IsMain = catalogMemoryItem.CatalogItem.IsMain,
        Reference = this.createCatalogItemModel(catalogMemoryItem.CatalogItem.Reference)
      };
    }
    private CatalogItemModel createCatalogItemModel(CatalogItem catalogItem)
    {
      if (catalogItem == null)
        return null;
      return new CatalogItemModel()
      {
        Id = catalogItem.Id,
        Value = catalogItem.Value,
        Order = catalogItem.Order,
        ReferenceId = catalogItem.ReferenceId,
        IsMain = catalogItem.IsMain,
        Type = catalogItem.Type,
        ShowInFilter = catalogItem.ShowInFilter,
        HasMultiple = catalogItem.HasMultiple,
        CatalogId = catalogItem.CatalogId,
        RowVersion = catalogItem.RowVersion,
      };
    }
    #endregion
    #region MarketStuffImage
    public async Task<MarketStuffImageModel> PrepareMarketStuffImageModel(int marketStuffId, int imageId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productImage = await this.productService.GetProductImageById(productId: marketStuffId, imageId: imageId, cancellationToken: cancellationToken);
      if (productImage == null)
      {
        throw this.errorFactory.ResourceNotFound(imageId, "MarketStuffImage");
      }
      return this.createMarketStuffImageModel(productImage);
    }
    public async Task<IList<MarketStuffImageModel>> PrepareMarketStuffImageListModel(int marketStuffId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productImages = this.productService.GetProductImages(productId: marketStuffId);
      return await this.CreateModelListAsync(productImages, createMarketStuffImageModel, cancellationToken);
    }
    private MarketStuffImageModel createMarketStuffImageModel(ProductImage productImage)
    {
      if (productImage == null)
        return null;
      return new MarketStuffImageModel()
      {
        Id = productImage.Id,
        ImageId = productImage.ImageId,
        Order = productImage.Order,
        ImageAlt = productImage.ImageAlt,
        ImageTitle = productImage.ImageTitle,
        RowVersion = productImage.RowVersion
      };
    }
    #endregion
    #region MarketStiffPrice
    public async Task<IList<MarketStuffPriceModel>> PrepareMarketStuffPriceListModel(int marketStuffId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productPrices = productService.GetCurrentProductPrices(productId: marketStuffId,
                                                                 include: new Include<ProductPrice>(query =>
                                                                 {
                                                                   query = query.Include(x => x.City).ThenInclude(x => x.Province);
                                                                   return query;
                                                                 }));
      return await CreateModelListAsync(source: productPrices,
                                        convertFunction: createMarketStuffPriceModel,
                                        cancellationToken: cancellationToken);
    }
    #endregion
    #region MargetStuffColor
    public async Task<MarketStuffColorModel> PrepareMarketStuffColorModel(int marketStuffId, int colorId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productColor = await this.productService.GetProductColorById(productId: marketStuffId,
                                                                       colorId: colorId,
                                                                       cancellationToken: cancellationToken,
                                                                       include: new Include<ProductColor>(query =>
                                                                       {
                                                                         query = query.Include(x => x.Color);
                                                                         return query;
                                                                       }));
      if (productColor == null)
      {
        throw this.errorFactory.ResourceNotFound(colorId, "MarketStuffColor");
      }
      return this.createMarketStuffColorModel(productColor);
    }
    public async Task<IList<MarketStuffColorModel>> PrepareMarketStuffColorListModel(int marketStuffId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productColors = this.productService.GetProductColors(productId: marketStuffId,
                                                              include: new Include<ProductColor>(query =>
                                                              {
                                                                query = query.Include(x => x.Color);
                                                                return query;
                                                              }));
      return await this.CreateModelListAsync(productColors, this.createMarketStuffColorModel, cancellationToken);
    }
    private MarketStuffColorModel createMarketStuffColorModel(ProductColor productColor)
    {
      if (productColor == null)
        return null;
      return new MarketStuffColorModel
      {
        ColorId = productColor.ColorId,
        Color = this.CreateColorModel(productColor.Color),
        Code = productColor.Color.Code,
      };
    }
    #endregion
    #region MarketStuffBrochure
    public async Task<MarketStuffBrochureModel> PrepareMarketStuffBrochureModel(int marketStuffId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productBrochure = await this.productService.GetProductBrochureByProductId(marketStuffId, cancellationToken);
      if (productBrochure == null)
      {
        return null;
      }
      return this.createMarketStuffBrochureModel(productBrochure);
    }
    private MarketStuffBrochureModel createMarketStuffBrochureModel(ProductBrochure productBrochure)
    {
      if (productBrochure == null)
        return null;
      return new MarketStuffBrochureModel
      {
        Id = productBrochure.Id,
        ProductId = productBrochure.ProductId,
        Html = productBrochure.HTML,
        RowVersion = productBrochure.RowVersion
      };
    }
    #endregion
    #region MarketStuffTag
    public async Task<MarketStuffTagModel> PrepareMarketStuffTagModel(int marketStuffId, int tagId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId);
      var productTag = await this.productService.GetProductTagById(productId: marketStuffId,
                                                                           tagId: tagId,
                                                                           cancellationToken: cancellationToken);
      return this.createMarketStuffTagModel(productTag: productTag);
    }
    public async Task<IList<MarketStuffTagModel>> PrepareMarketStuffTagListModel(int marketStuffId, CancellationToken cancellationToken)
    {
      var productTags = this.productService.GetProductTags(productId: marketStuffId);
      return await this.CreateModelListAsync(source: productTags,
                                             convertFunction: this.createMarketStuffTagModel,
                                             cancellationToken: cancellationToken);
    }
    private MarketStuffTagModel createMarketStuffTagModel(ThreadActivity productTag)
    {
      //TODO complate this
      if (productTag == null)
        return null;
      return new MarketStuffTagModel
      { };
    }
    #endregion
    #region MarketStuffAnswer
    public async Task<IPagedList<MarketStuffAnswerModel>> PrepareMarketStuffAnswerPagedListModel(int marketStuffId,
                                                                                                 int questionId,
                                                                                                 ProductAnswerSearchParameters parameters,
                                                                                                 CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var answers = this.productService.GetProductAnswers(productId: marketStuffId,
                                                          questionId: questionId,
                                                          include: questionInclude);
      return await this.CreateModelPagedListAsync(source: answers,
                                                  convertFunction: this.createMarketStuffAnswerModel,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  cancellationToken: cancellationToken);
    }
    public async Task<MarketStuffAnswerModel> PrepareMarketStuffAnswerModel(int marketStuffId,
                                                                            int questionId,
                                                                            int answerId,
                                                                            CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var answer = await this.productService.GetProductAnswer(productId: marketStuffId, questionId: questionId, answerId: answerId, cancellationToken);
      return this.createMarketStuffAnswerModel(answer);
    }
    private MarketStuffAnswerModel createMarketStuffAnswerModel(ThreadActivity answer)
    {
      if (answer == null)
        return null;
      return new MarketStuffAnswerModel()
      {
        Id = answer.Id,
        ProfileId = answer.User.ProfileId,
        UserId = answer.UserId,
        Payload = answer.Payload,
        FirstName = answer.User.Profile?.PersonProfile?.FirstName,
        LastName = answer.User.Profile?.PersonProfile?.LastName,
        CreatedAt = answer.CreatedAt,
        RowVersion = answer.RowVersion
      };
    }
    #endregion
    #region MarketStuffComment
    public async Task<MarketStuffCommentModel> PrepareMarketStuffCommentModel(int marketStuffId, int commentId, CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productComment = await this.productService.GetProductCommentById(productId: marketStuffId,
                                                                           commentId: commentId,
                                                                           cancellationToken: cancellationToken,
                                                                           include: new Include<ThreadActivity>(query =>
                                                                            {
                                                                              query = query.Include(x => x.User);
                                                                              query = query.Include(x => x.User.Profile);
                                                                              query = query.Include(x => x.User.Profile.PersonProfile);
                                                                              return query;
                                                                            }));
      return this.createMarketStuffCommentModel(productComment: productComment);
    }
    public async Task<IPagedList<MarketStuffCommentModel>> PrepareMarketStuffCommentPagedListModel(int marketStuffId,
                                                                                                   ProductCommentSearchParameters parameters,
                                                                                                   CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productComments = this.productService.GetProductComments(productId: marketStuffId,
                                                                    showUnPublished: false,
                                                                     include: new Include<ThreadActivity>(query =>
                                                                            {
                                                                              query = query.Include(x => x.User);
                                                                              query = query.Include(x => x.User.Profile);
                                                                              query = query.Include(x => x.User.Profile.PersonProfile);
                                                                              return query;
                                                                            })
      );
      return await this.CreateModelPagedListAsync(source: productComments,
                                                  this.createMarketStuffCommentModel,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  sortBy: parameters.SortBy,
                                                  cancellationToken: cancellationToken);
    }
    private MarketStuffCommentModel createMarketStuffCommentModel(ThreadActivity productComment)
    {
      if (productComment == null)
        return null;
      return new MarketStuffCommentModel
      {
        Id = productComment.Id,
        UserId = productComment.UserId,
        ProfileId = productComment.User.ProfileId,
        FirstName = productComment.User.Profile?.PersonProfile?.FirstName,
        LastName = productComment.User.Profile?.PersonProfile?.LastName,
        Payload = productComment.Payload,
        CreatedAt = productComment.CreatedAt,
      };
    }
    #endregion
    #region MarketStuffCommentReply
    public async Task<MarketStuffCommentReplyModel> PrepareMarketStuffCommentReplyModel(int marketStuffId,
                                                                                        int commentId,
                                                                                        int replyId,
                                                                                        CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productCommnetReply = await productService.GetProductCommentReplyById(marketStuffId, commentId, replyId, cancellationToken);
      return createMarketStuffCommentReplyModel(productCommentReply: productCommnetReply);
    }
    public async Task<IPagedList<MarketStuffCommentReplyModel>> PrepareMarketStuffCommentReplyPagedListModel(int marketStuffId,
                                                                                                             int commentId,
                                                                                                             ProductCommentReplySearchParameters parameters,
                                                                                                             CancellationToken cancellationToken)
    {
      this.CheckMarketStuffExistence(marketStuffId: marketStuffId);
      var productCommentReplies = this.productService.GetProductCommentReplies(productId: marketStuffId,
                                                                               commentId: commentId);
      return await this.CreateModelPagedListAsync(source: productCommentReplies,
                                       convertFunction: this.createMarketStuffCommentReplyModel,
                                       order: parameters.Order,
                                       pageIndex: parameters.PageIndex,
                                       pageSize: parameters.PageSize,
                                       sortBy: parameters.SortBy,
                                       cancellationToken: cancellationToken);
    }
    private MarketStuffCommentReplyModel createMarketStuffCommentReplyModel(ThreadActivity productCommentReply)
    {
      //TODO complate this 
      if (productCommentReply == null)
        return null;
      return new MarketStuffCommentReplyModel { };
    }
    private void CheckMarketStuffExistence(int marketStuffId)
    {
      var isProductExist = this.productService.CheckProductExistencById(marketStuffId);
      if (!isProductExist)
      {
        throw this.errorFactory.ResourceNotFound(marketStuffId, "MarketStuff");
      }
    }
    #endregion
    #region MarketStuffCategoryProperty
    public async Task<IList<MarketStuffCategoryPropertyModel>> PrepareMarketStuffCategoryPropertyListModel(int marketStuffCategoryId, CancellationToken cancellationToken)
    {
      var categoryProperties = this.productService.GetProductCategoryProperties(productCategoryId: marketStuffCategoryId);
      return await this.CreateModelListAsync(categoryProperties, this.createMarketStuffCategoryPropertyModel, cancellationToken);
    }
    private MarketStuffCategoryPropertyModel createMarketStuffCategoryPropertyModel(CatalogItem productCategoryProperty)
    {
      if (productCategoryProperty == null)
        return null;
      return new MarketStuffCategoryPropertyModel()
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
    #region MarketStuffCategoryBrand
    public async Task<IList<MarketBrandModel>> PrepareMarketStuffCategoryBrandListModel(int marketStuffCategoryId, CancellationToken cancellationToken)
    {
      var productCategoryBrands = this.productService.GetProductCategoryBrands(productCategoryId: marketStuffCategoryId,
                                                                               include: new Include<ProductCategoryBrand>(query =>
                                                                               {
                                                                                 query = query.Include(x => x.Brand);
                                                                                 return query;
                                                                               }));
      var brands = productCategoryBrands.Select(x => x.Brand);
      return await this.CreateModelListAsync(brands, this.createMarketBrandModel, cancellationToken);
    }
    #endregion
    #region MarketStuffCategoryStuffPrice
    public async Task<IList<double>> PrepareMarketStuffCategoryStuffPriceListModel(int marketStuffCategoryId, CancellationToken cancellationToken)
    {
      var currentCityId = workContext.GetCurrentCityId();
      var productPrices = this.productService.GetProductPrices(cityId: currentCityId).Where(x => x.Product.ProductCategoryId == marketStuffCategoryId);
      var prices = productPrices.Select(x => x.Price).Distinct().OrderBy(x => x);
      return await this.CreateModelListAsync(source: prices,
                                             convertFunction: x => x,
                                             cancellationToken: cancellationToken);
    }
    #endregion
    #region MarketStuffCategoryPropertyValue
    public async Task<IList<MarketStuffCategoryPropertyValueModel>> PrepareMarketStuffCategoryPropertyValueListModel(int marketStuffCategoryId, int propertyId, CancellationToken cancellationToken)
    {
      var property = await this.productService.GetProductCategoryPropertyById(id: propertyId,
                                                                             productCategoryId: marketStuffCategoryId,
                                                                             include: new Include<CatalogItem>(query =>
                                                                             {
                                                                               query = query.Include(x => x.Children);
                                                                               return query;
                                                                             }),
                                                                             cancellationToken: cancellationToken);
      if (property.Type == CatalogItemType.List)
      {
        return property.Children.Select(x => new MarketStuffCategoryPropertyValueModel()
        {
          Key = x.Id.ToString(),
          Value = x.Value
        }).ToList();
      }
      else if (property.Type == CatalogItemType.Text || property.Type == CatalogItemType.Number)
      {
        var query = productService.GetProductProperties().Where(x => x.CatalogItemId == propertyId);
        return query.Select(x => new MarketStuffCategoryPropertyValueModel() { Key = x.Value.ToString(), Value = x.Value })
                    .Distinct()
                    .ToList();
      }
      return new List<MarketStuffCategoryPropertyValueModel>();
    }
    #endregion
  }
}