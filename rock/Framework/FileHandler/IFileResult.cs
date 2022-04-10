using System;
namespace rock.Framework.FileHandler
{
  public interface IFileResult
  {
    Guid Id { get; set; }
    string FileName { get; set; }
    string FileType { get; set; }
    byte[] FileStream { get; set; }
    byte[] RowVersion { get; set; }
  }
}