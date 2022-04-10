using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using rock.Core.Services.Users;
using rock.Framework.Autofac;
using rock.Framework.ExceptionHandler;
namespace rock.Core.Errors
{
  public interface IErrorFactory : ISingletonDependency
  {
    object InvalidModelSupplied(ModelStateDictionary state);
    Exception PaymentGatewayIsNotConfigured();
    Exception ResourceNotFound(object id, string name = null);
    Exception CannotChangeYourRoles();
    Exception InvalidPasswordRange();
    Exception InvalidCredentialsSupplied();
    Exception InvalidCredentialType(object credentials, CredentialType type);
    Exception InvalidEmailCredentials();
    Exception InvalidPhoneCredentials();
    Exception AccessDenied();
    Exception InvalidToken();
    Exception UserHasBeenDisabled();
    Exception ProfileWithCurrentEmailExists();
    Exception ProfileWithCurrentPhoneExists();
    Exception TheEnteredNationalCodeIsInvalid();
    Exception CouponIsNotActive();
    Exception CouponIsExpired();
    Exception CouponUsageIsFinished();
    Exception CouponIsUsed();
    Exception CouponDuplicateCode();
    Exception ShopStuffPriceIsOutOfRange();
    Exception ThisUserIsAltered();
    Exception InValidVerificationCode();
    Exception InValidVerificationToken();
    Exception InValidPhoneToVerification();
    Exception InvalidLoginId();
    Exception EmailIsNotRegistered();
    Exception InvalidAuthenticateType();
    Exception InvalidPassword();
    Exception InvalidCoupon();
    Exception InvalidPaymentPreview();
    Exception InvalidOrderToPay();
    Exception NotAvailableShippingForRoute();
    Exception InvalidProductPrice();
    Exception LessThanMinimumOrderPayablePrice();
    Exception LessThanMinimumInventoryAmount();
    Exception ThePaymentGatewayDefinedBefore();
    Exception XssDetect();
  }
}