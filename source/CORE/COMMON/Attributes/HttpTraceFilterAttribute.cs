using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.Attributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HttpTraceFilterAttribute : ActionFilterAttribute
    {
        private readonly IBaseRepository<HTTPTraceListener> _repository;

        public HttpTraceFilterAttribute()
        {
            var unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
         
            _repository = new BaseRepository<HTTPTraceListener>(unitOfWork);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;

            _repository.Insert(new HTTPTraceListener
            {
                CategoryName = "OnActionExecuting",
                RequestUrl = request.Url.ToString(),
                TraceDate = DateTime.Now,
                TraceMessage = "On action executing",
                UserAgent = request.UserAgent,
                UserHostAddress = UtilHelper.GetVisitorIPAddress(),
                UserHostName = request.UserHostName,
                UserName = MembershipHelper.Current.UserName
            });

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;

            _repository.Insert(new HTTPTraceListener
            {
                CategoryName = "OnActionExecuted",
                RequestUrl = request.Url.ToString(),
                TraceDate = DateTime.Now,
                TraceMessage = "On action executed",
                UserAgent = request.UserAgent,
                UserHostAddress = UtilHelper.GetVisitorIPAddress(),
                UserHostName = request.UserHostName,
                UserName = MembershipHelper.Current.UserName
            });

            base.OnActionExecuted(filterContext);
        }
    }
}