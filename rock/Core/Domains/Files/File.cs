using System;
using rock.Framework.Models;
using rock.Core.Domains.Users;
using rock.Framework.FileHandler;
using rock.Core.Domains.Threads;
namespace rock.Core.Domains.Files
{
  [Serializable]
  public class File : IEntity, ITimestamp, IRemovable, IFileResult
  {
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public int? ThreadId { get; set; }
    public byte[] FileStream { get; set; }
    public byte[] RowVersion { get; set; }
    public FileAccessType Access { get; set; }
    public UserRole? OwnerGroup { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [field: NonSerialized]
    public virtual Thread Thread { get; set; }
  }
}