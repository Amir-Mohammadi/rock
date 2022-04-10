using System;
using System.Collections.Generic;
using rock.Core.Data.Mappings;
using rock.Core.Domains.Products;

namespace rock.Models.ProductApi
{
  public class ProductBrochureModel
  {
    public int Id { get; set; }
    public string HTML { get; set; }
    public IList<ProductBrochureAttachmentModel> Attachments { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
