using System;
using System.Collections.Generic;
using rock.Core.Domains.Files;
using rock.Framework.Models;
namespace rock.Core.Domains.Contents
{
  public class Content : IEntity, IHasCode, IHasDescription, ISeoFriendly, IRemovable, ITimestamp
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Description { get; set; }
    public string UrlTitle { get; set; }
    public string BrowserTitle { get; set; }
    public string MetaDescription { get; set; }
    public ContentType ContentType { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ContentFile> ContentFiles { get; set; }
  }
}