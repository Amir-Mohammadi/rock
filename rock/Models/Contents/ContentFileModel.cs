using System;
namespace rock.Models.Contents
{
  public class ContentFileModel
  {
    public int Id { get; set; }
    public string Title { get; internal set; }
    public string ImageAlt { get; internal set; }
    public int ContentId { get; internal set; }
    public Guid FileId { get; set; }
    public byte[] FileRowVersion { get; internal set; }
    public byte[] RowVersion { get; set; }
  }
}