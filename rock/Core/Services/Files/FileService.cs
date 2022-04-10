using System;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Files;
using rock.Framework.StateManager;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using rock.Framework.FileHandler;
using rock.Models.FileApi;
using rock.Core.Domains.Threads;
using rock.Core.Services.Threads;
namespace rock.Core.Services.Files
{
  public class FileService : IFileService
  {
    #region Fields
    private readonly IStateManagerService stateManagerService;
    private readonly IRepository<File> fileRepository;
    private readonly IThreadService threadService;
    #endregion
    #region Constractor        
    public FileService(IStateManagerService stateManagerService,
                       IThreadService threadService,
                       IRepository<File> fileRepository)
    {
      this.threadService = threadService;
      this.stateManagerService = stateManagerService;
      this.fileRepository = fileRepository;
    }
    #endregion
    #region File
    public async Task DeleteMyFile(File file, CancellationToken cancellationToken)
    {
      await fileRepository.DeleteAsync(predicate: x => x.Id == file.Id, cancellationToken);
    }
    public async Task DeleteFile(File file, CancellationToken cancellationToken)
    {
      await fileRepository.DeleteAsync(predicate: x => x.Id == file.Id, cancellationToken);
    }
    public async Task<File> GetFileById(Guid id, CancellationToken cancellationToken, bool loadStream = false)
    {
      var query = fileRepository.GetQuery().Where(predicate: x => x.Id == id).Select(x => new File()
      {
        Id = x.Id,
        OwnerGroup = x.OwnerGroup,
        Access = x.Access,
        FileName = x.FileName,
        FileType = x.FileType,
        ThreadId = x.ThreadId,
        CreatedAt = x.CreatedAt,
        UpdatedAt = x.UpdatedAt,
        DeletedAt = x.DeletedAt,
        RowVersion = x.RowVersion,
        FileStream = loadStream ? x.FileStream : null
      });
      return await query.FirstOrDefaultAsync(cancellationToken);
    }
    public IQueryable<File> GetFiles(string tag, CancellationToken cancellationToken)
    {
      var query = fileRepository.GetQuery();
      if (!string.IsNullOrWhiteSpace(tag))
      {
        var filesWithTag = from file in query
                           from ta in file.Thread.Activities
                           where ta.Type == ThreadActivityType.Tag && ta.Payload == tag
                           select new { file.Id };
        filesWithTag = filesWithTag.Distinct();
        query = from file in query
                join fwt in filesWithTag on file.Id equals fwt.Id
                select file;
      }
      var result = query.Select(x => new File()
      {
        Id = x.Id,
        OwnerGroup = x.OwnerGroup,
        Access = x.Access,
        FileName = x.FileName,
        FileType = x.FileType,
        ThreadId = x.ThreadId,
        CreatedAt = x.CreatedAt,
        UpdatedAt = x.UpdatedAt,
        DeletedAt = x.DeletedAt,
        RowVersion = x.RowVersion,
        FileStream = null
      });
      return result;
    }
    public async Task<IFileResult> GetFileResultWithStreamById(Guid id, CancellationToken cancellationToken)
    {
      var result = await GetFileById(id: id,
                                     cancellationToken: cancellationToken,
                                     loadStream: true);
      return result;
    }
    public async Task<File> InsertFile(File file, CancellationToken cancellationToken)
    {
      var uploadFileInput = await this.stateManagerService.GetState<UploadFileInput>("fk" + file.Id.ToString());
      file.FileStream = uploadFileInput.Stream;
      file.FileName = uploadFileInput.FileName;
      file.CreatedAt = DateTime.UtcNow;
      file.FileType = System.IO.Path.GetExtension(uploadFileInput.FileName);
      await fileRepository.AddAsync(file, cancellationToken);
      return file;
    }
    public async Task UpdateFile(File file, CancellationToken cancellationToken)
    {
      await fileRepository.UpdateAsync(file, cancellationToken);
    }
    public async Task<Guid> UploadToMemory(IUploadFileInput uploadFileInput, CancellationToken cancellationToken)
    {
      var fileId = Guid.NewGuid();
      await this.stateManagerService.SetState("fk" + fileId.ToString(), uploadFileInput);
      return fileId;
    }
    #endregion
    #region FileTag
    public async Task<ThreadActivity> InsertFileTag(ThreadActivity tag, File file, CancellationToken cancellationToken)
    {
      fileRepository.LoadReference(entity: file,
                                   referenceProperty: x => x.Thread);
      if (file.Thread == null)
      {
        file.Thread = new Domains.Threads.Thread();
        await fileRepository.UpdateAsync(entity: file,
                                         cancellationToken: cancellationToken);
      }
      return await threadService.AddTag(tag: tag,
                                        thread: file.Thread,
                                        cancellationToken: cancellationToken);
    }
    public async Task<ThreadActivity> GetFileTagById(Guid fileId, int tagId, CancellationToken cancellationToken, IInclude<ThreadActivity> include = null)
    {
      var query = GetFileTags(fileId: fileId,
                              include: include);
      query = query.Where(x => x.Id == tagId);
      return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    public IQueryable<ThreadActivity> GetFileTags(Guid fileId, IInclude<ThreadActivity> include = null)
    {
      return getFileThreadActivities(fileId: fileId,
                                        type: ThreadActivityType.Tag,
                                        include: include);
    }
    public async Task DeleteFileTag(ThreadActivity tag, CancellationToken cancellationToken)
    {
      await threadService.DeleteThreadActivity(threadActivity: tag,
                                               cancellationToken: cancellationToken);
    }
    public IQueryable<TagModel> GetTags()
    {
      var query = getFileThreadActivities(type: ThreadActivityType.Tag);
      var fileTags = query.Select(x => new TagModel { Text = x.Payload });
      var tags = fileTags.Distinct();
      return tags;
    }
    #endregion
    #region FileTreadActivity    
    private IQueryable<ThreadActivity> getFileThreadActivities(ThreadActivityType type, Guid? fileId = null, int? referenceId = null, bool showUnPublished = true, IInclude<ThreadActivity> include = null)
    {
      var files = fileRepository.GetQuery();
      if (fileId != null)
        files = files.Where(x => x.Id == fileId);
      var query = from p in files
                  from ta in p.Thread.Activities
                  where ta.Type == type
                  select ta;
      if (referenceId != null)
        query = query.Where(x => x.ReferenceId == referenceId);
      if (showUnPublished == false)
        query = query.Where(x => x.PublishAt != null);
      if (include != null)
        query = include.Execute(query);
      return query;
    }
    #endregion
  }
}