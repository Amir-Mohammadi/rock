using rock.Core.Domains.Orders;
using rock.Core.Domains.Profiles;
using rock.Core.Services.Tipax.TipaxResponse;
using rock.Framework.Autofac;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax
{
    public interface ITipaxService : IScopedDependency
    {
        Task<int> CalculateContractPrice(List<CartItem> cartItems, ProfileAddress profileAddress, CancellationToken cancellationToken);
    }
}
