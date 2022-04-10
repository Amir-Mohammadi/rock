using System;
namespace rock.Models.MarketApi
{
  public class MarketStuffPropertyModel
  {
    public int Id { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }
    public int CatalogItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public int? ReferenceId { get; set; }
    public string CatalogItemKeyName { get; set; }
    public string ExtraKeyName { get; set; }
    public bool IsMain { get; set; }
    public CatalogItemModel Reference { get; set; }
    public bool ShowInFilter { get; set; }
  }
}