using System.Web.Mvc;

using TFax.Web.CORE.COMMON.Attributes;

namespace TFax.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        { 
            filters.Add(new HandleErrorAttribute()); 
        }
    }
}
