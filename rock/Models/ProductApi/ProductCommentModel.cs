using System;
using Microsoft.VisualBasic;
using rock.Models.UserApi;
namespace rock.Models.ProductApi
{
  public class ProductCommentModel
  {
    public int Id { get; set; }
    public SimpleUserModel Author { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public string ProductName { get; set; }
    public ProductImageModel PreviewProductImage { get; set; }
    public ProductColorModel DefaultProductColor { get; set; }
    public int? ProductId { get; set; }
    public string BrandName { get; set; }
  }
}