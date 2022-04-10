using System;
using rock.Core.Domains.Files;
using rock.Framework.Models;
namespace rock.Core.Domains.Contents
{
  public class ContentFile : IEntity
  {
    public int Id { get; set; }
    public Guid FileId { get; set; }
    public int ContentId { get; set; }
    public virtual Content Content { get; set; }
    public string Title { get; set; }
    public string ImageAlt { get; set; }
    public virtual File File { get; set; }
    public byte[] RowVersion { get; set; }
  }
}