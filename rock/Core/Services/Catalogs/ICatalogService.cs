using rock.Framework.Autofac;
using rock.Core.Domains.Catalogs;
using rock.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
namespace rock.Core.Services.Catalogs
{
  public interface ICatalogService : IScopedDependency
  {
    #region Catalog
    Task<Catalog> NewCatalog(CancellationToken cancellationToken);
    Task<Catalog> GetCatalogById(int id, CancellationToken cancellationToken, IInclude<Catalog> include = null);
    Task DeleteCatalog(Catalog catalog, CancellationToken cancellationToken);
    #endregion
    #region CatalogItem
    Task<CatalogItem> InsertCatalogItem(CatalogItem catalogItem, CancellationToken cancellationToken, CatalogItem reference = null);
    Task UpdateCatalogItem(CatalogItem catalogItem, CancellationToken cancellationToken, CatalogItem reference = null);
    Task DeleteCatalogItem(CatalogItem catalogItem, CancellationToken cancellationToken);
    Task<CatalogItem> GetCatalogItemById(int id, int catalogId, CancellationToken cancellationToken, IInclude<CatalogItem> include = null);
    IQueryable<CatalogItem> GetCatalogItemsByCatalogId(int catalogId, IInclude<CatalogItem> include = null);
    IQueryable<CatalogItem> GetChildCatalogItems(int referenceId, IInclude<CatalogItem> include = null);
    #endregion
    #region CatalogMemory
    Task<CatalogMemory> InsertCatalogMemory(CatalogMemory catalogMemory, CancellationToken cancellationToken);
    Task UpdateCatalogMemory(CatalogMemory catalogMemory, CancellationToken cancellationToken);
    Task DeleteCatalogMemory(CatalogMemory catalogMemory, CancellationToken cancellationToken);
    Task<CatalogMemory> GetCatalogMemoryById(int id, CancellationToken cancellationToken, IInclude<CatalogMemory> include = null);
    #endregion
    #region CatalogMemoryItem
    IQueryable<CatalogMemoryItem> GetCatalogMemoryItemsByCatalogItemId(int catalogItemId, IInclude<CatalogMemoryItem> include = null);
    IQueryable<CatalogMemoryItem> GetCatalogMemoryItemsByCatalogMemoryId(int catalogMemoryId, IInclude<CatalogMemoryItem> include = null);
    Task<CatalogMemoryItem> InsertCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken);
    Task UpdateCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken);
    Task DeleteCatalogMemoryItem(CatalogMemoryItem catalogMemoryItem, CancellationToken cancellationToken);
    Task DeleteCatalogMemoryItemsByCatalogMemoryId(int catalogMemoryId, CancellationToken cancellationToken);
    Task<CatalogMemoryItem> GetCatalogMemoryItemById(int Id, CancellationToken cancellationToken, IInclude<CatalogMemoryItem> include = null);
    #endregion
  }
}