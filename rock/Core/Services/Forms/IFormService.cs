using rock.Framework.Autofac;
using rock.Core.Domains.Forms;
using System.Linq;
using System.Threading.Tasks;
using rock.Core.Data;
using System.Threading;
namespace rock.Core.Services.Forms
{
  public interface IFormService : IScopedDependency
  {
    Task<Form> GetFormById(int id, CancellationToken cancellationToken, IInclude<Form> include = null);
    IQueryable<Form> GetForms(IInclude<Form> include = null);
    Task<Form> InsertForm(Form form, CancellationToken cancellationToken);
    Task UpdateForm(Form form, CancellationToken cancellationToken);
    Task DeleteForm(Form form, CancellationToken cancellationToken);
  }
}