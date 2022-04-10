using System.Collections.Generic;

namespace rock.Models.CustomerApi
{
  public class BriefCartModel
  {
    public int Id { get; set; }
    public int TotalPrice { get; set; }
    public IList<BriefCartItemModel> CartItems { get; set; }
  }
}