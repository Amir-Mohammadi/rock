using System;
using rock.Core.Domains.Files;
using rock.Core.Domains.Users;
namespace rock.Models.FileApi
{
  public class FileModel
  {
    public Guid Id { get; set; }
    public string Ext { get; set; }
    public UserRole? Owners { get; set; }
    public FileAccessType Access { get; set; }
    public byte[] RowVersion { get; set; }
  }
}