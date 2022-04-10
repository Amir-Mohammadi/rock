using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Orders;
using rock.Models.ProductApi;
using rock.Core.Domains.Shops;

namespace rock.Models.OrderApi
{
  public class OrderItemModel
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductImageModel PreviewProductImage { get; set; }
    public string ProductName { get; set; }
    public string ProductCategoryName { get; set; }
    public string BrandName { get; set; }
    public string ProviderName { get; set; }
    public string ProductColor { get; set; }
    public double ProductPrice { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
