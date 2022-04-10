using System;

namespace rock.Models.ProductApi
{
  public class EditProductImageModel
  {

    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
    public byte[] RowVersion { get; internal set; }
  }
}
