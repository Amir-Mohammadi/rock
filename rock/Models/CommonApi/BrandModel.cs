using System;
using System.ComponentModel.DataAnnotations;
using rock.Models.FileApi;
namespace rock.Models.CommonApi
{
  public class BrandModel
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string UrlTitle { get; set; }
    [Required]
    public string ImageTitle { get; set; }
    [Required]
    public string ImageAlt { get; set; }
    [Required]
    public Guid ImageId { get; set; }
    public byte[] RowVersion { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public string Description { get; set; }
  }
}