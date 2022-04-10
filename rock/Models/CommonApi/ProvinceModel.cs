using System;
namespace rock.Models.CommonApi
{
  public class ProvinceModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int AreaCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}