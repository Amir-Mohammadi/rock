using System;
using rock.Models.UserApi;
namespace rock.Models.ProductApi
{
  public class ProductQuestionModel
  {
    public int Id { get; set; }
    public string Question { get; set; }
   public SimpleUserModel User { get; set; }
  }
}