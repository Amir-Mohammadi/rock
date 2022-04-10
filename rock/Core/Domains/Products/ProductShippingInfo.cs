using System;
using rock.Framework.Models;

namespace rock.Core.Domains.Products
{
  public class ProductShippingInfo : IEntity, ITimestamp
  {
    public int ProductId { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual Product Product { get; set; }
  }
}