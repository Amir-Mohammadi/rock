using rock.Framework.Autofac;
using rock.Core.Domains.Warehousing;
namespace rock.Core.Services.Warehousing
{
  public interface IMerchantInventoryModificationItemService : IScopedDependency
  {
    void InsertMerchantInventoryModificationItem(ShopInventoryModificationItem entity);
    void UpdateMerchantInventoryModificationItem(ShopInventoryModificationItem entity, byte[] rowVersion);
    void DeleteMerchantInventoryModificationItem(ShopInventoryModificationItem entity);
    ShopInventoryModificationItem GetMerchantInventoryModificationItemById(int Id);
  }
}