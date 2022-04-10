using System;
using System.Collections.Generic;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Core.Domains.Warehousing;

namespace rock.Core.Services.Warehousing
{
  public interface IMerchantInventoryModificationService : IScopedDependency
  {
      void InsertMerchantInventoryModification(ShopInventoryModification entity);
      void UpdateMerchantInventoryModification(ShopInventoryModification entity, byte[] rowVersion);
      void DeleteMerchantInventoryModification(ShopInventoryModification entity);
      ShopInventoryModification GetMerchantInventoryModificationById(int Id);
  }
}