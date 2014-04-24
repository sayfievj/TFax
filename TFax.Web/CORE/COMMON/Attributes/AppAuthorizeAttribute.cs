using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Enums;

namespace TFax.Web.CORE.COMMON.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        private Role[] _roles;

        public AppAuthorizeAttribute()
        {
            _roles = new Role[] { };
        }

        public AppAuthorizeAttribute(ref Role[] role)
        {
            _roles = role;
        }

        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="httpContext"/> parameter is null.</exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return MembershipHelper.Current.IsAuthenticated;
        }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute"/>.</param><exception cref="T:System.ArgumentNullException">The <paramref name="filterContext"/> parameter is null.</exception>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
              || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                return;

            if (!AuthorizeCore(filterContext.HttpContext))
                HandleUnauthorizedRequest(filterContext);
            else
            {
                if (!_roles.Any())
                    return;

                if (_roles.Any(a => a == MembershipHelper.Current.Role))
                    return;

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    { 
                      { "area", "" },
                      { "controller", "Error" },
                      { "action", "AccessDenied" }
                    });
            }
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute"/>. The <paramref name="filterContext"/> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "area", "" },
                { "controller", "Account" },
                { "action", "Login" },
                { "returnUrl", filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath }
            });
        }

        /// <summary>
        /// Called when the caching module requests authorization.
        /// </summary>
        /// <returns>
        /// A reference to the validation status.
        /// </returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="httpContext"/> parameter is null.</exception>
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return MembershipHelper.Current.IsAuthenticated
                ? HttpValidationStatus.Valid
                : HttpValidationStatus.Invalid;
        }

    }
}