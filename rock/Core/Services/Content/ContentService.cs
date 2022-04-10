using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Contents;
using rock.Framework.Autofac;
using rock.Models.Contents;
namespace rock.Core.Services
{
  public class ContentService : IContentService
  {
    #region Fields
    private readonly IRepository<Content> contentRepository;
    private readonly IRepository<ContentFile> contentFileRepository;
    #endregion
    #region Constractor
    public ContentService(IRepository<Content> contentRepository,
                          IRepository<ContentFile> contentFileRepository)
    {
      this.contentRepository = contentRepository;
      this.contentFileRepository = contentFileRepository;
    }
    #endregion
    #region Content
    public async Task<Content> AddContent(Content content, CancellationToken cancellationToken)
    {
      content.CreatedAt = DateTime.UtcNow;
      content.IsActive = true;
      await contentRepository.AddAsync(entity: content, cancellationToken: cancellationToken);
      return content;
    }
    public IQueryable<Content> GetContents(IInclude<Content> include = null)
    {
      return contentRepository.GetQuery(include);
    }
    public async Task<Content> GetContent(int id, CancellationToken cancellationToken, IInclude<Content> include = null)
    {
      var content = await contentRepository.GetAsync(predicate: x => x.Id == id,
                                                     cancellationToken: cancellationToken,
                                                     include: include);
      return content;
    }
    public async Task<Content> GetContent(string code, CancellationToken cancellationToken, IInclude<Content> include = null)
    {
      var content = await contentRepository.GetAsync(predicate: x => x.Code == code,
                                                     cancellationToken: cancellationToken,
                                                     include: include);
      return content;
    }
    public IQueryable<Content> GetContent(IInclude<Content> include = null)
    {
      return contentRepository.GetQuery(include: include);
    }
    public async Task EditContent(Content content, CancellationToken cancellationToken)
    {
      content.UpdatedAt = DateTime.UtcNow;
      await contentRepository.UpdateAsync(entity: content,
                                          cancellationToken: cancellationToken);
    }
    public async Task DeleteContent(int id, CancellationToken cancellationToken)
    {
      var content = await GetContent(id: id, cancellationToken: cancellationToken);
      content.DeletedAt = DateTime.UtcNow;
      await contentRepository.UpdateAsync(content, cancellationToken);
    }
    public async Task ActivateContent(Content content, CancellationToken cancellationToken)
    {
      content.IsActive = true;
      content.UpdatedAt = DateTime.UtcNow;
      await contentRepository.UpdateAsync(content, cancellationToken);
    }
    public async Task DeactivateContent(Content content, CancellationToken cancellationToken)
    {
      content.IsActive = false;
      content.UpdatedAt = DateTime.UtcNow;
      await contentRepository.UpdateAsync(content, cancellationToken);
    }
    #endregion
    #region ContentFile
    public async Task<ContentFile> AddContentFile(ContentFile contentFile, Content content, CancellationToken cancellationToken)
    {
      contentFile.Content = content;
      await contentFileRepository.AddAsync(contentFile, cancellationToken);
      return contentFile;
    }
    public async Task EditContentFile(ContentFile contentFile, CancellationToken cancellationToken)
    {
      await contentFileRepository.UpdateAsync(contentFile, cancellationToken);
    }
    public async Task<ContentFile> GetContentFile(int id, CancellationToken cancellationToken, IInclude<ContentFile> include = null)
    {
      return await contentFileRepository.GetAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken, include: include);
    }
    public IQueryable<ContentFile> GetContentFiles(int? contentId = null, string code = null, IInclude<ContentFile> include = null)
    {
      var query = contentFileRepository.GetQuery(include: include);
      if (contentId != null)
        query = query.Where(x => x.ContentId == contentId);
      if (code != null)
        query = query.Where(x => x.Content.Code == code);
      return query;
    }
    public async Task DeleteContentFile(int contentId, int id, CancellationToken cancellationToken)
    {
      var contentFile = await GetContentFiles().FirstOrDefaultAsync(x => x.Id == id
                                                                      && x.ContentId == contentId, cancellationToken);
      await contentFileRepository.DeleteAsync(contentFile, cancellationToken);
    }
    #endregion
  }
}