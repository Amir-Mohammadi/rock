using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Payment;
using rock.Core.Services.Payment;
using rock.Models.PaymentApi;

namespace rock.Factories
{
  public class PaymentFactory : BaseFactory, IPaymentFactory
  {

    private readonly IPaymentGatewayService paymentGatewayService;

    public PaymentFactory(IPaymentGatewayService paymentGatewayService)
    {
      this.paymentGatewayService = paymentGatewayService;
    }
    public async Task<IList<PaymentGatewayModel>> PreparePaymentGatewayListModel(PaymentGatewaySearchParameters parameters,
                                                                             CancellationToken cancellationToken)
    {
      var paymentGateways = paymentGatewayService.GetPaymentGateways(gateway: parameters.Gateway,
                                                            include: new Include<PaymentGateway>(query =>
                                                            {
                                                              query = query.Include(x => x.Image);
                                                              query = query.Include(x => x.FinancialAccount);
                                                              query = query.Include(x => x.FinancialAccount).ThenInclude(x => x.Bank);
                                                              query = query.Include(x => x.FinancialAccount).ThenInclude(x => x.Profile);
                                                              query = query.Include(x => x.FinancialAccount).ThenInclude(x => x.Profile).ThenInclude(x => x.PersonProfile);
                                                              return query;
                                                            }));

      return await this.CreateModelPagedListAsync(source: paymentGateways,
                                             convertFunction: this.createPaymentGatewayModel,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             sortBy: parameters.SortBy,
                                             cancellationToken: cancellationToken);
    }

    public async Task<IList<BriefPaymentGatewayModel>> PrepareBriefPaymentGatewayListModel(PaymentGatewaySearchParameters parameters,
                                                                             CancellationToken cancellationToken)
    {
      var paymentGateways = paymentGatewayService.GetPaymentGateways(gateway: parameters.Gateway,
                                                            include: new Include<PaymentGateway>(query =>
                                                            {
                                                              query = query.Include(x => x.Image);
                                                              return query;
                                                            }));

      return await this.CreateModelPagedListAsync(source: paymentGateways,
                                             convertFunction: this.createBriefPaymentGatewayModel,
                                             order: parameters.Order,
                                             pageIndex: parameters.PageIndex,
                                             pageSize: parameters.PageSize,
                                             sortBy: parameters.SortBy,
                                             cancellationToken: cancellationToken);
    }

    private BriefPaymentGatewayModel createBriefPaymentGatewayModel(PaymentGateway paymentGateway)
    {
      if (paymentGateway == null)
        return null;
      return new BriefPaymentGatewayModel
      {
        Gateway = paymentGateway.Gateway,
        ImageAlt = paymentGateway.ImageAlt,
        ImageTitle = paymentGateway.ImageTitle,
        ImageId = paymentGateway.ImageId,
        ImageRowVersion = paymentGateway.RowVersion
      };
    }

    private PaymentGatewayModel createPaymentGatewayModel(PaymentGateway paymentGateway)
    {
      if (paymentGateway == null)
        return null;
      return new PaymentGatewayModel
      {
        Gateway = paymentGateway.Gateway,
        FinancialAccountId = paymentGateway.FinancialAccountId,
        FinancialAccountTitle = paymentGateway.FinancialAccount.Title,
        FistName = paymentGateway.FinancialAccount.Profile.PersonProfile.FirstName,
        LastName = paymentGateway.FinancialAccount.Profile.PersonProfile.LastName,
        ImageAlt = paymentGateway.ImageAlt,
        ImageTitle = paymentGateway.ImageTitle,
        ImageId = paymentGateway.ImageId,
        ImageRowVersion = paymentGateway.Image.RowVersion,
        BankName = paymentGateway.FinancialAccount.Bank.Name,
        RowVersion = paymentGateway.RowVersion
      };
    }
  }
}