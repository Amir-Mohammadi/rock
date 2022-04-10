using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Common;
using rock.Core.Domains.Files;
using rock.Core.Domains.Threads;
using rock.Core.Services.Files;
using rock.Models.FileApi;
namespace rock.Factories
{
  public class FileFactory : BaseFactory, IFileFactory
  {
    #region Field
    private readonly IFileService fileService;
    #endregion
    #region Constractor
    public FileFactory(IFileService fileService)
    {
      this.fileService = fileService;
    }
    #endregion
    #region File
    public async Task<FileModel> PrepareFileModel(Guid id, CancellationToken cancellationToken)
    {
      var file = await fileService.GetFileById(id: id, loadStream: false, cancellationToken: cancellationToken);
      return CreateFileModel(file);
    }
    public async Task<IPagedList<FileModel>> PrepareFilePagedListModel(FileSearchParameters parameters, CancellationToken cancellationToken)
    {
      var files = fileService.GetFiles(tag: parameters.Tag, cancellationToken: cancellationToken);
      return await this.CreateModelPagedListAsync(source: files,
                                                  convertFunction: this.CreateFileModel,
                                                  sortBy: parameters.SortBy,
                                                  order: parameters.Order,
                                                  pageIndex: parameters.PageIndex,
                                                  pageSize: parameters.PageSize,
                                                  cancellationToken: cancellationToken);
    }
    private FileModel CreateFileModel(File file)
    {
      if (file == null)
        return null;
      return new FileModel()
      {
        Id = file.Id,
        Ext = file.FileType,
        Access = file.Access,
        Owners = file.OwnerGroup, 
        RowVersion = file.RowVersion       
      };
    }
    #endregion
    #region FileTag
    public async Task<FileTagModel> PrepareFileTagModel(Guid fileId, int tagId, CancellationToken cancellationToken)
    {
      var fileTag = await this.fileService.GetFileTagById(fileId: fileId,
                                                                   tagId: tagId,
                                                                   cancellationToken: cancellationToken);
      return this.createFileTagModel(fileTag: fileTag);
    }
    public async Task<IList<FileTagModel>> PrepareFileTagListModel(Guid fileId, CancellationToken cancellationToken)
    {
      var fileTags = this.fileService.GetFileTags(fileId: fileId);
      return await this.CreateModelListAsync(source: fileTags,
                                             convertFunction: this.createFileTagModel,
                                             cancellationToken: cancellationToken);
    }
    public async Task<IList<TagModel>> PrepareTagListModel(CancellationToken cancellationToken)
    {
      var tags = this.fileService.GetTags();
      return await this.CreateModelListAsync(source: tags,
                                             convertFunction: x => x,
                                             cancellationToken: cancellationToken);
    }
    private FileTagModel createFileTagModel(ThreadActivity fileTag)
    {
      if (fileTag == null)
        return null;
      return new FileTagModel()
      {
        CreatedAt = fileTag.CreatedAt,
        Id = fileTag.Id,
        Text = fileTag.Payload,
        UpdateAt = fileTag.UpdatedAt
      };
    }
    #endregion
  }
}