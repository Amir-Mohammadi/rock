using System;
using rock.Core.Domains.Forms;
using rock.Core.Domains.Users;

namespace rock.Models.FinancialApi
{
  public class DocumentModel
  {
    public int Id { get; set; }
    public Form Form { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
