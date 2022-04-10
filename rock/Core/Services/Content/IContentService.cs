using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Contents;
using rock.Framework.Autofac;
using rock.Models.Contents;
namespace rock.Core.Services
{
  public interface IContentService : IScopedDependency
  {
    #region Content
    Task<Content> AddContent(Content content, CancellationToken cancellationToken);
    Task<Content> GetContent(int id, CancellationToken cancellationToken, IInclude<Content> include = null);
    Task<Content> GetContent(string code, CancellationToken cancellationToken, IInclude<Content> include = null);
    IQueryable<Content> GetContents(IInclude<Content> include = null);
    Task EditContent(Content content, CancellationToken cancellationToken);
    Task DeleteContent(int id, CancellationToken cancellationToken);
    Task ActivateContent(Content content, CancellationToken cancellationToken);
    Task DeactivateContent(Content content, CancellationToken cancellationToken);
    #endregion
    #region ContentFile
    Task<ContentFile> AddContentFile(ContentFile contentFile, Content content, CancellationToken cancellationToken);
    Task EditContentFile(ContentFile contentFile, CancellationToken cancellationToken);
    Task<ContentFile> GetContentFile(int id, CancellationToken cancellationToken, IInclude<ContentFile> include = null);
    IQueryable<ContentFile> GetContentFiles(int? contentId = null, string code = null, IInclude<ContentFile> include = null);
    Task DeleteContentFile(int contentId, int id, CancellationToken cancellationToken);
    #endregion
  }
}