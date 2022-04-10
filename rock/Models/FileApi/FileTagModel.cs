using System;
using Microsoft.VisualBasic;
using rock.Models.UserApi;
namespace rock.Models.FileApi
{
  public class FileTagModel
  {
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
  }
}