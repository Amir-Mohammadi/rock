using System;

namespace rock.Models.FinancialApi
{
  public class FinancialFormModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
