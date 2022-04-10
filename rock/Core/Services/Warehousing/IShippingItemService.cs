using System;
using System.Collections.Generic;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Core.Domains.Warehousing;

namespace rock.Core.Services.Warehousing
{
  public interface IShippingItemService : IScopedDependency
  {
      void InsertShippingItem(ShippingItem entity);
      void UpdateShippingItem(ShippingItem entity, byte[] rowVersion);
      void DeleteShippingItem(ShippingItem entity);
      ShippingItem GetShippingItemById(int Id);
  }
}