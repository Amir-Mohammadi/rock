using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Domains.Payment;
using rock.Core.Domains.Forms;
using rock.Core.Services.Forms;
using rock.Core.Services.Payment;
using System;

namespace rock.Core.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private readonly IFormService formService;
    private readonly IPaymentGatewayService paymentGatewayService;

    public StaticDataService(IFormService formService, IPaymentGatewayService paymentGatewayService)
    {
      this.formService = formService;
      this.paymentGatewayService = paymentGatewayService;
    }

    public async Task Init()
    {
      var cancellationToken = new CancellationToken();
      await initStaticForms(cancellationToken: cancellationToken);
    }
    private async Task initStaticForms(CancellationToken cancellationToken)
    {

      IList<Form> forms = typeof(StaticData.Forms).GetProperties(bindingAttr: BindingFlags.Public | BindingFlags.Static)
                                                  .Select(x => (Form)x.GetValue(null, null))
                                                  .ToList();

      var oldForms = await formService.GetForms().ToListAsync(cancellationToken);
      foreach (var form in forms)
      {
        var oldForm = oldForms.FirstOrDefault(x => x.Id == form.Id);
        if (oldForm == null)
          await formService.InsertForm(form: form, cancellationToken: cancellationToken);
        else
        {
          if (oldForm.Title != form.Title
              | oldForm.Description != form.Description
              | oldForm.FormOptions != form.FormOptions)
          {
            oldForm.Title = form.Title;
            oldForm.Description = form.Description;
            oldForm.FormOptions = form.FormOptions;
            await formService.UpdateForm(form: oldForm, cancellationToken: cancellationToken);
          }
        }
      }
    }
  }
}