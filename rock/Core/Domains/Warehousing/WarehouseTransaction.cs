using rock.Core.Domains.Commons;
using rock.Core.Domains.Documents;
using rock.Core.Domains.Products;
using rock.Framework.Models;
namespace rock.Core.Domains.Warehousing
{
  public class WarehouseTransaction : IEntity
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public TransactionType Factor { get; set; }
    public int Amount { get; set; }
    public int DocumentId { get; set; }
    public int WarehouseId { get; set; }
    public virtual Document Document { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual Product Product { get; set; }
    public virtual ProductColor ProductColor { get; set; }
    public virtual Color Color { get; set; }
    public byte[] RowVersion { get; set; }
  }
}