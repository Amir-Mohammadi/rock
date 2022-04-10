using System;
using rock.Core.Domains.Files;
using rock.Framework.Models;
namespace rock.Core.Domains.Products
{
  public class ProductImage : IEntity , IHasImage
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Order { get; set; }
    public Guid ImageId { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual File Image { get; set; }
    public virtual Product Product { get; set; }
  }
}