using System;

namespace rock.Models.CommonApi
{
  public class ColorModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Code { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
