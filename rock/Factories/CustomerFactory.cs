using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Payment;
using rock.Core.Domains.Products;
using rock.Core.Domains.Profiles;
using rock.Core.Errors;
using rock.Core.Services.Common;
using rock.Core.Services.Orders;
using rock.Core.Services.Payment;
using rock.Framework.Setting;
using rock.Models.CommonApi;
using rock.Models.CustomerApi;
using rock.Models.MarketApi;
using rock.Models.UserApi;

namespace rock.Factories
{
  public class CustomerFactory : BaseFactory, ICustomerFactory
  {
    #region Fields
    private readonly ICartService cartService;
    private readonly IOrderPaymentService orderPaymentService;
    private readonly IOrderService orderService;
    private readonly IPaymentGatewayService paymentGatewayService;
    private readonly ICommonService commonService;
    private readonly IErrorFactory errorFactory;
    private readonly IOptionsSnapshot<SiteSettings> siteSettings;
    #endregion

    #region Constractor
    public CustomerFactory(ICartService cartService,
                           IOrderPaymentService orderPaymentService,
                           IOrderService orderService,
                           IPaymentGatewayService paymentGatewayService,
                           ICommonService commonService,
                           IErrorFactory errorFactory,
                           IOptionsSnapshot<SiteSettings> siteSettings)
    {
      this.cartService = cartService;
      this.orderPaymentService = orderPaymentService;
      this.orderService = orderService;
      this.paymentGatewayService = paymentGatewayService;
      this.commonService = commonService;
      this.errorFactory = errorFactory;
      this.siteSettings = siteSettings;
    }
    #endregion

    #region Cart
    public async Task<CartModel> PrepareShoppingCartModel(CancellationToken cancellationToken)
    {
      var cart = await cartService.GetCart(cancellationToken: cancellationToken,
                                                   include: new Include<Cart>(query =>
                                                   {
                                                     query.Include(x => x.ProfileAddress);
                                                     query = query.Include(x => x.Items);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.Brand);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.PreviewProductImage).ThenInclude(x => x.Image);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.Color);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice).ThenInclude(x => x.City).ThenInclude(x => x.Province);
                                                     return query;
                                                   }));


      var cartBill = await cartService.GetCartBill(coupon: null, cancellationToken: cancellationToken);


      return createShoppinCartModel(cart: cart, cartBill: cartBill);
    }

    public async Task<BriefCartModel> PrepareShoppingBriefCartModel(CancellationToken cancellationToken)
    {
      var cart = await cartService.GetCart(cancellationToken: cancellationToken,
                                                   include: new Include<Cart>(query =>
                                                   {
                                                     query.Include(x => x.ProfileAddress);
                                                     query = query.Include(x => x.Items);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.Brand);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.PreviewProductImage).ThenInclude(x => x.Image);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.Color);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice);
                                                     query = query.Include(x => x.Items).ThenInclude(x => x.ProductPrice).ThenInclude(x => x.City).ThenInclude(x => x.Province);
                                                     return query;
                                                   }));



      return createShoppinBriefCartModel(cart: cart);
    }
    private BriefCartModel createShoppinBriefCartModel(Cart cart)
    {
      if (cart == null)
        return null;
      return new BriefCartModel()
      {
        Id = cart.Id,
        CartItems = createBriefCartItemModel(cartItems: cart.Items),
        TotalPrice = calculateTotalPriceFromCartItem(cartItems: cart.Items)
      };
    }
    private CartModel createShoppinCartModel(Cart cart, CartBillModel cartBill)
    {
      if (cart == null)
        return null;
      return new CartModel()
      {
        Id = cart.Id,
        CreatedAt = cart.CreatedAt,
        UpdatedAt = cart.UpdatedAt,
        UserAddressId = cart.ProfileAddressId,
        CartBill = cartBill,
        CartItems = createCartItemModel(cartItems: cart.Items),
        RowVersion = cart.RowVersion
      };
    }

    private int calculateTotalPriceFromCartItem(ICollection<CartItem> cartItems)
    {
      var prices = cartItems.Select(cartItem => new
      {
        ProductPrice = (int)(cartItem.ProductPrice.Price - (cartItem.ProductPrice.Price * cartItem.ProductPrice.Discount) / 100),
      });

      return prices.Sum(x => x.ProductPrice);
    }
    private IList<BriefCartItemModel> createBriefCartItemModel(ICollection<CartItem> cartItems)
    {
      if (cartItems == null)
        return null;

      return cartItems.Select(cartItem => new BriefCartItemModel
      {
        Id = cartItem.Id,
        Amount = cartItem.Amount,
        ProductId = cartItem.ProductId,
        ProductName = cartItem.Product.Name,
        MetaDescription = cartItem.Product.MetaDescription,
        ProductPrice = (int)(cartItem.ProductPrice.Price - (cartItem.ProductPrice.Price * cartItem.ProductPrice.Discount) / 100),
        PreviewImage = createShoppingProductPerviewImageModel(cartItem.Product.PreviewProductImage)
      }).ToList();
    }
    private IList<CartItemModel> createCartItemModel(ICollection<CartItem> cartItems)
    {
      if (cartItems == null)
        return null;

      return cartItems.Select(cartItem => new CartItemModel
      {
        Id = cartItem.Id,
        Amount = cartItem.Amount,
        Product = createShoppingProductModel(cartItem.Product, cartItem.Color, cartItem.ProductPrice),
        IsAvailableShipping = true,
        RowVersion = cartItem.RowVersion
      }).ToList();
    }

    private ShoppingProductModel createShoppingProductModel(Product product, Color color, ProductPrice productPrice)
    {
      if (product == null)
        return null;
      return new ShoppingProductModel()
      {
        ProductId = product.Id,
        ProductName = product.Name,
        ProductBrand = createShoppingProductBrandModel(product.Brand),
        ProductColor = createShoppingProductColorModel(color),
        ProductPreviewImage = createShoppingProductPerviewImageModel(product.PreviewProductImage),
        ProductPrice = createShoppingProductPriceModel(productPrice),
        RowVersion = product.RowVersion
      };
    }
    private ShoppingProductPriceModel createShoppingProductPriceModel(ProductPrice price)
    {
      if (price == null)
        return null;
      return new ShoppingProductPriceModel()
      {
        Id = price.Id,
        Price = price.Price,
        Discount = price.Discount,
        City = mapShoppingCartCityModel(price.City)
      };
    }

    private ShoppingCartCityModel mapShoppingCartCityModel(City city)
    {
      if (city == null)
        return null;
      return new ShoppingCartCityModel()
      {
        Id = city.Id,
        ProvinceId = city.ProvinceId,
        CityName = city.Name,
        ProvinceName = city.Province.Name
      };
    }
    private ShoppingProductPreviewImageModel createShoppingProductPerviewImageModel(ProductImage productImage)
    {
      if (productImage == null)
        return null;
      return new ShoppingProductPreviewImageModel()
      {
        ImageId = productImage.ImageId,
        ImageAlt = productImage.ImageAlt,
        ImageTitle = productImage.ImageTitle,
        ImageRowVersion = productImage.Image.RowVersion,
      };
    }
    private ShoppingProductColorModel createShoppingProductColorModel(Color color)
    {
      if (color == null)
        return null;
      return new ShoppingProductColorModel()
      {
        Id = color.Id,
        ColorName = color.Name,
        ColorCode = color.Code
      };
    }
    private ShoppingProductBrandModel createShoppingProductBrandModel(Brand brand)
    {
      if (brand == null)
        return null;
      return new ShoppingProductBrandModel()
      {
        Id = brand.Id,
        BrandName = brand.Name
      };
    }
    private UserAddressModel createUserAddressModel(ProfileAddress profileAddress)
    {
      if (profileAddress == null)
        return null;
      return new UserAddressModel()
      {
        Id = profileAddress.Id,
        City = this.createCityModel(profileAddress.City),
        Description = profileAddress.Description,
        PostalCode = profileAddress.PostalCode,
        Phone = profileAddress.Phone,
        AddressOwnerName = profileAddress.AddressOwnerName,
        IsDefault = profileAddress.IsDefault,
        RowVersion = profileAddress.RowVersion
      };
    }

    private IList<MarketStuffModel> createMarketStuffModel(ICollection<CartItem> cartItems)
    {
      if (cartItems == null)
        return null;

      var result = from cartItem in cartItems
                   select new MarketStuffModel
                   {
                     Id = cartItem.ProductId,
                     Name = cartItem.Product.Name,
                     AltTitle = cartItem.Product.PreviewProductImage.ImageAlt,
                   };
      return result.ToList();
    }
    #endregion

    #region Coupon
    public async Task<ShoppingCouponModel> ValidateCoupon(Coupon coupon, CancellationToken cancellationToken)
    {

      var couponStatus = await orderService.DefineCouponStatus(coupon: coupon, cancellationToken: cancellationToken);
      return mapCoupon(coupon: coupon, status: couponStatus);
    }

    private ShoppingCouponModel mapCoupon(Coupon coupon, CouponValidateStatus status)
    {
      if (coupon == null)
        return null;
      return new ShoppingCouponModel
      {
        Id = coupon.Id,
        Value = coupon.Value,
        Code = coupon.CouponCode,
        Status = status
      };
    }
    #endregion
    #region Shipping 
    public Task<ShoppingModel> PrepareShoppingModel(int shoppingId, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    public Task<IPagedList<ShoppingModel>> PrepareShoppingPagedListModel(ShoppingSearchParameters parameters, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region Document
    public Task<IPagedList<CustomerDocumentModel>> PrepareCustomerDocumentPagedListModel(CustomerDocumentsSearchParameters parameters, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    public Task<CustomerWalletModel> PrepareCustomerWalletModel(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region  privates
    public async Task<OrderCheckoutModel> PrepareOrderSummary(int orderId, CancellationToken cancellationToken)
    {
      var order = await orderService.GetOrderById(id: orderId,
                                                  cancellationToken: cancellationToken,
                                                  include: new Include<Order>(query =>
                                                  {
                                                    query = query.Include(x => x.Cart);
                                                    query = query.Include(x => x.Cart).ThenInclude(x => x.Items);
                                                    query = query.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Product);
                                                    query = query.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Product).ThenInclude(x => x.Brand);
                                                    query = query.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Color);
                                                    query = query.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.ProductPrice);
                                                    query = query.Include(x => x.Coupon);
                                                    query = query.Include(x => x.OrderPayments);
                                                    query = query.Include(x => x.OrderPayments).ThenInclude(x => x.BankTransaction);
                                                    query = query.Include(x => x.Transports);
                                                    return query;
                                                  }));
      return this.createOrderCheckoutModel(order: order);
    }

    private OrderCheckoutModel createOrderCheckoutModel(Order order)
    {
      if (order == null)
        return null;
      return new OrderCheckoutModel
      {
        BankBills = this.createOrderCheckoutBankBillModel(order.OrderPayments),
        Cargos = this.createOrderCheckoutCargoModel(order.Cart.Items),
        Facture = this.creatOrderCheckoutFactureModel(order)
      };
    }
    private IList<OrderCheckoutCargoModel> createOrderCheckoutCargoModel(ICollection<CartItem> cartItems)
    {
      if (cartItems == null)
        return null;

      return cartItems.Select(cartItem => new OrderCheckoutCargoModel
      {
        ProductName = cartItem.Product.Name,
        ShippingDateTime = null,
        ProductBrandName = cartItem.Product.Brand.Name,
        ProductColorName = cartItem.Color.Name,
        Price = cartItem.ProductPrice.Price,
        Discount = cartItem.ProductPrice.Discount,
        amount = cartItem.Amount
      }).ToList();
    }
    private IList<OrderCheckoutBankBillModel> createOrderCheckoutBankBillModel(ICollection<OrderPayment> orderPayments)
    {
      if (orderPayments == null)
        return null;
      return orderPayments.Select(orderPayment => new OrderCheckoutBankBillModel
      {
        BillDateTime = orderPayment.CreatedAt,
        RefNum = orderPayment.BankTransaction.BankParameter4,
        OrderNum = orderPayment.OrderId.ToString(),
        PaymentNum = orderPayment.Id.ToString()
      }).ToList();
    }
    private OrderCheckoutFactureModel creatOrderCheckoutFactureModel(Order order)
    {
      if (order == null)
        return null;

      var discountPrice = order.Coupon == null ? 0 : (order.TotalAmount * order.Coupon.Value) / 100;
      var taxPrice = (order.TotalAmount * order.Tax) / 100;
      return new OrderCheckoutFactureModel
      {
        TotalPrice = order.TotalAmount,
        DiscountPrice = discountPrice,
        ShippingPrice = order.Transports.Sum(x => x.Cost),
        TaxPrice = taxPrice
      };
    }

    public async Task<OrderPaymentStatusModel> PrepareOrderPaymentStatus(int orderPaymentId, CancellationToken cancellationToken)
    {
      var orderPayment = await orderPaymentService.GetOrderPaymentById(id: orderPaymentId,
                                                cancellationToken: cancellationToken,
                                                include: null);

      return this.createOrderPaymentStatusModel(orderPayment: orderPayment);
    }

    private OrderPaymentStatusModel createOrderPaymentStatusModel(OrderPayment orderPayment)
    {
      if (orderPayment == null)
        return null;
      return new OrderPaymentStatusModel
      {
        OrderId = orderPayment.OrderId,
        OrderPaymentId = orderPayment.Id,
        Status = orderPayment.PaymentStatus
      };
    }
    #endregion
  }
}