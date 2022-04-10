using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Files;
using rock.Core.Domains.Threads;
using rock.Core.Services.Files;
using rock.Factories;
using rock.Models;
using rock.Models.FileApi;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [Authorize]
  public class FileApiController : BaseController
  {
    #region Fields
    private readonly IFileService fileService;
    private readonly IFileFactory factory;
    #endregion
    #region Constractor
    public FileApiController(IFileService fileService,
                             IFileFactory factory)
    {
      this.fileService = fileService;
      this.factory = factory;
    }
    #endregion
    #region File
    [HttpPost("files/store")]
    public async Task StoreImage(Guid imageId, CancellationToken cancellationToken)
    {
      var image = new File();
      image.Id = imageId;
      image.Access = FileAccessType.Public;
      image.OwnerGroup = Core.Domains.Users.UserRole.None;
      await fileService.InsertFile(file: image,
                                   cancellationToken: cancellationToken);
    }
    [HttpGet("files/{fileId}")]
    public async Task<FileModel> GetFile([FromRoute] Guid fileId, CancellationToken cancellationToken)
    {
      return await factory.PrepareFileModel(fileId, cancellationToken);
    }
    [HttpGet("files")]
    public async Task<IPagedList<FileModel>> GetFiles([FromQuery] FileSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareFilePagedListModel(parameters: parameters,
                                                     cancellationToken: cancellationToken);
    }
    [HttpDelete("me/files/{fileId}")]
    public async Task DeleteMyFile([FromRoute] Guid fileId, CancellationToken cancellationToken)
    {
      var file = await fileService.GetFileById(id: fileId, cancellationToken: cancellationToken);
      await fileService.DeleteMyFile(file: file, cancellationToken: cancellationToken);
    }
    [HttpDelete("files/{fileId}")]
    public async Task DeleteFile([FromRoute] Guid fileId, CancellationToken cancellationToken)
    {
      var file = await fileService.GetFileById(id: fileId, cancellationToken: cancellationToken);
      await fileService.DeleteFile(file, cancellationToken);
    }
    [HttpPost("files/upload")]
    [Authorize]
    public async Task<IList<Guid>> UploadFileToSession(List<IFormFile> files, CancellationToken cancellationToken)
    {
      var result = new List<Guid>();
      long size = files.Sum(f => f.Length);
      foreach (var formFile in files)
      {
        if (formFile.Length > 0)
        {
          var fileInput = new UploadFileInput(formFile);
          var fileId = await fileService.UploadToMemory(fileInput, cancellationToken);
          result.Add(fileId);
        }
      }
      return result;
    }
    #endregion
    #region FileTags
    [HttpPost("files/{fileId}/tags")]
    public async Task<Key<int>> CreateFileTag([FromRoute] Guid fileId, [FromBody] WriteFileTagModel newTag, CancellationToken cancellationToken)
    {
      var file = await fileService.GetFileById(id: fileId,
                                               loadStream: true,
                                               cancellationToken: cancellationToken);
      var tag = new ThreadActivity();
      tag.Payload = newTag.Text;
      await fileService.InsertFileTag(tag: tag, file: file, cancellationToken);
      return new Key<int>(tag.Id);
    }
    [HttpDelete("files/{fileId}/tags/{tagId}")]
    public async Task DeleteFileTag([FromRoute] Guid fileId, [FromRoute] int tagId, CancellationToken cancellationToken)
    {
      var tag = await fileService.GetFileTagById(fileId: fileId,
                                                 tagId: tagId,
                                                 cancellationToken: cancellationToken);
      await fileService.DeleteFileTag(tag: tag,
                                            cancellationToken: cancellationToken);
    }
    [HttpGet("files/{fileId}/tags/{tagId}")]
    public async Task<FileTagModel> GetFileTag([FromRoute] Guid fileId, [FromRoute] int tagId, CancellationToken cancellationToken)
    {
      return await factory.PrepareFileTagModel(fileId: fileId,
                                               tagId: tagId,
                                               cancellationToken: cancellationToken);
    }
    [HttpGet("files/{fileId}/tags")]
    public async Task<IList<FileTagModel>> GetPagedFileTags([FromRoute] Guid fileId, CancellationToken cancellationToken)
    {
      return await factory.PrepareFileTagListModel(fileId: fileId,
                                                   cancellationToken: cancellationToken);
    }
    [HttpGet("files/tags")]
    public async Task<IList<TagModel>> GetFileTag(CancellationToken cancellationToken)
    {
      return await factory.PrepareTagListModel(cancellationToken: cancellationToken);
    }
    #endregion
  }
}