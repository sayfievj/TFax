using System.Web.Mvc;

using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Helpers;

namespace TFax.Web.Areas.Dashboard
{
    public class DashboardAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              name: "Dashboard_culture",
              url: "{culture}/Dashboard/{controller}/{action}/{id}",
              defaults: new { culture = CultureHelper.DefaultCulture, controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { AppSettings.ROUTE.DASHBOARD_ARE_CONTROLLER_NAMESPACE }
              );

            context.MapRoute(
                name: "Dashboard_default",
                url: "Dashboard/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { AppSettings.ROUTE.DASHBOARD_ARE_CONTROLLER_NAMESPACE }
                );
        }
    }
}