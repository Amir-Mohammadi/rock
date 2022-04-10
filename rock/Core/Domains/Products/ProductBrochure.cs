using System;
using System.Collections.Generic;
using rock.Framework.Models;

namespace rock.Core.Domains.Products
{
  public class ProductBrochure : IEntity
  {
    public int Id { get; set; }
    public int ProductId { get; set; }

    public string HTML { get; set; }

    public byte[] RowVersion { get; set; }

    public virtual ICollection<ProductBrochureAttachment> Attachments { get; set; }
    public virtual Product Product { get; set; }

  }
}
