using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Common;
using rock.Core.Domains.Contents;
using rock.Core.Services;
using rock.Core.Services.Files;
using rock.Factories;
using rock.Models;
using rock.Models.Contents;
namespace WebsiteApi.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  public class ContentController : ControllerBase
  {
    #region Fields
    private readonly IContentService contentService;
    private readonly IContentFactory contentFactory;
    private readonly IFileService fileService;
    #endregion
    #region Constractor
    public ContentController(IContentService contentService,
                             IContentFactory contentFactory,
                             IFileService fileService)
    {
      this.contentService = contentService;
      this.contentFactory = contentFactory;
      this.fileService = fileService;
    }
    #endregion
    #region Content
    [Authorize]
    [HttpPost("contents")]
    public async Task<Key<int>> AddContent([FromBody] ContentModel model, CancellationToken cancellationToken)
    {
      var content = new Content
      {
        Code = model.Code,
        Title = model.Title,
        Body = model.Body,
        Description = model.Description,
        UrlTitle = model.UrlTitle,
        BrowserTitle = model.BrowserTitle,
        MetaDescription = model.MetaDescription,
        ContentType = model.ContentType,
      };
      await contentService.AddContent(content: content, cancellationToken: cancellationToken);
      return new Key<int>(content.Id);
    }
    [Authorize]
    [HttpPut("contents/{contentId}")]
    public async Task EditContent([FromRoute] int contentId, [FromBody] ContentModel model, CancellationToken cancellationToken)
    {
      var content = await contentService.GetContent(id: contentId,
                                                    cancellationToken: cancellationToken);
      content.Code = model.Code;
      content.Title = model.Title;
      content.Body = model.Body;
      content.Description = model.Description;
      content.UrlTitle = model.UrlTitle;
      content.BrowserTitle = model.BrowserTitle;
      content.MetaDescription = model.MetaDescription;
      content.ContentType = model.ContentType;
      content.RowVersion = model.RowVersion;
      await contentService.EditContent(content: content,
                                       cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpDelete("contents/{contentId}")]
    public async Task DeleteContent([FromRoute] int contentId, CancellationToken cancellationToken)
    {
      await contentService.DeleteContent(id: contentId, cancellationToken: cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("contents/code:{code}")]
    public async Task<ContentModel> GetContent([FromRoute] string code, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentModel(code: code, cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("contents/id:{contentId}")]
    public async Task<ContentModel> GetContent([FromRoute] int contentId, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentModel(id: contentId, cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("contents")]
    public async Task<IPagedList<ContentModel>> GetContents([FromQuery] ContentSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentPagedListModel(parameters: parameters, cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpPut("contents/{contentId}/activate")]
    public async Task ActivateContent([FromRoute] int contentId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var content = await contentService.GetContent(id: contentId,
                                                    cancellationToken: cancellationToken);
      content.RowVersion = rowVersion;
      await contentService.ActivateContent(content: content, cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpPut("contents/{contentId}/deactivate")]
    public async Task DeactivateContent([FromRoute] int contentId, [FromHeader] byte[] rowVersion, CancellationToken cancellationToken)
    {
      var content = await contentService.GetContent(id: contentId,
                                                    cancellationToken: cancellationToken);
      content.RowVersion = rowVersion;
      await contentService.DeactivateContent(content: content, cancellationToken: cancellationToken);
    }
    #endregion
    #region ContentFile
    [Authorize]
    [HttpPost("contents/{contentId}/files")]
    public async Task<Key<int>> AddContentFile([FromRoute] int contentId, [FromBody] ContentFileModel model, CancellationToken cancellationToken)
    {
      var content = await contentService.GetContent(id: contentId, cancellationToken: cancellationToken);
      var contentFile = new ContentFile
      {
        FileId = model.FileId,
        Title = model.Title,
        ImageAlt = model.ImageAlt
      };
      await contentService.AddContentFile(contentFile: contentFile, content: content, cancellationToken: cancellationToken);
      return new Key<int>(content.Id);
    }
    [Authorize]
    [HttpPut("contents/{contentId}/files/{contentFileId}")]
    public async Task EditContentFile([FromRoute] int contentFileId, [FromBody] ContentFileModel model, CancellationToken cancellationToken)
    {
      var contentFile = await contentService.GetContentFile(id: contentFileId,
                                                            cancellationToken: cancellationToken);
      contentFile.FileId = model.FileId;
      contentFile.Title = model.Title;
      contentFile.ImageAlt = model.ImageAlt;
      contentFile.RowVersion = model.RowVersion;
      await contentService.EditContentFile(contentFile: contentFile,
                                           cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpDelete("contents/{contentId}/files/{contentFileId}")]
    public async Task DeleteContentFile([FromRoute] int contentId, [FromRoute] int contentFileId, CancellationToken cancellationToken)
    {
      await contentService.DeleteContentFile(contentId: contentId, id: contentFileId, cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("contents/id:{contentId}/files/{contentFileId}")]
    public async Task<ContentFileModel> GetContentFile([FromRoute] int contentId, [FromRoute] int contentFileId, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentFileModel(contentId: contentId,
                                                          id: contentFileId,
                                                          cancellationToken: cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("contents/code:{contentCode}/files/{contentFileId}")]
    public async Task<ContentFileModel> GetContentFile([FromRoute] string contentCode, [FromRoute] int contentFileId, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentFileModel(code: contentCode,
                                                          contentFileId: contentFileId,
                                                          cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpGet("contents/id:{contentId}/files")]
    public async Task<IList<ContentFileModel>> GetContentFiles([FromRoute] int contentId, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentFileListModel(contentId: contentId, cancellationToken: cancellationToken);
    }
    [AllowAnonymous]
    [HttpGet("contents/code:{code}/files")]
    public async Task<IList<ContentFileModel>> GetContentFiles([FromRoute] string code, CancellationToken cancellationToken)
    {
      return await contentFactory.PrepareContentFileListModel(code: code, cancellationToken: cancellationToken);
    }
    #endregion
  }
}