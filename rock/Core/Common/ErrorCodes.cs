using System.Linq;
namespace rock.Core.Common
{
  public class ErrorCodes
  {
    #region Error code name resolver
    public static string Resolve(int errorCode)
    {
      var errorCodeType = typeof(ErrorCodes);
      var dictionary = errorCodeType.GetFields().Select(x => new { Name = x.Name, Value = x.GetValue(x.Name).ToString() });
      return dictionary.FirstOrDefault(x => x.Value == errorCode.ToString()).Name;
    }
    #endregion
    public const int INTERNAL_SERVER_ERROR = 1000;
    public const int OBJECT_NOT_FOUND = 1001;
    public const int ROW_VERSION_MISMATCH = 1002;
    public const int ACCESS_DENIED = 1003;
    public const int INVALID_CREDENTIALS = 1005;
    public const int BAD_VERIFICATION_CODE = 1006;
    public const int INVALID_AUTHENTICATED_USER = 1007;
    public const int EMAIL_IS_ALREADY_VERIFIED = 1008;
    public const int PHONE_IS_ALREADY_VERIFIED = 1009;
    public const int INVALID_EMAIL = 1010;
    public const int INVALID_PASSWORD_LENGTH = 1011;
    public const int PASSWORDS_DOSE_NOT_MATCH = 1012;
    public const int INVALID_PHONE_NUMBER = 1013;
    public const int AUTH_UNKNOWN_REACTION = 1014;
    public const int INVALID_MODEL_SUPPLIED = 1015;
    public const int RESOURCE_NOT_FOUND = 1016;
    public const int CANNOT_CHANGE_YOUR_ROLES = 1017;
    public const int INVALID_PROFILE_SUPPLIED = 1018;
    public const int INVALID_CREDENTIAL_TYPE = 1019;
    public const int INVALID_EMAIL_CREDENTIAL = 1020;
    public const int INVALID_PHONE_CREDENTIAL = 1021;
    public const int INVALID_TOKEN = 1022;
    public const int USER_HAS_BEEN_DISABLED = 1023;
    public const int PROFILE_WITH_CURRENT_EMAIL_EXISTS = 1024;
    public const int PROFILE_WITH_CURRENT_PHONE_EXISTS = 1025;
    public const int PAYMENT_GATEWAY_IS_NOT_CONFIGURED = 1026;
    public const int COUPON_IS_NOT_ACTIVE = 1027;
    public const int COUPON_IS_EXPIRED = 1028;
    public const int COUPON_USAGE_IS_FINISHED = 1029;
    public const int COUPON_IS_USED = 1030;
    public const int SHOP_STUFF_PRICE_IS_OUT_OF_RANGE = 1031;
    public const int THIS_USER_IS_ALTERED = 1031;
    public const int INVALID_VERIFICATION_CODE = 1032;
    public const int INVALID_VERIFICATION_Token = 1033;
    public const int INVALID_PHONE_TO_VERIFICATION = 1034;
    public const int INVALID_LOGIN_ID = 1035;
    public const int EMAIL_IS_NOT_REGISTERED = 1036;
    public const int INVALID_PASSWORD = 1037;
    public const int INVALID_AUTHENTICATE_TYPE = 1038;
    public const int INVALID_COUPON = 1039;
    public const int INVALID_PAYMENT_PREVIEW = 1040;
    public const int INVALID_ORDER_TO_PAY = 1041;
    public const int NOT_AVAILABLE_SHIPPING_FOR_ROUTE = 1042;
    public const int INVALID_PRODUCT_PRICE = 1043;
    public const int LESS_THAN_MINIMUM_ORDER_PAYABLE_PRICE = 1044;
    public const int The_Entered_National_Code_Is_Invalid = 1045;
    public const int LESS_THAN_MINIMUM_INVENTORY_AMOUNT = 1046;
    public const int THE_PAYMENT_GATEWAY_DEFINED_BEFORE = 1047;
    public const int COUPON_CODE_IS_DUPLICATE = 1048;
    public const int XSS_DETECT = 1049;
  }
}