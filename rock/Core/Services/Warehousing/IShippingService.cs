using System;
using System.Collections.Generic;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Core.Domains.Warehousing;

namespace rock.Core.Services.Warehousing
{
  public interface IShippingService : IScopedDependency
  {
      void InsertShipping(Shipping entity);
      void UpdateShipping(Shipping entity, byte[] rowVersion);
      void DeleteShipping(Shipping entity);
      Shipping GetShippingById(int Id);
  }
}