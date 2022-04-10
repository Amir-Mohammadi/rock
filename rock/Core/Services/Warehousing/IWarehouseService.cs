using rock.Framework.Autofac;
using rock.Core.Domains.Warehousing;
using System.Threading;
using rock.Core.Domains.Commons;
using System.Threading.Tasks;
using rock.Core.Domains.Products;
using System.Linq;
using rock.Models.WarehousingApi;
namespace rock.Core.Services.Warehousing
{
  public interface IWarehouseService : IScopedDependency
  {
    #region Inventory
    Task IncreaseShopInventory(Warehouse warehouse, Product product, Color color, int amount, CancellationToken cancellationToken);
    Task DecreaseShopInventory(Warehouse warehouse, Product product, Color color, int amount, CancellationToken cancellationToken);
    IQueryable<InventoryModel> GetWarehouseInventory(int? warehouseId = null, int? productId = null, int? colorId = null);
    #endregion
    #region Warehouse
    Task<Warehouse> InsertWarehouse(Warehouse warehouse, CancellationToken cancellationToken);
    Task UpdateWarehouse(Warehouse warehouse, CancellationToken cancellationToken);
    #endregion
  }
}