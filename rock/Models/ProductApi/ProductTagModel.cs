using System;
using Microsoft.VisualBasic;
using rock.Models.UserApi;
namespace rock.Models.ProductApi
{
  public class ProductTagModel
  {
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
  }
}