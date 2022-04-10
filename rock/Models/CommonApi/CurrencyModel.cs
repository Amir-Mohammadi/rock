using System;

namespace rock.Models.CommonApi
{
  public class CurrencyModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public double Ratio { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
