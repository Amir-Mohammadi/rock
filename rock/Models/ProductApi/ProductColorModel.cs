using System;
using rock.Models.CommonApi;
namespace rock.Models.ProductApi
{
  public class ProductColorModel
  {
    public int ProductId { get; internal set; }
    public int ColorId { get; set; }
    public ColorModel Color { get; internal set; }
  }
}