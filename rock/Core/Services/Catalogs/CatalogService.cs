using System;
using rock.Core.Domains.Catalogs;
using rock.Core.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace rock.Core.Services.Catalogs
{
  public class CatalogService : ICatalogService
  {
    #region Fields
    private readonly IRepository<Catalog> catalogRepository;
    private readonly IRepository<CatalogItem> catalogItemRepository;
    private readonly IRepository<CatalogMemory> catalogMemoryRepository;
    private readonly IRepository<CatalogMemoryItem> catalogMemoryItemRepository;
    #endregion
    #region Constractor
    public CatalogService(IRepository<Catalog> catalogRepository,
                          IRepository<CatalogItem> catalogItemRepository,
                          IRepository<CatalogMemory> catalogMemoryRepository,
                          IRepository<CatalogMemoryItem> catalogMemoryItemRepository)
    {
      this.catalogRepository = catalogRepository;
      this.catalogItemRepository = catalogItemRepository;
      this.catalogMemoryRepository = catalogMemoryRepository;
      this.catalogMemoryItemRepository = catalogMemoryItemRepository;
    }
    #endregion
    #region Catalog
    public async Task<Catalog> GetCatalogById(int id, CancellationToken cancellationToken, IInclude<Catalog> include = null)
    {
      return await catalogRepository.GetAsync(predicate: x => x.Id == id,
                                              cancellationToken: cancellationToken,
                                              include: include);
    }
    public async Task DeleteCatalog(Catalog catalog, CancellationToken cancellationToken)
    {
      foreach (var catalogItem in catalog.Items)
        await DeleteCatalogItem(catalogItem, cancellationToken);
      await catalogRepository.DeleteAsync(catalog, cancellationToken);
    }
    public async Task<Catalog> NewCatalog(CancellationToken cancellationToken)
    {
      var catalog = new Catalog();
      catalog.CreatedAt = DateTime.UtcNow;
      await catalogRepository.AddAsync(catalog, cancellationToken);
      return catalog;
    }
    #endregion
    #region CatalogItem
    public async Task<CatalogItem> GetCatalogItemById(int id, int catalogId, CancellationToken cancellationToken, IInclude<CatalogItem> include = null)
    {
      return await catalogItemRepository.GetAsync(predicate: x => x.Id == id && x.CatalogId == catalogId,
                                                  cancellationToken: cancellationToken,
                                                  include: include);
    }
    public async Task UpdateCatalogItem(CatalogItem catalogItem, CancellationToken cancellationToken, CatalogItem reference = null)
    {
      await catalogItemRepository.UpdateAsync(catalogItem, cancellationToken);
    }
    public async Task<CatalogItem> InsertCatalogItem(CatalogItem catalogItem, CancellationToken cancellationToken, CatalogItem reference = null)
    {
      await catalogItemRepository.AddAsync(catalogItem, cancellationToken);
      return catalogItem;
    }
    public async Task DeleteCatalogItem(CatalogItem catalogItem, CancellationToken cancellationToken)
    {
      if (catalogItem.Children == null)
        await catalogItemRepository.LoadCollectionAsync(catalogItem, x => x.Children, cancellationToken);
      foreach (var child in catalogItem.Children)
        await DeleteCatalogItem(child, cancellationToken);
      await catalogMemoryItemRepository.DeleteAsync(x => x.CatalogItemId == catalogItem.Id, cancellationToken);
      await catalogItemRepository.DeleteAsync(catalogItem, cancellationToken);
    }
    public IQueryable<CatalogItem> GetCatalogItemsByCatalogId(int catalogId, IInclude<CatalogItem> include = null)
    {
      return catalogItemRepository.GetQuery(include).Where(x => x.CatalogId == catalogId);
    }
    public IQueryable<CatalogItem> GetChildCatalogItems(int referenceId, IInclude<CatalogItem> include = null)
    {
      return catalogItemRepository.GetQuery(include).Where(x => x.ReferenceId == referenceId);
    }
    #endregion
    #region CatalogMemory
    public async Task<CatalogMemory> InsertCatalogMemory(CatalogMemory catalogMemory, CancellationToken cancellationToken)
    {
      await catalogMemoryRepository.AddAsync(catalogMemory, cancellationToken);
      return catalogMemory;
    }
    public async Task DeleteCatalogMemory(CatalogMemory catalogMemory, CancellationToken cancellationToken)
    {
      await catalogMemoryItemRepository.DeleteAsync(predicate: x => x.CatalogMemoryId == catalogMemory.Id,
                                                        cancellationToken: cancellationToken);
      await catalogMemoryRepository.DeleteAsync(entity: catalogMemory,
                                                cancellationToken: cancellationToken);
    }
    public async Task<CatalogMemory> GetCatalogMemoryById(int id, CancellationToken cancellationToken, IInclude<CatalogMemory> include = null)
    {
      return await catalogMemoryRepository.GetAsync(predicate: x => x.Id == id,
                                                    cancellationToken: cancellationToken,
                                                    include: include);
    }
    public async Task UpdateCatalogMemory(CatalogMemory catalogMemory, CancellationToken cancellationToken)
    {
      await catalogMemoryRepository.UpdateAsync(catalogMemory, cancellationToken);
    }
    #endregion
    #region CatalogMemoryItem
    public async Task DeleteCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken)
    {
      await catalogMemoryItemRepository.DeleteAsync(catalogMemoryItem, cancellationToken);
    }
    public async Task DeleteCatalogMemoryItemsByCatalogMemoryId(int catalogMemoryId, CancellationToken cancellationToken)
    {
      await catalogMemoryItemRepository.DeleteAsync(predicate: x => x.CatalogMemoryId == catalogMemoryId,
                                                        cancellationToken: cancellationToken);
    }
    public IQueryable<CatalogMemoryItem> GetCatalogMemoryItemsByCatalogItemId(int catalogItemId, IInclude<CatalogMemoryItem> include = null)
    {
      return catalogMemoryItemRepository.GetQuery(include).Where(x => x.CatalogItemId == catalogItemId);
    }
    public IQueryable<CatalogMemoryItem> GetCatalogMemoryItemsByCatalogMemoryId(int catalogMemoryId, IInclude<CatalogMemoryItem> include = null)
    {
      return catalogMemoryItemRepository.GetQuery(include).Where(x => x.CatalogMemoryId == catalogMemoryId);
    }
    public async Task<CatalogMemoryItem> GetCatalogMemoryItemById(int id, CancellationToken cancellationToken, IInclude<CatalogMemoryItem> include = null)
    {
      return await catalogMemoryItemRepository.GetAsync(predicate: x => x.Id == id,
                                                        cancellationToken: cancellationToken,
                                                        include: include);
    }
    public async Task<CatalogMemoryItem> InsertCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken)
    {
      await catalogMemoryItemRepository.AddAsync(catalogMemoryItem, cancellationToken);
      return catalogMemoryItem;
    }
    public async Task UpdateCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken)
    {
      await catalogMemoryItemRepository.UpdateAsync(catalogMemoryItem, cancellationToken);
    }
    #endregion
  }
}