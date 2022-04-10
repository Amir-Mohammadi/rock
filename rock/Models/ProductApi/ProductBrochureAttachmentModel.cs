using rock.Models.FileApi;

namespace rock.Models.ProductApi
{
  public class ProductBrochureAttachmentModel
  {
    public int Id { get; set; }
    public int ProductBrochurId { get; set; }
    public FileModel File { get; set; }
    public byte[] RowVersion { get; set; }
  }
}