using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Serialization;
using rock.Core.Common;
using rock.Core.Common.Exception;
using rock.Core.Services.Users;
using rock.Framework.ExceptionHandler;
namespace rock.Core.Errors
{
  public class ErrorFactory : IErrorFactory
  {
    #region Defs
    private NamingStrategy namingStrategy => new SnakeCaseNamingStrategy();
    private IExceptionFactory<AppException> factory;
    #endregion
    public ErrorFactory(IExceptionFactory<AppException> factory)
    {
      this.factory = factory;
    }
    public object InvalidModelSupplied(ModelStateDictionary state)
    {
      var additionalData = state.Select(s => new
      {
        Field = namingStrategy.GetPropertyName(s.Key, false),
        Messages = s.Value.Errors.Select(e => e.ErrorMessage.Replace(s.Key, namingStrategy.GetPropertyName(s.Key, false)))
      });
      var response = new { Code = 0x00001, Info = additionalData };
      return factory.Failed().UseCode(ErrorCodes.INVALID_MODEL_SUPPLIED).UseInfo(response).AsPayload();
    }
    public Exception AccessDenied()
    {
      return factory.Unauthorized().UseCode(ErrorCodes.ACCESS_DENIED).AsException();
    }
    public Exception ResourceNotFound(object id, string name = null)
    {
      var info = new
      {
        Id = id,
        Name = name
      };
      return factory.NotFound().UseCode(ErrorCodes.RESOURCE_NOT_FOUND).UseInfo(info).AsException();
    }
    public Exception CannotChangeYourRoles()
    {
      return factory.NotFound()
                    .UseCode(ErrorCodes.CANNOT_CHANGE_YOUR_ROLES)
                    .AsException();
    }
    public Exception InvalidPasswordRange()
    {
      return factory.NotFound()
                    .UseCode(ErrorCodes.CANNOT_CHANGE_YOUR_ROLES)
                    .AsException();
    }
    public Exception InvalidPassword()
    {
      return factory.NotFound()
                    .UseCode(ErrorCodes.INVALID_PASSWORD)
                    .AsException();
    }
    public Exception InvalidAuthenticateType()
    {
      return factory.NotFound()
                    .UseCode(ErrorCodes.INVALID_PASSWORD)
                    .AsException();
    }
    public Exception InvalidCredentialsSupplied()
    {
      throw new NotImplementedException();
    }
    public Exception InvalidCredentialType(object credentials, CredentialType type)
    {
      var info = new
      {
        Context = credentials,
        Type = type
      };
      return factory.Failed()
      .UseCode(ErrorCodes.INVALID_CREDENTIAL_TYPE)
      .UseInfo(info)
      .AsException();
    }
    public Exception InvalidLoginId()
    {
      return factory.Failed()
      .UseCode(ErrorCodes.INVALID_LOGIN_ID)
      .AsException();
    }
    public Exception InvalidEmailCredentials()
    {
      return factory.Failed()
       .UseCode(ErrorCodes.INVALID_EMAIL_CREDENTIAL)
       .AsException();
    }
    public Exception InvalidPhoneCredentials()
    {
      return factory.Failed()
        .UseCode(ErrorCodes.INVALID_PHONE_CREDENTIAL)
        .AsException();
    }
    public Exception InvalidToken()
    {
      return factory.Failed()
                    .UseCode(ErrorCodes.INVALID_TOKEN)
                    .AsException();
    }
    public Exception UserHasBeenDisabled()
    {
      return factory.Forbidden().UseCode(ErrorCodes.USER_HAS_BEEN_DISABLED).AsException();
    }
    public Exception ProfileWithCurrentEmailExists()
    {
      return factory.Failed().UseCode(ErrorCodes.PROFILE_WITH_CURRENT_EMAIL_EXISTS).AsException();
    }
    public Exception TheEnteredNationalCodeIsInvalid()
    {
      return factory.Failed().UseCode(ErrorCodes.The_Entered_National_Code_Is_Invalid).AsException();
    }
    public Exception ProfileWithCurrentPhoneExists()
    {
      return factory.Failed().UseCode(ErrorCodes.PROFILE_WITH_CURRENT_PHONE_EXISTS).AsException();
    }
    public Exception PaymentGatewayIsNotConfigured()
    {
      return factory.Failed().UseCode(ErrorCodes.PAYMENT_GATEWAY_IS_NOT_CONFIGURED).UseInfo("Use `Load` method to select `PSP` provider.").AsException();
    }
    public Exception CouponIsNotActive()
    {
      return factory.Failed().UseCode(ErrorCodes.COUPON_IS_NOT_ACTIVE).AsException();
    }
    public Exception CouponIsExpired()
    {
      return factory.Failed().UseCode(ErrorCodes.COUPON_IS_EXPIRED).AsException();
    }
    public Exception CouponUsageIsFinished()
    {
      return factory.Failed().UseCode(ErrorCodes.COUPON_USAGE_IS_FINISHED).AsException();
    }
    public Exception CouponIsUsed()
    {
      return factory.Failed().UseCode(ErrorCodes.COUPON_IS_USED).AsException();
    }
    public Exception CouponDuplicateCode()
    {
      return factory.Failed().UseCode(ErrorCodes.COUPON_CODE_IS_DUPLICATE).AsException();
    }
    public Exception ShopStuffPriceIsOutOfRange()
    {
      return factory.Failed().UseCode(ErrorCodes.SHOP_STUFF_PRICE_IS_OUT_OF_RANGE).AsException();
    }
    public Exception ThisUserIsAltered()
    {
      return factory.Failed().UseCode(ErrorCodes.THIS_USER_IS_ALTERED).AsException();
    }
    public Exception InValidVerificationCode()
    {
      return factory.Failed().UseCode(ErrorCodes.INVALID_VERIFICATION_CODE).AsException();
    }
    public Exception InValidVerificationToken()
    {
      return factory.Failed().UseCode(ErrorCodes.INVALID_VERIFICATION_CODE).AsException();
    }
    public Exception InValidPhoneToVerification()
    {
      return factory.Failed().UseCode(ErrorCodes.INVALID_PHONE_TO_VERIFICATION).AsException();
    }
    public Exception EmailIsNotRegistered()
    {
      return factory.Failed().UseCode(ErrorCodes.EMAIL_IS_NOT_REGISTERED).AsException();
    }
    public Exception InvalidCoupon()
    {
      return factory.Failed().UseCode(ErrorCodes.INVALID_COUPON).AsException();
    }
    public Exception InvalidPaymentPreview()
    {
      return factory.NotFound().UseCode(ErrorCodes.INVALID_PAYMENT_PREVIEW).AsException();
    }
    public Exception InvalidOrderToPay()
    {
      return factory.Failed().UseCode(ErrorCodes.INVALID_ORDER_TO_PAY).AsException();
    }
    public Exception NotAvailableShippingForRoute()
    {
      return factory.Failed().UseCode(ErrorCodes.NOT_AVAILABLE_SHIPPING_FOR_ROUTE).AsException();
    }
    public Exception InvalidProductPrice()
    {
      return factory.Failed().UseCode(ErrorCodes.INVALID_PRODUCT_PRICE).AsException();
    }
    public Exception LessThanMinimumOrderPayablePrice()
    {
      return factory.Failed().UseCode(ErrorCodes.LESS_THAN_MINIMUM_ORDER_PAYABLE_PRICE).AsException();
    }
    public Exception LessThanMinimumInventoryAmount()
    {
      return factory.Failed().UseCode(ErrorCodes.LESS_THAN_MINIMUM_INVENTORY_AMOUNT).AsException();
    }
    public Exception ThePaymentGatewayDefinedBefore()
    {
      return factory.Failed().UseCode(ErrorCodes.THE_PAYMENT_GATEWAY_DEFINED_BEFORE).AsException();
    }
    public Exception XssDetect()
    {
      return factory.Failed().UseCode(ErrorCodes.XSS_DETECT).AsException();
    }
  }
}