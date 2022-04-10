using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Products;
using rock.Core.Services.Orders;
using rock.Models.OrderApi;
using rock.Models.ProductApi;
namespace rock.Factories
{
  public class OrderFactory : BaseFactory, IOrderFactory
  {
    private readonly IOrderService orderService;
    private readonly IOrderItemService orderItemService;
    public OrderFactory(IOrderService orderService, IOrderItemService orderItemService)
    {
      this.orderService = orderService;
      this.orderItemService = orderItemService;
    }
    public async Task<GetOrderModel> PrepareOrderModel(int orderId, CancellationToken cancellationToken)
    {
      var order = await this.orderService.GetOrderById(id: orderId, cancellationToken: cancellationToken);
      return this.createOrderModel(order);
    }
    public async Task<IList<OrderItemModel>> PrepareOrderItemModel(int orderId, CancellationToken cancellationToken)
    {
      var orderItems = this.orderItemService.GetOrderItems(orderId: orderId,
                                                           include: new Include<OrderItem>(query =>
                                                            {
                                                              query = query
                                                                      .Include(x => x.CartItem.Product).ThenInclude(x => x.Brand)
                                                                      .Include(x => x.CartItem.Product.ProductCategory)
                                                                      .Include(x => x.CartItem.Product.PreviewProductImage)
                                                                      .Include(x => x.CartItem.ProductPrice)
                                                                      .Include(x => x.CartItem.StuffPrice.Shop)
                                                                      .Include(x => x.CartItem.ProductColor.Color);
                                                              return query;
                                                            }));

      return await this.CreateModelListAsync(source: orderItems,
                                             convertFunction: this.createOrderItemModel,
                                             cancellationToken: cancellationToken);
      //return this.createOrderModel(order);
    }
    public async Task<OrderDetailModel> PrepareOrderDetailModel(int orderId, CancellationToken cancellationToken)
    {
      var order = await this.orderService.GetOrderById(id: orderId,
                                                       cancellationToken: cancellationToken,
                                                       include: new Include<Order>(query =>
                                                        {
                                                          query = query
                                                                  .Include(x => x.Cart.User.Profile).ThenInclude(x => x.PersonProfile)
                                                                  .Include(x => x.Cart.ProfileAddress.City).ThenInclude(x => x.Province);
                                                          return query;
                                                        }));
      return this.createOrderDetailModel(order);
    }
    public async Task<IPagedList<GetOrderModel>> PrepareOrderListModel(OrderSearchParameters parameters, CancellationToken cancellationToken)
    {
      var orders = this.orderService.GetOrders(include: new Include<Order>(query =>
                                                {
                                                  query = query
                                                          .Include(x => x.Cart.User.Profile.PersonProfile)
                                                          .Include(x => x.Cart.Items).ThenInclude(x => x.ProductPrice);
                                                  return query;
                                                }));

      return await this.CreateModelPagedListAsync(source: orders,
                                                  convertFunction: this.createOrderModel,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  cancellationToken: cancellationToken);
    }
    private OrderItemModel createOrderItemModel(OrderItem item)
    {
      if (item == null)
        return null;
      return new OrderItemModel()
      {
        Id = item.Id,
        ProductId = item.CartItem.ProductId,
        BrandName = item.CartItem.Product.Brand.Name,
        PreviewProductImage = this.ProductImageToModel(item.CartItem.Product.PreviewProductImage),
        ProductCategoryName = item.CartItem.Product.ProductCategory.Name,
        ProviderName = item.CartItem.StuffPrice.Shop.Name,
        ProductColor = item.CartItem.ProductColor.Color.Name,
        ProductName = item.CartItem.Product.Name,
        ProductPrice = item.CartItem.ProductPrice.Price,
        RowVersion = item.RowVersion,
      };
    }
    private ProductImageModel ProductImageToModel(ProductImage image)
    {
      return new ProductImageModel
      {
        Id = image.Id,
        ImageAlt = image.ImageAlt,
        ImageId = image.ImageId,
        ImageTitle = image.ImageTitle,
        Order = image.Order,
        RowVersion = image.RowVersion
      };
    }
    private OrderDetailModel createOrderDetailModel(Order order)
    {
      if (order == null)
        return null;
      return new OrderDetailModel()
      {
        Id = order.Id,
        CustomerName = order.Cart.User.Profile.PersonProfile.FirstName + order.Cart.User.Profile.PersonProfile.LastName,
        CustomerEmail = order.Cart.User.Profile.Email,
        Address = order.Cart.ProfileAddress.Description,
        City = order.Cart.ProfileAddress.City.Name,
        PostalCode = order.Cart.ProfileAddress.PostalCode,
        Province = order.Cart.ProfileAddress.City.Province.Name,
        PaymentTrackingCode = "0",
        PhoneNumber = order.Cart.User.Profile.Phone,
        TelNumber = order.Cart.ProfileAddress.Phone,
        RowVersion = order.RowVersion,
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt
      };
    }
    private GetOrderModel createOrderModel(Order order)
    {
      if (order == null)
        return null;
      return new GetOrderModel
      {
        Id = order.Id,
        CustomerName = order.Cart.User.Profile.PersonProfile.FirstName + order.Cart.User.Profile.PersonProfile.LastName,
        CustomerEmail = order.Cart.User.Profile.Email,
        TotalPrice = order.Cart.Items.Sum(x => (x.ProductPrice.Price - ((x.ProductPrice.Price * x.ProductPrice.Discount) / 100)) * x.Amount),
        RowVersion = order.RowVersion,
        CreatedAt = order.CreatedAt
      };
    }
  }
}