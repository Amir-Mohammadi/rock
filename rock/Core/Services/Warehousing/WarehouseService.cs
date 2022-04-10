using rock.Core.Domains.Warehousing;
using System.Threading.Tasks;
using rock.Core.Domains.Commons;
using System.Threading;
using rock.Core.Data;
using rock.Core.Services.Documents;
using rock.Core.Domains.Products;
using System.Linq;
using rock.Models.WarehousingApi;
namespace rock.Core.Services.Warehousing
{
  public partial class WarehouseService : IWarehouseService
  {
    #region Field
    private readonly IRepository<WarehouseTransaction> warehouseTransactionRepository;
    private readonly IRepository<Warehouse> warehouseRepository;
    private readonly IDocumentService documentService;
    #endregion
    #region Constractor
    public WarehouseService(IRepository<WarehouseTransaction> warehouseTransactionRepository,
                            IRepository<Warehouse> warehouseRepository,
                            IDocumentService documentService)
    {
      this.warehouseTransactionRepository = warehouseTransactionRepository;
      this.warehouseRepository = warehouseRepository;
      this.documentService = documentService;
    }
    #endregion
    #region Inventory
    public async Task DecreaseShopInventory(Warehouse warehouse, Product product, Color color, int amount, CancellationToken cancellationToken)
    {
      var document = await documentService.CreateDocument(form: StaticData.Forms.DecreaseShopInventory,
                                                          description: "کاهش موجودی انبار",
                                                          cancellationToken: cancellationToken);
      var warehouseTransaction = new WarehouseTransaction
      {
        Document = document,
        Amount = amount,
        Product = product,
        Color = color,
        Warehouse = warehouse,
        Factor = TransactionType.Debit,
      };
      await warehouseTransactionRepository.AddAsync(entity: warehouseTransaction,
                                                    cancellationToken: cancellationToken);
    }
    public async Task IncreaseShopInventory(Warehouse warehouse, Product product, Color color, int amount, CancellationToken cancellationToken)
    {
      var document = await documentService.CreateDocument(form: StaticData.Forms.IncreaseShopInventory,
                                                          description: "افزایش موجود انبار",
                                                          cancellationToken: cancellationToken);
      var WarehouseTransaction = new WarehouseTransaction
      {
        Document = document,
        Amount = amount,
        Product = product,
        Color = color,
        Warehouse = warehouse,
        Factor = TransactionType.Credit,
      };
      await warehouseTransactionRepository.AddAsync(entity: WarehouseTransaction,
                                                    cancellationToken: cancellationToken);
    }
    public IQueryable<InventoryModel> GetWarehouseInventory(int? warehouseId = null, int? productId = null, int? colorId = null)
    {
      var query = warehouseTransactionRepository.GetQuery();
      if (warehouseId != null)
        query = query.Where(x => x.WarehouseId == warehouseId);
      if (productId != null)
        query = query.Where(x => x.ProductId == productId);
      if (colorId != null)
        query = query.Where(x => x.ColorId == colorId);
      return from tr in query
             group tr by new { tr.ProductId, tr.WarehouseId, tr.ColorId } into gItems
             select new InventoryModel
             {
               WarehouseId = gItems.Key.WarehouseId,
               ProductId = gItems.Key.ProductId,
               ColorId = gItems.Key.ColorId,
               Amount = gItems.Sum(x => x.Amount * (int)x.Factor)
             };
    }
    #endregion
    #region Warehouse
    public async Task<Warehouse> InsertWarehouse(Warehouse warehouse, CancellationToken cancellationToken)
    {
      await warehouseRepository.AddAsync(entity: warehouse,
                                         cancellationToken: cancellationToken);
      return warehouse;
    }
    public async Task UpdateWarehouse(Warehouse warehouse, CancellationToken cancellationToken)
    {
      await warehouseRepository.UpdateAsync(entity: warehouse,
                                            cancellationToken: cancellationToken);
    }
    #endregion
  }
}