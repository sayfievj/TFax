using System.Web;

namespace TFax.Web.CORE.COMMON.Helpers
{
    public class SessionHelper
    {
        public static T GetStore<T>(string key)
        {
            return HttpContext.Current.Session[key] != null
                ? (T)HttpContext.Current.Session[key]
                : default(T);
        }

        public static void SetStore<T>(T t, string key)
        {
            HttpContext.Current.Session[key] = t;
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Remove(key);
        }

    }
}