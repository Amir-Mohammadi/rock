
using rock.Core.Common;

using System.Collections.Generic;
using rock.Core.Domains.Warehousing;
namespace rock.Core.Services.Warehousing
{
  public partial class ShippingItemService : IShippingItemService
  {
    protected ShippingItemService()
    {
    }
      public void InsertShippingItem(ShippingItem entity) 
      {
        throw new System.NotImplementedException();
      }

     public void UpdateShippingItem(ShippingItem entity, byte[] rowVersion)
     {
        throw new System.NotImplementedException();
     }
     public void DeleteShippingItem(ShippingItem entity)
     {
        throw new System.NotImplementedException();
     }
     public ShippingItem GetShippingItemById(int Id)
     {
        throw new System.NotImplementedException();
     }
  }
}