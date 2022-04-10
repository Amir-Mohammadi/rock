using System;
using System.ComponentModel.DataAnnotations;
using rock.Core.Domains.Catalogs;
namespace rock.Models.ProductApi
{
  public class ProductCategoryPropertyModel
  {
    [Required]
    public int Id { get; set; }
    [Required, StringLength(64)]
    public string Title { get; set; }
    [Required]
    public CatalogItemType Type { get; set; }
    [Required]
    public bool HasMultiple { get; set; }
    [Required]
    public int CatalogId { get; set; }
    public int? ReferenceId { get; set; }
    [Required]
    public bool IsMain { get; set; }
    [Required]
    public int Order { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }
    [Required]
    public bool ShowInFilter { get;  set; }
  }
}