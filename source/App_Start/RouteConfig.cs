using System.Web.Mvc;
using System.Web.Routing;

using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Helpers;

namespace TFax.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Culture",
              url: "{culture}/{controller}/{action}/{id}",
              defaults: new { culture = CultureHelper.DefaultCulture, controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { AppSettings.ROUTE.DEFAULT_CONTROLLER_NAMESPACE });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { AppSettings.ROUTE.DEFAULT_CONTROLLER_NAMESPACE });

        }
    }
}
