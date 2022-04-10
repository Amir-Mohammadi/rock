using rock.Core.Services.Common;
using rock.Core.Data;
using rock.Core.Domains.Documents;
using rock.Core.Domains.Forms;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace rock.Core.Services.Documents
{
  public partial class DocumentService : IDocumentService
  {
    #region Field
    private readonly IRepository<Document> documentRepository;
    private readonly IWorkContext workContext;
    #endregion
    #region Constractor
    public DocumentService(IRepository<Document> documentRepository,
                           IWorkContext workContext)
    {
      this.documentRepository = documentRepository;
      this.workContext = workContext;
    }
    #endregion
    #region Document
    public async Task<Document> GetDocumentById(int id, CancellationToken cancellationToken, IInclude<Document> include = null)
    {
      return await documentRepository.GetAsync(predicate: x => x.Id == id,
                                               cancellationToken: cancellationToken,
                                               include: include);
    }
    public IQueryable<Document> GetDocuments(IInclude<Document> include = null)
    {
      return documentRepository.GetQuery(include: include);
    }
    public async Task<Document> CreateDocument(Form form, string description, CancellationToken cancellationToken)
    {
      var document = new Document();
      document.UserId = workContext.GetCurrentUserId();
      document.CreatedAt = DateTime.UtcNow;
      document.FormId = form.Id;
      await documentRepository.AddAsync(entity: document, cancellationToken: cancellationToken);
      return document;
    }

    public async Task<Document> CreateDocumentByUserId(Form form, int userId, string description, CancellationToken cancellationToken)
    {
      var document = new Document();
      document.UserId = userId;
      document.CreatedAt = DateTime.UtcNow;
      document.FormId = form.Id;
      await documentRepository.AddAsync(entity: document, cancellationToken: cancellationToken);
      return document;
    }
    #endregion
  }
}