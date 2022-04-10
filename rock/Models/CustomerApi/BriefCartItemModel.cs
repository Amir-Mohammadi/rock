using System;

namespace rock.Models.CustomerApi
{
  public class BriefCartItemModel
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string MetaDescription { get; set; }
    public int Amount { get; set; }
    public int ProductPrice { get; set; }
    public ShoppingProductPreviewImageModel PreviewImage { get; set; }
  }
}