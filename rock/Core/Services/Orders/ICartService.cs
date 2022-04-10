using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Payment;
using rock.Core.Services.Payment;
using rock.Framework.Autofac;
using rock.Models.CustomerApi;

namespace rock.Core.Services.Orders
{
  public interface ICartService : IScopedDependency, IPaymentHandler
  {
    #region Cart    
    Task<Cart> GetOrCreateCart(CancellationToken cancellationToken, IInclude<Cart> include = null);
    Task<Cart> GetCart(CancellationToken cancellationToken, IInclude<Cart> include = null);
    Task ArchiveCart(Cart cart, CancellationToken cancellationToken);
    Task UpdateCartAddress(Cart cart, CancellationToken cancellationToken);
    IQueryable<Cart> GetCarts(int? userId = null, IInclude<Cart> include = null);
    Task<double> CalculateOrderShippingCost(CancellationToken cancellationToken);
    Task<List<int>> GetNotAvailableCartItemShippings(CancellationToken cancellationToken);
    Task<CartBillModel> GetCartBill(Coupon coupon, CancellationToken cancellationToken);
    #endregion

    #region CartItem
    Task<CartItem> GetCurrentCartItemById(int id, CancellationToken cancellationToken, IInclude<CartItem> include = null);
    Task<CartItem> GetCartItemById(int id, CancellationToken cancellationToken, IInclude<CartItem> include = null);
    Task<CartItem> CreateOrUpdateCartItem(CartItem cartItem, CancellationToken cancellationToken);
    Task DeleteCartItem(CartItem cartItem, CancellationToken cancellationToken);
    Task EditCartItem(CartItem cartItem, CancellationToken cancellationToken);
    Task DeleteCartItemsByCartId(int cartId, CancellationToken cancellationToken);
    #endregion
  }
}