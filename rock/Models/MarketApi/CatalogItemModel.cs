using System;
using rock.Core.Domains.Catalogs;
namespace rock.Models.MarketApi
{
  public class CatalogItemModel
  {
    public int Id { get; set; }
    public CatalogItemType Type { get; set; }
    public int Order { get; set; }
    public bool ShowInFilter { get; set; }
    public bool IsMain { get; set; }
    public bool HasMultiple { get; set; }
    public string Value { get; set; }
    public int CatalogId { get; set; }
    public int? ReferenceId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}