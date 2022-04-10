
using System;
using Microsoft.AspNetCore.Http;
using rock.Framework.FileHandler;
namespace rock.Models.FileApi
{
  public class UploadFileInput : IUploadFileInput
  {
    public UploadFileInput() { }
    public UploadFileInput(IFormFile formFile)
    {
      var fileStream = formFile.OpenReadStream();
      Stream = new byte[formFile.Length];
      fileStream.Read(Stream, 0, (int)formFile.Length);
      this.FileKey = Guid.NewGuid();
      FileName = formFile.FileName;
    }
    public Guid FileKey { get; set; }
    public string FileName { get; set; }
    public byte[] Stream { get; set; }
  }
}