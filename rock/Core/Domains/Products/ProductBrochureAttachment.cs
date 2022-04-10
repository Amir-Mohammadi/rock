using System;
using rock.Core.Domains.Files;
using rock.Framework.Models;

namespace rock.Core.Domains.Products
{
  public class ProductBrochureAttachment : IEntity
  {
    public int Id { get; set; }

    public int BrochurId { get; set; }
    public Guid FileId { get; set; }
    public File File { get; set; }

    public virtual ProductBrochure Brochure { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
