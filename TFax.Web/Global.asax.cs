using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using TFax.Web.CORE;
using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.DAL;

namespace TFax.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IOCConfig.Initialize();
            SmtpConfig.Configure();
            AutoMapperConfig.Initialize(); 
        }

        protected void Application_Error()
        {
            var exp = Server.GetLastError();

            if (exp == null)
                return;

            using (var unitOfWork = new UnitOfWork())
            {
                var repository = new BaseRepository<HTTPTraceListener>(unitOfWork);

                repository.Insert(new HTTPTraceListener
                {
                    CategoryName = "OnException",
                    RequestUrl = Request.Url.ToString(),
                    TraceDate = DateTime.Now,
                    TraceMessage = String.Format("Exception - {0} ", exp.Message),
                    UserAgent = Request.UserAgent,
                    UserHostAddress = UtilHelper.GetVisitorIPAddress(),
                    UserHostName = Request.UserHostName,
                    UserName = MembershipHelper.Current.UserName
                });

                EmailHelper.SendExceptionEmail(exp);

            }
        }

    }
}