using rock.Framework.Models;
namespace rock.Core.Domains.Forms
{
  public class Form : IEntity, IHasDescription
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public FormOption FormOptions { get; set; }
    public byte[] RowVersion { get; set; }
  }
}