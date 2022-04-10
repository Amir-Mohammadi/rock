using System;
using rock.Models.UserApi;

namespace rock.Models.ProductApi
{
  public class ProductAnswerModel
  {
    public int Id { get; set; }
    public string Answer { get; set; }
    public SimpleUserModel User { get; set; }
  }
}
