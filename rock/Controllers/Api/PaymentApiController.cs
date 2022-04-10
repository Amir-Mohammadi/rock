using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Services.Payment;

using rock.Models.PaymentApi;
using Microsoft.AspNetCore.Authorization;
using rock.Core.Services.Common;
using rock.Core.Services.Orders;
using rock.Core.Domains.Payment;
using rock.Core.Services.Financial;
using rock.Core.Errors;
using rock.Factories;

namespace rock.Controllers
{
  [Route("/api/v1")]
  [ApiController]
  public class PaymentApiController : BaseController
  {
    private readonly IPaymentService paymentService;
    private readonly IOrderPaymentService orderPaymentService;
    private readonly IPaymentGatewayService paymentGatewayService;
    private readonly IFinancialAccountService financialAccountService;
    private readonly IErrorFactory errorFactory;
    private readonly IPaymentFactory paymentFactory;
    public PaymentApiController(IPaymentService paymentService,
                                IOrderPaymentService orderPaymentService,
                                IPaymentGatewayService paymentGatewayService,
                                IFinancialAccountService financialAccountService,
                                IErrorFactory errorFactory,
                                IPaymentFactory paymentFactory)
    {
      this.paymentService = paymentService;
      this.orderPaymentService = orderPaymentService;
      this.financialAccountService = financialAccountService;
      this.errorFactory = errorFactory;
      this.paymentFactory = paymentFactory;
      this.paymentGatewayService = paymentGatewayService;
    }

    #region  payment gateway
    [Authorize]
    [HttpPost("payments/gateways")]
    public async Task CreatePaymentGateway([FromBody] CreatePaymentGatewayModel model, CancellationToken cancellationToken)
    {
      var paymentGateway = this.mapPaymentGateway(model);

      var financialAccount = await financialAccountService.GetFinancialAccountById(id: model.FinancialAccountId, cancellationToken: cancellationToken);
      if (financialAccount == null)
      {
        errorFactory.ResourceNotFound(model.FinancialAccountId);
      }

      await paymentGatewayService.InsertPaymentGateway(paymentGateway: paymentGateway,
                                                       financialAccount: financialAccount,
                                                       cancellationToken: cancellationToken);
    }

    [Authorize]
    [HttpGet("payments/gateways")]
    public async Task<IList<PaymentGatewayModel>> GetPaymentGateways([FromQuery] PaymentGatewaySearchParameters parameters, CancellationToken cancellationToken)
    {
      return await paymentFactory.PreparePaymentGatewayListModel(parameters: parameters, cancellationToken: cancellationToken);
    }

    [HttpGet("payments/gateways/brief")]
    public async Task<IList<BriefPaymentGatewayModel>> GetBriefPaymentGateway([FromQuery] PaymentGatewaySearchParameters parameters, CancellationToken cancellationToken)
    {
      return await paymentFactory.PrepareBriefPaymentGatewayListModel(parameters: parameters, cancellationToken: cancellationToken);
    }
    [Authorize]
    [HttpPost("payments/gateways/{gateway}/set-default")]
    public async Task SetDefaultPaymentGateway([FromRoute] string gateway, CancellationToken cancellationToken)
    {

      var paymentGateway = await paymentGatewayService.GetPaymentGateway(gateway: gateway, cancellationToken: cancellationToken);
      if (paymentGateway == null)
      {
        errorFactory.ResourceNotFound(gateway);
      }

      await paymentGatewayService.SetDefaultPaymentGateway(paymentGateway: paymentGateway, cancellationToken: cancellationToken);
    }

    [Authorize]
    [HttpDelete("payments/gateways/{gateway}")]
    public async Task DeletePaymentGateway([FromRoute] string gateway, CancellationToken cancellationToken)
    {

      var paymentGateway = await paymentGatewayService.GetPaymentGateway(gateway: gateway, cancellationToken: cancellationToken);
      if (paymentGateway == null)
      {
        errorFactory.ResourceNotFound(gateway);
      }

      await paymentGatewayService.DeletePaymentGateway(paymentGateway: paymentGateway, cancellationToken: cancellationToken);
    }
    private PaymentGateway mapPaymentGateway(CreatePaymentGatewayModel model)
    {
      return new PaymentGateway
      {
        Gateway = model.Gateway,
        ImageAlt = model.ImageAlt,
        ImageTitle = model.ImageTitle,
        IsDefault = model.IsDefault,
        ImageId = model.ImageId
      };
    }
    #endregion

    [Authorize]
    [HttpGet("payments/order-payment-visit/{orderPaymentId}")]
    public async Task CheckOrderPaymentVisit([FromRoute] int orderPaymentId, CancellationToken cancellationToken)
    {
      await orderPaymentService.CheckOrderPaymentVisit(orderPaymentId: orderPaymentId, cancellationToken: cancellationToken);
    }

    [Authorize]
    [HttpPatch("payments/order-payment-visit/{orderPaymentId}")]
    public async Task SetOrderPaymentVisit([FromRoute] int orderPaymentId, CancellationToken cancellationToken)
    {
      await orderPaymentService.SetOrderPaymentVisit(orderPaymentId: orderPaymentId, cancellationToken: cancellationToken);
    }


    [Authorize]
    [HttpPost("payments")]
    public async Task<ContentResult> Purchase([FromBody] PaymentModel payment, CancellationToken cancellationToken)
    {
      var html = await paymentService.Start(payment.Gateway, payment.Kind, extraParams: payment.ExtraParams, cancellationToken);
      return Content(html, "text/html");
    }

    [HttpPost("payments/{paymentId}/verify")]
    [Consumes("application/x-www-form-urlencoded")]
    [AllowAnonymous]
    public async Task<RedirectResult> Verify([FromRoute] int paymentId, [FromForm] Dictionary<string, string> parameters, CancellationToken cancellationToken)
    {
      var url = await paymentService.Finalize(paymentId, parameters, cancellationToken);
      return Redirect(url);
    }

  }
}