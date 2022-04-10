using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Framework.Autofac;
using rock.Models.FileApi;
namespace rock.Factories
{
  public interface IFileFactory : IScopedDependency
  {
    Task<FileModel> PrepareFileModel(Guid fileId, CancellationToken cancellationToken);
    Task<FileTagModel> PrepareFileTagModel(Guid fileId, int tagId, CancellationToken cancellationToken);
    Task<IList<FileTagModel>> PrepareFileTagListModel(Guid fileId, CancellationToken cancellationToken);
    Task<IList<TagModel>> PrepareTagListModel(CancellationToken cancellationToken);
    Task<IPagedList<FileModel>> PrepareFilePagedListModel(FileSearchParameters parameters, CancellationToken cancellationToken);
  }
}