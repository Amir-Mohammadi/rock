using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Products;
using rock.Core.Domains.Profiles;
using rock.Framework.Autofac;

namespace rock.Core.Services.Orders
{
    public interface ITransportService : IScopedDependency
    {

        #region  Tipax
        Task<int> CalculateShippingPrice(List<CartItem> cartItems, ProfileAddress profileAddress, TransportType transportType, CancellationToken cancellationToken);
        Task<int> CalculateTipaxShippingPirce(Product product, ProfileAddress profileAddress, CancellationToken cancellationToken);
        Task<TransportResult> RegisterTipaxShipping(Product product, ProfileAddress profileAddress, CancellationToken cancellationToken);
        #endregion
    }
}