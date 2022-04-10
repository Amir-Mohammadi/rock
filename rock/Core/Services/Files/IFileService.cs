using System;
using rock.Framework.Autofac;
using rock.Core.Domains.Files;
using System.Threading.Tasks;
using System.Threading;
using rock.Framework.FileHandler;
using rock.Core.Data;
using rock.Core.Domains.Threads;
using System.Linq;
using rock.Models.FileApi;
namespace rock.Core.Services.Files
{
  public partial interface IFileService : IScopedDependency, rock.Framework.FileHandler.IFileService
  {
    #region File
    Task<File> InsertFile(File file, CancellationToken cancellationToken);
    Task UpdateFile(File file, CancellationToken cancellationToken);
    Task<File> GetFileById(Guid id, CancellationToken cancellationToken, bool loadStream = false);
    IQueryable<File> GetFiles(string tag, CancellationToken cancellationToken);
    Task DeleteMyFile(File file, CancellationToken cancellationToken);
    Task DeleteFile(File file, CancellationToken cancellationToken);
    Task<Guid> UploadToMemory(IUploadFileInput uploadFileInput, CancellationToken cancellationToken);
    #endregion
    #region FileTag
    Task<ThreadActivity> InsertFileTag(ThreadActivity tag, File file, CancellationToken cancellationToken);
    Task<ThreadActivity> GetFileTagById(Guid fileId, int tagId, CancellationToken cancellationToken, IInclude<ThreadActivity> include = null);
    Task DeleteFileTag(ThreadActivity tag, CancellationToken cancellationToken);
    IQueryable<ThreadActivity> GetFileTags(Guid fileId, IInclude<ThreadActivity> include = null);
    IQueryable<TagModel> GetTags();
    #endregion
  }
}