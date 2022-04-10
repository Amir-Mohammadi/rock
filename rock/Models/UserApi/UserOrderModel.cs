using rock.Models.CommonApi;
using rock.Models.ProductApi;
using rock.Core.Domains.Orders;
namespace rock.Models.UserApi
{
  public class UserOrderModel
  {
    public int Id { get; set; }
    public ProductImageModel PreviewProductImage { get; set; }
    public string ProductName { get; set; }
    public string ProductCategoryName { get; set; }
    public string BrandName { get; set; }
    public ColorModel OrderedColor { get; set; }
    public string ProductPrice { get; set; }
    public OrderItemStatusType Status { get; set; }
    public string City { get; set; }
    public byte[] RowVersion { get; set; }
  }
}