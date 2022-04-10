using rock.Models.ProductApi;
using rock.Models.CommonApi;
using rock.Core.Domains.Shops;
namespace rock.Models.OrderApi
{
  public class OrderCartItemModel
  {
    public int Id { get; set; }
    public int ShopStuffPriceId { get; set; }
    public int ProductPriceId { get; set; }
    public int CartId { get; set; }
    public double Amount { get; set; }
    public ProductModel Product { get; set; }
    public ShopStuffPrice StuffPrice { get; set; }
    public ProductPriceModel ProductPrice { get; set; }
    public ColorModel Color { get; set; }
    public byte[] RowVersion { get; set; }
  }
}