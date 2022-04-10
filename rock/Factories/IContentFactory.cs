using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Models.Contents;
using rock.Models.ProductApi;
namespace rock.Factories
{
  public interface IContentFactory : IScopedDependency
  {
    #region Content
    Task<ContentModel> PrepareContentModel(int id, CancellationToken cancellationToken);
    Task<ContentModel> PrepareContentModel(string code, CancellationToken cancellationToken);
    Task<IPagedList<ContentModel>> PrepareContentPagedListModel(ContentSearchParameters parameters, CancellationToken cancellationToken);
    #endregion
    #region ContentFile
    Task<ContentFileModel> PrepareContentFileModel(string code, int contentFileId, CancellationToken cancellationToken);
    Task<ContentFileModel> PrepareContentFileModel(int contentId, int id, CancellationToken cancellationToken);
    Task<IList<ContentFileModel>> PrepareContentFileListModel(int contentId, CancellationToken cancellationToken);
    Task<IList<ContentFileModel>> PrepareContentFileListModel(string code, CancellationToken cancellationToken);
    #endregion
  }
}