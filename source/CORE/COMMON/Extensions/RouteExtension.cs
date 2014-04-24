using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.Helpers;

namespace TFax.Web
{
    public static class RouteExtension
    {
        public static string GetCultureCode(this RouteValueDictionary values)
        {
            return values.ContainsKey("culture")
                ? (string)values["culture"]
                : CultureHelper.DefaultCulture;
        }

        public static long GetCultureId(this RouteValueDictionary values)
        {
            var code = values.GetCultureCode();
            var culture = CultureHelper.GetCulture(code);

            return culture.Id;
        }

    }
}