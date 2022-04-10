using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Common;
using rock.Core.Domains.Orders;
using rock.Core.Errors;
using rock.Core.Services.Common;
using rock.Core.Services.Orders;
using rock.Core.Services.Products;
using rock.Factories;
using rock.Models.CustomerApi;

namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [Authorize]
  public class CustomerApiController : BaseController
  {
    #region Fields
    private readonly ICustomerFactory factory;
    private readonly ICommonService commonService;
    private readonly ICartService cartService;
    private readonly IComplexIdBundlerService complexIdBundlers;
    private readonly IProductService productService;
    private readonly IOrderPaymentService orderPaymentService;
    private readonly IErrorFactory errorFactory;

    #endregion

    #region Constractor
    public CustomerApiController(ICustomerFactory factory,
                                 ICartService cartService,
                                 IComplexIdBundlerService bundlers,
                                 ICommonService commonService,
                                 IProductService productService,
                                 IOrderPaymentService orderPaymentService,
                                 IErrorFactory errorFactory)
    {
      this.factory = factory;
      this.cartService = cartService;
      this.commonService = commonService;
      this.complexIdBundlers = bundlers;
      this.productService = productService;
      this.orderPaymentService = orderPaymentService;
      this.errorFactory = errorFactory;
    }
    #endregion

    #region Cart
    [HttpGet("customer/cart")]
    public async Task<CartModel> GetShoppingCart(CancellationToken cancellationToken)
    {
      return await factory.PrepareShoppingCartModel(cancellationToken: cancellationToken);
    }
    [HttpGet("customer/cart/brief")]
    public async Task<BriefCartModel> GetShoppingCartBrief(CancellationToken cancellationToken)
    {
      return await factory.PrepareShoppingBriefCartModel(cancellationToken: cancellationToken);
    }

    [HttpPatch("customer/cart/address")]
    public async Task EditCartAddress([FromBody] EditCartAddressModel updateCartAddressModel, CancellationToken cancellationToken)
    {
      var cart = await cartService.GetCart(cancellationToken: cancellationToken);
      cart = this.mapUpdateCartAddressModel(cart: cart, updateCartAddressModel: updateCartAddressModel);
      await cartService.UpdateCartAddress(cart: cart, cancellationToken: cancellationToken);
    }

    [HttpGet("customer/cart/bill")]
    public async Task<CartBillModel> GetCartBill([FromQuery] CalculateCartBillModel model, CancellationToken cancellationToken)
    {
      var coupon = await commonService.GetCouponByCode(couponCode: model.CouponCode, cancellationToken: cancellationToken);
      return await cartService.GetCartBill(coupon: coupon, cancellationToken: cancellationToken);
    }
    #endregion

    #region CartCoupon
    [HttpGet("customer/cart/coupon/{couponCode}")]
    public async Task<ShoppingCouponModel> CouponValidate([FromRoute] string couponCode, CancellationToken cancellationToken)
    {

      var coupon = await commonService.GetCouponByCode(couponCode: couponCode, cancellationToken: cancellationToken);

      if (coupon == null)
        throw errorFactory.ResourceNotFound(couponCode);

      return await factory.ValidateCoupon(coupon: coupon, cancellationToken: cancellationToken);

    }
    #endregion

    #region CartItem
    [HttpPost("customer/cart/item")]
    public async Task CreateShoppingCartItem([FromBody] ShoppingCartItemModel item, CancellationToken cancellationToken)
    {

      var cartItem = mapCartItem(model: item);
      await cartService.CreateOrUpdateCartItem(cartItem: cartItem,
                                       cancellationToken: cancellationToken);
    }

    [HttpPatch("customer/cart/item/{id}")]
    public async Task EditCartItemAmount([FromRoute] int id, [FromBody] ShoppingCartItemAmountModel model, CancellationToken cancellationToken)
    {
      var cartItem = await cartService.GetCurrentCartItemById(id: id,
                                                       cancellationToken: cancellationToken);

      if (cartItem == null)
        throw errorFactory.ResourceNotFound(id: id);

      cartItem = this.mapShoppingCartItemAmountModel(model: model, cartItem: cartItem);

      await cartService.EditCartItem(cartItem: cartItem,
                                       cancellationToken: cancellationToken);
    }
    [HttpDelete("customer/cart/item/{id}")]
    public async Task RemoveShoppingCartItem([FromRoute] int id, CancellationToken cancellationToken)
    {
      var cartItem = await cartService.GetCurrentCartItemById(id: id,
                                                       cancellationToken: cancellationToken);

      if (cartItem == null)
        throw errorFactory.ResourceNotFound(id: id);

      await cartService.DeleteCartItem(cartItem: cartItem,
                                       cancellationToken: cancellationToken);
    }
    #endregion

    #region Shopping
    [HttpGet("shoppings")]
    public async Task<IPagedList<ShoppingModel>> GetPagedShoppings([FromQuery] ShoppingSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareShoppingPagedListModel(parameters: parameters,
                                                         cancellationToken: cancellationToken);
    }
    [HttpGet("shoppings/{shoppingId}")]
    public async Task<ShoppingModel> GetShopping([FromRoute] int shoppingId, CancellationToken cancellationToken)
    {
      return await factory.PrepareShoppingModel(shoppingId: shoppingId,
                                                cancellationToken: cancellationToken);
    }

    [HttpGet("shopping/checkout/{orderId}")]
    public async Task<OrderCheckoutModel> PrepareOrderSummary([FromRoute] int orderId, CancellationToken cancellationToken)
    {
      return await factory.PrepareOrderSummary(orderId: orderId, cancellationToken: cancellationToken);
    }

    [HttpGet("shopping/order-payment-status/{orderPaymentId}")]
    public async Task<OrderPaymentStatusModel> GetOrderPaymentStatus([FromRoute] int orderPaymentId, CancellationToken cancellationToken)
    {
      return await factory.PrepareOrderPaymentStatus(orderPaymentId: orderPaymentId, cancellationToken: cancellationToken);
    }

    #endregion

    #region Financial Documents
    [HttpGet("customer/financial/documents")]
    public Task<PagedList<CustomerDocumentModel>> GetPagedDocuments([FromQuery] CustomerDocumentsSearchParameters parameters, CancellationToken cancellationToken)
    {
      // var user = workContext.GetCurrentUser();
      // return aw factory.PrepareCustomerDocumentPagedListModel(user.Id, parameters);
      // return Task<PagedList<CustomerDocumentModel>>.CompletedTask;
      return null;
    }
    #endregion

    #region Privates

    private CartItem mapShoppingCartItemAmountModel(ShoppingCartItemAmountModel model, CartItem cartItem)
    {
      cartItem.Amount = model.Amount;
      cartItem.RowVersion = model.RowVersion;
      return cartItem;
    }
    private CartItem mapCartItem(ShoppingCartItemModel model)
    {
      var cartItem = new CartItem();
      cartItem.ProductId = model.ProductId;
      cartItem.Amount = model.Amount;
      cartItem.ColorId = model.ColorId;
      return cartItem;
    }
    private Cart mapUpdateCartAddressModel(Cart cart, EditCartAddressModel updateCartAddressModel)
    {
      cart.ProfileAddressId = updateCartAddressModel.UserAddressId;
      cart.RowVersion = updateCartAddressModel.RowVersion;
      return cart;
    }
    #endregion
  }
}