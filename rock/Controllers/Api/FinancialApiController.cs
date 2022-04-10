using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Common;
using rock.Core.Domains.Financial;
using rock.Core.Errors;
using rock.Core.Services.Financial;
using rock.Factories;
using rock.Models;
using rock.Models.FinancialApi;
namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  [AllowAnonymous]
  public class FinancialApiController : BaseController
  {
    #region Fields
    private readonly IFinancialAccountService financialAccountService;
    private readonly IFinancialFactory factory;
    private readonly IErrorFactory errorFactory;
    #endregion
    #region Constractor
    public FinancialApiController(IFinancialAccountService financialAccountService,
                                  IFinancialFactory factory,
                                  IErrorFactory errorFactory)
    {
      this.financialAccountService = financialAccountService;
      this.factory = factory;
      this.errorFactory = errorFactory;
    }
    #endregion
    [HttpPost("financial-accounts")]
    public async Task<Key<int>> CreateFinancialAccount([FromBody] CreateFinancialAccountModel newAccount, CancellationToken cancellationToken)
    {
      var account = new FinancialAccount();
      this.mapCreateFinancialAccount(account, newAccount);
      await financialAccountService.InsertFinancialAccount(financialAccount: account, cancellationToken: cancellationToken);
      return new Key<int>(account.Id);
    }

    [HttpPatch("financial-accounts/{financialAccountId}")]
    public async Task EditFinancialAccount([FromRoute] int financialAccountId, [FromBody] UpdateFinancialAccountModel account, CancellationToken cancellationToken)
    {
      var currentAccount = await financialAccountService.GetFinancialAccountById(id: financialAccountId, cancellationToken: cancellationToken, include: null);

      if (currentAccount == null)
        errorFactory.ResourceNotFound(id: financialAccountId);

      this.mapUpdateFinancialAccount(currentAccount, account);

      await financialAccountService.UpdateFinancialAccount(financialAccount: currentAccount, cancellationToken: cancellationToken);

    }

    [HttpGet("financial-accounts/{financialAccountId}")]
    public async Task<FinancialAccountModel> GetFinancialAccount([FromRoute] int financialAccountId, CancellationToken cancellationToken)
    {
      var financialAccount = await factory.PrepareFinancialAccountModel(financialAccountId: financialAccountId, cancellationToken: cancellationToken);
      if (financialAccount == null)
        errorFactory.ResourceNotFound(id: financialAccountId);
      return financialAccount;
    }

    [HttpDelete("financial-accounts/{financialAccountId}")]
    public async Task DeleteFinancialAccount([FromRoute] int financialAccountId, CancellationToken cancellationToken)
    {
      var financialAccount = await financialAccountService.GetFinancialAccountById(id: financialAccountId, cancellationToken: cancellationToken, include: null);
      if (financialAccount == null)
        errorFactory.ResourceNotFound(id: financialAccountId);

      await financialAccountService.DeleteFinancialAccount(financialAccount: financialAccount, cancellationToken: cancellationToken);
    }
    [HttpGet("financial-accounts")]
    public async Task<IList<FinancialAccountModel>> GetPagedFinancialAccounts([FromQuery] FinancialAccountSearchParameters parameters, CancellationToken cancellationToken)
    {
      return await factory.PrepareFinancialAccountListModel(parameters: parameters, cancellationToken: cancellationToken);
    }
    [HttpGet("financial-accounts/{financialAccountId}/transactions")]
    public IPagedList<FinancialTransactionModel> GetFinancialAccountTransactions([FromRoute] int financialAccountId, [FromQuery] FinancialTransactionSearchParameters parameters) { return null; }
    [HttpGet("bills/{billId}")]
    public BillModel GetBill([FromRoute] int billId) { return null; }
    [HttpGet("bills")]
    public IPagedList<BillModel> GetPagedBills(BillSearchParameters parameters) { return null; }
    [HttpGet("purchases/{purchaseId}")]
    public PurchaseModel GetPurchase([FromRoute] int purchaseId) { return null; }
    [HttpGet("purchases")]
    public IPagedList<PurchaseModel> GetPagedPurchase(PurchaseSearchParamters paramters) { return null; }
    [HttpGet("financial-forms")]
    public IList<FinancialFormModel> GetFinancialForms() { return null; }
    [HttpPost("financial-documents")]
    public void CreateDocument([FromBody] DocumentModel document) { }
    [HttpGet("financial-documents/{DocumentId}")]
    public DocumentModel GetDocument([FromRoute] int DocumentId) { return null; }
    [HttpGet("financial-documents")]
    public IPagedList<DocumentModel> GetPagedDocuments([FromQuery] DocumentSearchParameters parameters) { return null; }
    private void mapCreateFinancialAccount(FinancialAccount account, CreateFinancialAccountModel model)
    {
      account.CurrencyId = model.CurrencyId;
      account.BankId = model.BankId;
      account.ProfileId = model.ProfileId;
      account.Title = model.Title;
      account.Type = model.Type;
      account.No = model.No;
    }
    private void mapUpdateFinancialAccount(FinancialAccount account, UpdateFinancialAccountModel model)
    {
      account.CurrencyId = model.CurrencyId;
      account.BankId = model.BankId;
      account.ProfileId = model.ProfileId;
      account.Title = model.Title;
      account.Type = model.Type;
      account.No = model.No;
      account.RowVersion = model.RowVersion;
    }
  }
}