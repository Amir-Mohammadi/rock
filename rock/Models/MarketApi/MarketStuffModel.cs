using System.Collections.Generic;
using rock.Models.CommonApi;
using rock.Models.ProductApi;
using rock.Models.FileApi;
namespace rock.Models.MarketApi
{
  public class MarketStuffModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string AltTitle { get; set; }
    public MarketStuffImageModel PreviewProductImage { get; set; }
    public byte[] RowVersion { get; set; }
    public MarketBrandModel Brand { get; set; }
    public MarketStuffCategoryModel ProductCategory { get; set; }    
    public int? DefaultColorId { get; set; }
    public bool LikedByMe { get; set; }
    public string BriefDescription { get; set; }
  }
}