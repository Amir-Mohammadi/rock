using System;
namespace rock.Framework.FileHandler
{
  public interface IUploadFileInput
  {
    Guid FileKey { get; set; }
    string FileName { get; set; }
    byte[] Stream { get; set; }
  }
}