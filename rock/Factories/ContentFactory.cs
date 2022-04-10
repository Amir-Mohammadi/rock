using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Data;
using rock.Core.Domains.Contents;
using rock.Core.Domains.Files;
using rock.Core.Services;
using rock.Core.Services.Files;
using rock.Framework.Autofac;
using rock.Models.Contents;
using rock.Models.ProductApi;
namespace rock.Factories
{
  public class ContentFactory : BaseFactory, IContentFactory
  {
    #region Fields
    private readonly IContentService contentService;
    private readonly IFileService fileService;
    #endregion
    #region Constractor
    public ContentFactory(IContentService contentService,
    IFileService fileService)
    {
      this.contentService = contentService;
      this.fileService = fileService;
    }
    #endregion
    #region Content
    private readonly IInclude<Content> contentInclude = new Include<Content>(query =>
     {
       query = query.Include(x => x.ContentFiles);
       return query;
     });
    private ContentModel CreateContentModel(Content content)
    {
      if (content == null)
        return null;
      return new ContentModel
      {
        Id = content.Id,
        Code = content.Code,
        Title = content.Title,
        Body = content.Body,
        Description = content.Description,
        UrlTitle = content.UrlTitle,
        BrowserTitle = content.BrowserTitle,
        MetaDescription = content.MetaDescription,
        ContentType = content.ContentType,
        IsActive = content.IsActive,
        CreatedAt = content.CreatedAt,
        UpdatedAt = content.UpdatedAt,
        DeletedAt = content.DeletedAt,
        RowVersion = content.RowVersion,
        ContentFiles = content.ContentFiles?.Select(x => CreateContentFileModel(x)).ToArray(),
      };
    }
    public async Task<ContentModel> PrepareContentModel(int id, CancellationToken cancellationToken)
    {
      var content = await contentService.GetContent(id: id,
                                                    cancellationToken: cancellationToken,
                                                    include: contentInclude);
      return CreateContentModel(content);
    }
    public async Task<ContentModel> PrepareContentModel(string code, CancellationToken cancellationToken)
    {
      var content = await contentService.GetContent(code: code,
                                                    cancellationToken: cancellationToken,
                                                    include: contentInclude);
      return CreateContentModel(content);
    }
    public async Task<IPagedList<ContentModel>> PrepareContentPagedListModel(ContentSearchParameters parameters, CancellationToken cancellationToken)
    {
      var contents = this.contentService.GetContents();
      return await this.CreateModelPagedListAsync(source: contents,
                                                  convertFunction: this.CreateContentModel,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  cancellationToken: cancellationToken);
    }
    #endregion
    #region ContentFile
    private ContentFileModel CreateContentFileModel(ContentFile contentFile)
    {
      if (contentFile == null)
        return null;
      return new ContentFileModel
      {
        Id = contentFile.Id,
        FileId = contentFile.FileId,
        ContentId = contentFile.ContentId,
        Title = contentFile.Title,
        ImageAlt = contentFile.ImageAlt,
        RowVersion = contentFile.RowVersion,
      };
    }
    public async Task<ContentFileModel> PrepareContentFileModel(string code, int contentFileId, CancellationToken cancellationToken)
    {
      var contentFile = await contentService.GetContentFiles()
                                            .FirstOrDefaultAsync(x => x.Id == contentFileId
                                                                      && x.Content.Code == code, cancellationToken);
      return CreateContentFileModel(contentFile);
    }
    public async Task<ContentFileModel> PrepareContentFileModel(int contentId, int id, CancellationToken cancellationToken)
    {
      var contentFile = await contentService.GetContentFiles()
                                            .FirstOrDefaultAsync(x => x.Id == id
                                                                      && x.ContentId == contentId, cancellationToken);
      return CreateContentFileModel(contentFile);
    }
    public async Task<IList<ContentFileModel>> PrepareContentFileListModel(int contentId, CancellationToken cancellationToken)
    {
      var contentFiles = contentService.GetContentFiles(contentId: contentId);
      return await this.CreateModelListAsync(source: contentFiles,
                                             convertFunction: this.CreateContentFileModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<IList<ContentFileModel>> PrepareContentFileListModel(string code, CancellationToken cancellationToken)
    {
      var contentFiles = contentService.GetContentFiles(code: code);
      return await this.CreateModelListAsync(source: contentFiles,
                                             convertFunction: this.CreateContentFileModel,
                                             cancellationToken: cancellationToken);
    }
    #endregion
  }
}