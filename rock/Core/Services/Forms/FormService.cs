using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Forms;
namespace rock.Core.Services.Forms
{
  public partial class FormService : IFormService
  {
    #region  Fields
    private IRepository<Form> fromRepository;
    #endregion
    #region Constractor
    public FormService(IRepository<Form> fromRepository)
    {
      this.fromRepository = fromRepository;
    }
    #endregion
    #region Form
    public async Task<Form> GetFormById(int id, CancellationToken cancellationToken, IInclude<Form> include = null)
    {
      return await fromRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken, include: include);
    }
    public IQueryable<Form> GetForms(IInclude<Form> include = null)
    {
      var query = fromRepository.GetQuery();
      return query;
    }
    public async Task<Form> InsertForm(Form form, CancellationToken cancellationToken)
    {
      await fromRepository.AddAsync(entity: form, cancellationToken: cancellationToken);
      return form;
    }
    public async Task UpdateForm(Form form, CancellationToken cancellationToken)
    {
      await fromRepository.UpdateAsync(entity: form, cancellationToken: cancellationToken);
    }
    public async Task DeleteForm(Form form, CancellationToken cancellationToken)
    {
      await fromRepository.DeleteAsync(entity: form, cancellationToken: cancellationToken);
    }
    #endregion
  }
}