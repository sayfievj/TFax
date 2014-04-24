namespace TFax.Web.CORE
{
    public static class AppSettings
    {

        public const string APP_TITLE = "TUTORFAX";

        public struct STORE
        {
            public const string LOGIN_STORE_KEY = "_tfax_login_store_key_";
            public const string COUNRY_STORE_KEY = "_tfax_country_store_key_";
        }

        public struct SCAFFOLD
        {
            public const int PAGE_SIZE = 25;
        }

        public struct ROUTE
        { 
            public const string DEFAULT_CONTROLLER_NAMESPACE = "TFax.Web.Controllers";
            public const string DASHBOARD_ARE_CONTROLLER_NAMESPACE = "TFax.Web.Areas.Dashboard.Controllers";
        }

        public struct KEYS
        {
            public const string ACCOUNT_ACTIVATION_EXPIRED_DAYS = "AccountActivationExpiredDays";
            public const string SEND_EXCEPTION_EMAIL_ENABLED = "SendExceptionEmailEnabled";
        }

    }
}