using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Documents;
using rock.Core.Domains.Forms;
using rock.Framework.Autofac;
namespace rock.Core.Services.Documents
{
  public interface IDocumentService : IScopedDependency
  {
    Task<Document> GetDocumentById(int id, CancellationToken cancellationToken, IInclude<Document> include = null);
    IQueryable<Document> GetDocuments(IInclude<Document> include = null);
    Task<Document> CreateDocument(Form form, string description, CancellationToken cancellationToken);
    Task<Document> CreateDocumentByUserId(Form form, int userId, string description, CancellationToken cancellationToken);
  }
}