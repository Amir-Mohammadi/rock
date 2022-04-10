using System;
using rock.Models.FileApi;
namespace rock.Models.ProductApi
{
  public class ProductImageModel
  {
    public int Id { get; set; }
    public Guid ImageId { get; set; }
    public int Order { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}