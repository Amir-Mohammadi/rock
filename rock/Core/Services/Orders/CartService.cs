using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using rock.Core.Data;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Payment;
using rock.Core.Errors;
using rock.Core.Services.Common;
using rock.Core.Services.Financial;
using rock.Core.Services.Payment;
using rock.Core.Services.Products;
using rock.Core.Services.Profiles;
using rock.Core.Services.Users;
using rock.Framework.Setting;
using rock.Models.CustomerApi;

namespace rock.Core.Services.Orders
{
  public class CartService : ICartService
  {
    #region Fields
    private readonly IRepository<Cart> cartRepository;
    private readonly IRepository<Coupon> couponRepository;
    private readonly IRepository<CartItem> cartItemRepository;
    private readonly IOrderService orderService;
    private readonly IOrderItemService orderItemService;
    private readonly ICommonService commonService;
    private readonly IOptionsSnapshot<SiteSettings> siteSetting;
    private readonly IOrderPaymentService orderPaymentService;
    private readonly IWorkContext workContext;
    private readonly IBankTransactionService bankTransactionService;
    private readonly IErrorFactory errorFactory;
    private readonly IProductService productService;
    private readonly IProfileAddressService profileAddressService;
    private readonly IUserService userService;
    private readonly IOrderItemStatusService orderItemStatusService;
    private readonly IPaymentGatewayService paymentGatewayService;
    private readonly IFinancialService financialService;
    private readonly IFinancialAccountService financialAccountService;
    private readonly IOrderStatusService orderStatusService;
    private readonly ITransportService transportService;
    #endregion
    #region Constractor
    public CartService(IRepository<Cart> cartRepository,
                       IRepository<Coupon> couponRepository,
                       IRepository<CartItem> cartItemRepository,
                       IWorkContext workContext,
                       IOrderService orderService,
                       IOrderItemService orderItemService,
                       ICommonService commonService,
                       IOptionsSnapshot<SiteSettings> siteSetting,
                       IOrderPaymentService orderPaymentService,
                       IBankTransactionService bankTransactionService,
                       IErrorFactory errorFactory,
                       IProductService productService,
                       IProfileAddressService profileAddressService,
                       IUserService userService,
                       IOrderItemStatusService orderItemStatusService,
                       IPaymentGatewayService paymentGatewayService,
                       IFinancialService financialService,
                       IFinancialAccountService financialAccountService,
                       IOrderStatusService orderStatusService,
                       ITransportService transportService)
    {
      this.cartRepository = cartRepository;
      this.couponRepository = couponRepository;
      this.cartItemRepository = cartItemRepository;
      this.workContext = workContext;
      this.orderService = orderService;
      this.orderItemService = orderItemService;
      this.commonService = commonService;
      this.siteSetting = siteSetting;
      this.orderPaymentService = orderPaymentService;
      this.bankTransactionService = bankTransactionService;
      this.errorFactory = errorFactory;
      this.productService = productService;
      this.profileAddressService = profileAddressService;
      this.userService = userService;
      this.orderItemStatusService = orderItemStatusService;
      this.paymentGatewayService = paymentGatewayService;
      this.financialService = financialService;
      this.financialAccountService = financialAccountService;
      this.orderStatusService = orderStatusService;
      this.transportService = transportService;
    }
    #endregion
    #region Cart

    public async Task<CartBillModel> GetCartBill(Coupon coupon, CancellationToken cancellationToken)
    {

      var cart = await GetCart(cancellationToken: cancellationToken,
                              include: new Include<Cart>(query =>
                              {
                                query = query.Include(x => x.ProfileAddress);
                                query = query.Include(x => x.Items);
                                query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice);
                                query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice).ThenInclude(x => x.City);
                                return query;
                              }));

      if (cart == null)
      {
        return new CartBillModel
        {
          TotalPrice = 0,
          TotalDiscountPrice = 0,
          TotalShippingPrice = 0,
          TotalTaxPrice = 0

        };
      }
      double discountPrice = 0;
      double taxPrice = 0;


      #region Calculate product Total Price
      var totalPrice = cart.Items.Select(m => new { TotalAmount = (m.ProductPrice.Price - ((m.ProductPrice.Price * m.ProductPrice.Discount) / 100)) * m.Amount })
        .Sum(m => m.TotalAmount);
      #endregion

      #region  Apply Coupon
      if (coupon != null)
      {
        discountPrice = (totalPrice * coupon.Value) / 100;
      }
      #endregion

      #region  Get Tax
      var tax = siteSetting.Value.TaxSettings.TaxAmount;
      taxPrice = ((totalPrice - discountPrice) * tax) / 100;
      #endregion

      #region  Calculate Shipping Price
      var shippingCost = await CalculateOrderShippingCost(cancellationToken: cancellationToken);
      #endregion

      #region  Result
      return new CartBillModel
      {
        TotalPrice = (int)totalPrice,
        TotalDiscountPrice = (int)discountPrice,
        TotalShippingPrice = (int)shippingCost,
        TotalTaxPrice = (int)taxPrice

      };
      #endregion
    }
    public async Task ArchiveCart(Cart cart, CancellationToken cancellationToken)
    {
      cart.DeletedAt = DateTime.UtcNow;
      await cartRepository.UpdateAsync(entity: cart,
                                       cancellationToken: cancellationToken);
    }
    public async Task UpdateCartAddress(Cart cart, CancellationToken cancellationToken)
    {
      await cartRepository.UpdateAsync(entity: cart, cancellationToken: cancellationToken);
    }
    public async Task<Cart> GetOrCreateCart(CancellationToken cancellationToken, IInclude<Cart> include = null)
    {
      var cart = await GetCart(cancellationToken, include);
      if (cart == null)
      {
        var currentUserId = this.workContext.GetCurrentUserId();

        var user = await this.userService.GetUserById(id: currentUserId, cancellationToken);

        var address = await this.profileAddressService.GetDefaultOrFirstProfileAddressByUserId(profileId: user.ProfileId,
                                                                                                       cancellationToken: cancellationToken);
        cart = new Cart();
        cart.UserId = currentUserId;
        cart.CreatedAt = DateTime.UtcNow;
        cart.ProfileAddress = address;

        await cartRepository.AddAsync(entity: cart,
                                      cancellationToken: cancellationToken);
      }
      return cart;
    }
    public async Task<Cart> GetCart(CancellationToken cancellationToken, IInclude<Cart> include = null)
    {
      var currentUserId = this.workContext.GetCurrentUserId();
      var cart = await cartRepository.GetAsync(predicate: x => x.UserId == currentUserId && x.CartStatus == CartStatus.Temporary,
                                               cancellationToken: cancellationToken,
                                               include: include);
      return cart;
    }

    public async Task<List<int>> GetNotAvailableCartItemShippings(CancellationToken cancellationToken)
    {
      //This function worked on our based designed transportation 
      //later this function can be usable
      var cart = await GetCart(cancellationToken: cancellationToken,
                        include: new Include<Cart>(query =>
                        {
                          query = query.Include(x => x.ProfileAddress);
                          query = query.Include(x => x.Items);
                          query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice);
                          query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice).ThenInclude(x => x.City);
                          return query;
                        }));

      var notAvailableCartItems = new List<int>();
      if (cart == null || cart.ProfileAddress == null)
        return notAvailableCartItems;

      #region Get And Check Transportation
      var fromCityIds = cart.Items.Select(x => x.ProductPrice.CityId).Distinct().ToArray();
      var toCityId = cart.ProfileAddress.CityId;

      var transportations = await commonService.GetTransportations(fromCityIds: fromCityIds,
                                                             toCityId: toCityId)
                                          .ToListAsync(cancellationToken: cancellationToken);


      var cartItemTransportations = from cartItem in cart.Items
                                    join transportation in transportations on cartItem.ProductPrice.CityId equals transportation.FromCityId into temp
                                    from t in temp.DefaultIfEmpty()
                                    select new
                                    {
                                      CartItemId = cartItem.Id,
                                      TransportationId = t?.Id ?? null
                                    };

      return cartItemTransportations.Where(x => x.TransportationId == null).Select(x => x.CartItemId).ToList();
      #endregion



    }

    public async Task<double> CalculateOrderShippingCost(CancellationToken cancellationToken)
    {
      var cart = await GetCart(cancellationToken: cancellationToken,
                        include: new Include<Cart>(query =>
                        {
                          query = query.Include(x => x.ProfileAddress);
                          query = query.Include(x => x.ProfileAddress).ThenInclude(x => x.City);
                          query = query.Include(x => x.Items);
                          query = query.Include(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.ProductShippingInfo);
                          query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice);
                          query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice).ThenInclude(x => x.City);
                          return query;
                        }));


      var totalShippingCost = 0;
      if (cart == null || cart.ProfileAddress == null)
        return totalShippingCost;



      var groupCartItems = from cartItem in cart.Items
                           group cartItem by new { cartItem.ProductPrice.CityId } into g
                           select new
                           {
                             FromCityId = g.Key.CityId,
                             CartItems = g
                           };

      foreach (var item in groupCartItems)
      {
        var cartItems = item.CartItems.ToList();
        var transportCost = await transportService.CalculateShippingPrice(cartItems: cartItems,
                                                                          profileAddress: cart.ProfileAddress,
                                                                          transportType: TransportType.Tipax,
                                                                          cancellationToken: cancellationToken);
        totalShippingCost += transportCost;
      }

      return totalShippingCost;
    }
    #endregion

    #region CartItems
    public async Task<CartItem> GetCurrentCartItemById(int id, CancellationToken cancellationToken, IInclude<CartItem> include = null)
    {
      return await cartItemRepository.GetAsync(predicate: x => x.Id == id &&
                                               x.Cart.UserId == workContext.GetCurrentUserId() &&
                                               x.Cart.CartStatus == CartStatus.Temporary,
                                              cancellationToken: cancellationToken,
                                              include: include);
    }
    public async Task<CartItem> GetCartItemById(int id, CancellationToken cancellationToken, IInclude<CartItem> include = null)
    {
      return await cartItemRepository.GetAsync(predicate: x => x.Id == id,
                                           cancellationToken: cancellationToken,
                                           include: include);
    }
    public async Task<CartItem> CreateOrUpdateCartItem(CartItem cartItem, CancellationToken cancellationToken)
    {
      var cart = await GetOrCreateCart(cancellationToken: cancellationToken,
                                       include: new Include<Cart>(query =>
                                       {
                                         query = query.Include(x => x.ProfileAddress);
                                         return query;
                                       }));


      var currentCartItem = await cartItemRepository.GetAsync(
        predicate: x => x.ProductId == cartItem.ProductId &&
                        x.ColorId == cartItem.ColorId &&
                        x.CartId == cart.Id,
        cancellationToken: cancellationToken,
        include: null);

      if (currentCartItem == null)
      {
        var productPrice = await productService.GetCurrentProductPrice(productId: cartItem.ProductId,
                                                                            colorId: cartItem.ColorId,
                                                                            cancellationToken: cancellationToken);
        if (productPrice == null)
        {
          throw errorFactory.InvalidProductPrice();
        }


        cartItem.ProductPriceId = productPrice.Id;
        cartItem.CartId = cart.Id;

        await cartItemRepository.AddAsync(entity: cartItem,
                                          cancellationToken: cancellationToken);
      }
      else
      {
        await cartItemRepository.LoadReferenceAsync(entity: currentCartItem, x => x.ProductPrice, cancellationToken: cancellationToken);
        currentCartItem.Amount += 1;
        await EditCartItem(cartItem: currentCartItem, cancellationToken: cancellationToken);
      }
      return cartItem;
    }
    public async Task EditCartItem(CartItem cartItem, CancellationToken cancellationToken)
    {
      await cartItemRepository.UpdateAsync(entity: cartItem,
                                        cancellationToken: cancellationToken);
    }
    public async Task DeleteCartItem(CartItem cartItem, CancellationToken cancellationToken)
    {
      await cartItemRepository.DeleteAsync(entity: cartItem,
                                           cancellationToken: cancellationToken);
    }
    public async Task DeleteCartItemsByCartId(int cartId, CancellationToken cancellationToken)
    {
      await cartItemRepository.DeleteAsync(predicate: x => x.CartId == cartId,
                                               cancellationToken: cancellationToken);
    }
    public IQueryable<Cart> GetCarts(int? userId = null, IInclude<Cart> include = null)
    {
      var query = cartRepository.GetQuery(include);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      return query;
    }
    public async Task<Facture> PrepareOrder(Dictionary<string, string> extraParams, CancellationToken cancellationToken)
    {
      #region  Get Cart
      var cart = await GetCart(cancellationToken: cancellationToken,
                               include: new Include<Cart>(query =>
                               {
                                 query = query.Include(x => x.ProfileAddress);
                                 query = query.Include(x => x.Items);
                                 query = query.Include(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.ProductShippingInfo);
                                 query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice);
                                 query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice).ThenInclude(x => x.City);
                                 return query;
                               }));
      #endregion

      #region Get CartItems
      var cartItems = cart.Items;
      #endregion


      #region Create Order
      var order = new Order();
      order.CartId = cart.Id;
      order.Expiration = DateTime.UtcNow.AddMinutes(siteSetting.Value.OrderSettings.Expiration);
      await orderService.InsertOrder(order: order, cancellationToken: cancellationToken);
      #endregion

      #region Create Transport
      var totalShippingCost = await CalculateOrderShippingCost(cancellationToken: cancellationToken);

      foreach (var cartItem in cartItems)
      {
        // #region  Create Transport 
        // var transportationCost = await transportService.CalculateTipaxShippingPirce(product: cartItem.Product, profileAddress: cart.ProfileAddress, cancellationToken: cancellationToken);
        // var transport = new Transport();
        // transport.OrderId = order.Id;
        // transport.FromCityId = cartItem.ProductPrice.CityId;
        // transport.ToCityId = cart.ProfileAddress.CityId;
        // transport.Cost = transportationCost;
        // await orderService.InsertTransport(transport: transport, cancellationToken: cancellationToken);
        // totalShippingCost += transportationCost;
        // #endregion

        #region Create Order Item
        var orderItem = new OrderItem();
        orderItem.OrderId = order.Id;
        orderItem.CartItemId = cartItem.Id;
        //  orderItem.TransportId = transport.Id;
        await orderItemService.InsertOrderItem(orderItem: orderItem, cancellationToken: cancellationToken);
        #endregion

        #region  Create Order Item Status
        var orderItemStatus = new OrderItemStatus();
        orderItemStatus.OrderItem = orderItem;
        orderItemStatus.Type = OrderItemStatusType.Created;
        await orderItemStatusService.InsertOrderItemStatus(orderItemStatus: orderItemStatus, cancellationToken: cancellationToken);
        #endregion

        #region  Update Order Item
        orderItem.LatestStatus = orderItemStatus;
        await orderItemService.UpdateOrderItem(orderItem: orderItem, cancellationToken: cancellationToken);
        #endregion
      }



      #endregion


      #region Calculate Total Price
      double discountPrice = 0;
      double taxPrice = 0;
      var totalPrice = cartItems.Select(m => new { TotalAmount = (m.ProductPrice.Price - ((m.ProductPrice.Price * m.ProductPrice.Discount) / 100)) * m.Amount })
        .Sum(m => m.TotalAmount);
      #endregion

      #region  Apply Coupon
      Coupon coupon = null;
      if (extraParams.ContainsKey("coupon_code"))
      {
        coupon = await commonService.GetCouponByCode(couponCode: extraParams["coupon_code"].ToString(), cancellationToken: cancellationToken);
        if (coupon != null)
          discountPrice = (totalPrice * coupon.Value) / 100;
      }
      #endregion

      #region  Get Tax
      var tax = siteSetting.Value.TaxSettings.TaxAmount;
      taxPrice = ((totalPrice - discountPrice) * tax) / 100;
      #endregion

      #region  Calculate Minimum Payable Amount

      double payableAmount = totalPrice - discountPrice + taxPrice + totalShippingCost;
      var requestedPayAmount = payableAmount;
      if (extraParams.ContainsKey("requested_pay"))
      {
        var requestedPayValue = extraParams["requested_pay"];
        double.TryParse(requestedPayValue, out requestedPayAmount);
      }

      var minimumPayAmount = await orderPaymentService.CalculateMinimumOrderPayment(orderId: order.Id, cancellationToken: cancellationToken);

      if (requestedPayAmount < minimumPayAmount)
      {
        throw errorFactory.LessThanMinimumOrderPayablePrice();
      }
      #endregion

      #region  Create OrderPayment
      var orderPayment = new Domains.Payment.OrderPayment();
      orderPayment.OrderId = order.Id;
      orderPayment.Amount = (int)(requestedPayAmount);
      orderPayment.PaymentStatus = PaymentStatus.Pending;
      await orderPaymentService.InsertOrderPayment(orderPayment: orderPayment, cancellationToken: cancellationToken);
      #endregion

      #region  Create Order Status
      var orderStatus = new OrderStatus();
      orderStatus.OrderStatusType = OrderStatusType.PaymentWaiting;
      orderStatus.OrderId = order.Id;
      await orderStatusService.InsertOrderStatus(orderStatus: orderStatus, cancellationToken: cancellationToken);
      #endregion

      #region  Update Order
      order.LatestOrderStatus = orderStatus;
      order.Coupon = coupon;
      order.TotalAmount = (int)totalPrice;
      order.Tax = tax;
      await orderService.UpdateOrder(order: order, cancellationToken: cancellationToken);
      #endregion

      #region  Update Cart
      cart.UpdatedAt = DateTime.UtcNow;
      cart.CartStatus = CartStatus.Permanent;
      await cartRepository.UpdateAsync(entity: cart, cancellationToken: cancellationToken);
      #endregion

      #region  Return Purchase Payment Info
      return new Facture(orderPayment.Id.ToString(), (int)orderPayment.Amount);
      #endregion
    }
    public Task<Facture> InitPayment(Dictionary<string, string> extraParams, CancellationToken cancellationToken)
    {
      return PrepareOrder(extraParams: extraParams, cancellationToken: cancellationToken);
    }
    public async Task HandlePayment(BankInfo bankParsedInfo, bool isBankConfirmationOk, CancellationToken cancellationToken)
    {
      var orderPayment = await orderPaymentService.GetOrderPaymentById(id: int.Parse(bankParsedInfo.OrderPaymentId),
                                                  cancellationToken: cancellationToken,
                                                  include: new Include<OrderPayment>(query =>
                                                  {
                                                    query = query.Include(x => x.Order); // CLEANUP
                                                    query = query.Include(x => x.Order).ThenInclude(x => x.Cart); // CLEANUP
                                                    query = query.Include(x => x.Order).ThenInclude(x => x.Cart).ThenInclude(x => x.User.Profile);
                                                    return query;
                                                  }));

      var bankTransaction = await bankTransactionService.GetBankTransactionByOrderPaymentId(orderId: bankParsedInfo.OrderPaymentId,
                                                                                         cancellationToken: cancellationToken);

      orderPayment.UpdatedAt = DateTime.UtcNow;
      orderPayment.Order.Cart.UpdatedAt = DateTime.UtcNow;
      orderPayment.BankTransactionId = bankTransaction.Id;

      if (isBankConfirmationOk)
      {
        // payment is ok and should change status and order payment id
        orderPayment.PaymentStatus = PaymentStatus.Done;
        //Update FinancialTransaction

        var paymentGateway = await paymentGatewayService.GetPaymentGateway(gateway: bankTransaction.PaymentGateway,
                                                                           cancellationToken: cancellationToken,
                                                                           include: new Include<PaymentGateway>(query =>
                                                                           {
                                                                             query = query.Include(x => x.FinancialAccount).ThenInclude(x => x.Currency);
                                                                             query = query.Include(x => x.FinancialAccount).ThenInclude(x => x.Bank);
                                                                             return query;
                                                                           }));

        var financialAccount = await financialAccountService.GetOrCreateFinancialAccount(profile: orderPayment.Order.Cart.User.Profile,
                                                                                         currency: paymentGateway.FinancialAccount.Currency,
                                                                                         financialAccountType: FinancialAccountType.Customer,
                                                                                         cancellationToken: cancellationToken);

        await financialService.CreateOnlinePaymentFinancialTransaction(debitFinancialAccountId: paymentGateway.FinancialAccountId,
                                                                       creditFinancialAccountId: financialAccount.Id,
                                                                       amount: orderPayment.Amount,
                                                                       userId: orderPayment.Order.Cart.UserId,
                                                                       cancellationToken: cancellationToken);
      }
      else
      {
        // reverse the order Item and order and related status
        orderPayment.PaymentStatus = PaymentStatus.Failed;
      }
      await orderPaymentService.UpdateOrderPayment(orderPayment: orderPayment, cancellationToken: cancellationToken);
      await cartRepository.UpdateAsync(entity: orderPayment.Order.Cart, cancellationToken: cancellationToken);
    }
    public string GetSuccessfullPaymentRedirectUrl(string orderPaymentId)
    {
      return "http://localhost:3000/check-payment?payment_id=" + orderPaymentId;
    }
    public string GetFailedPaymentRedirectUrl(string orderPaymentId)
    {
      return "http://localhost:3000/check-payment?payment_id=" + orderPaymentId;
    }
    #endregion


  }
}