using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace rock.Models.ProductApi
{
  public class ProductCategoryModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public int? ParentId { get; set; }
    public string Explanation { get; set; }
    public bool IsArchive { get; internal set; }
    public byte[] RowVersion { get; set; }
    public bool IsPublished { get; set; }
  }
}