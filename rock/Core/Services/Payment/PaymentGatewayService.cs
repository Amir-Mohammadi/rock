using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Data;
using rock.Core.Domains.Files;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Payment;
using rock.Core.Errors;
using rock.Core.Services.Files;

namespace rock.Core.Services.Payment
{
  public class PaymentGatewayService : IPaymentGatewayService
  {
    private IRepository<PaymentGateway> paymentGatewayRepository;
    private readonly IFileService fileService;
    private readonly IErrorFactory errorFactory;
    public PaymentGatewayService(IRepository<PaymentGateway> paymentGatewayRepository, IFileService fileService, IErrorFactory errorFactory)
    {
      this.paymentGatewayRepository = paymentGatewayRepository;
      this.fileService = fileService;
      this.errorFactory = errorFactory;
    }
    public async Task<PaymentGateway> GetPaymentGateway(string gateway, CancellationToken cancellationToken, Include<PaymentGateway> include = null)
    {
      return await paymentGatewayRepository.GetAsync(predicate: x => x.Gateway == gateway && x.DeletedAt == null, cancellationToken: cancellationToken, include: include);
    }

    public IQueryable<PaymentGateway> GetPaymentGateways(string gateway = null, Include<PaymentGateway> include = null)
    {
      var query = paymentGatewayRepository.GetQuery(include: include).Where(x => x.DeletedAt == null);
      if (gateway != null)
      {
        query.Where(x => x.Gateway == gateway);
      }
      return query;
    }

    public async Task InsertPaymentGateway(PaymentGateway paymentGateway, FinancialAccount financialAccount, CancellationToken cancellationToken)
    {
      await checkExisitPaymentGateway(paymentGateway.Gateway, cancellationToken: cancellationToken);

      if (paymentGateway.IsDefault)
      {
        var paymentGateways = await GetPaymentGateways().Where(x => x.IsDefault == true).ToListAsync(cancellationToken: cancellationToken);

        foreach (var item in paymentGateways)
        {
          item.IsDefault = false;
          await paymentGatewayRepository.UpdateAsync(entity: item, cancellationToken: cancellationToken);
        }
      }

      var image = await insertPaymentGatewayImage(imageId: paymentGateway.ImageId,
                                                  cancellationToken: cancellationToken);

      paymentGateway.Image = image;
      paymentGateway.FinancialAccount = financialAccount;
      paymentGateway.CreatedAt = DateTime.UtcNow;
      await paymentGatewayRepository.AddAsync(entity: paymentGateway, cancellationToken: cancellationToken);
    }
    private async Task<File> insertPaymentGatewayImage(Guid imageId, CancellationToken cancellationToken)
    {
      var image = new File();
      image.Id = imageId;
      image.Access = FileAccessType.Public;
      image.OwnerGroup = Domains.Users.UserRole.None;
      image = await fileService.InsertFile(file: image,
                                           cancellationToken: cancellationToken);
      return image;
    }

    private async Task checkExisitPaymentGateway(string gateway, CancellationToken cancellationToken)
    {
      var paymentGateway = await GetPaymentGateway(gateway: gateway, cancellationToken: cancellationToken);
      if (paymentGateway != null)
      {
        throw errorFactory.ThePaymentGatewayDefinedBefore();
      }
    }
    public async Task UpdatePaymentGateway(PaymentGateway paymentGateway, CancellationToken cancellationToken)
    {
      await paymentGatewayRepository.UpdateAsync(entity: paymentGateway, cancellationToken: cancellationToken);
    }

    public async Task DeletePaymentGateway(PaymentGateway paymentGateway, CancellationToken cancellationToken)
    {
      paymentGateway.DeletedAt = DateTime.UtcNow;
      await paymentGatewayRepository.UpdateAsync(entity: paymentGateway, cancellationToken: cancellationToken);
    }

    public async Task SetDefaultPaymentGateway(PaymentGateway paymentGateway, CancellationToken cancellationToken)
    {
      var paymentGateways = await GetPaymentGateways().Where(x => x.IsDefault == true).ToListAsync(cancellationToken: cancellationToken);

      foreach (var item in paymentGateways)
      {
        item.IsDefault = false;
        await paymentGatewayRepository.UpdateAsync(entity: item, cancellationToken: cancellationToken);
      }

      paymentGateway.IsDefault = true;
      await paymentGatewayRepository.UpdateAsync(entity: paymentGateway, cancellationToken: cancellationToken);
    }
  }
}