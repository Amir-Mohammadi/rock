using System;

namespace rock.Models.CustomerApi
{
  public class ShoppingProductPreviewImageModel
  {
    public Guid ImageId { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public byte[] ImageRowVersion { get; set; }
  }
}