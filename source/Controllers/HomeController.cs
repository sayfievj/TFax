using System.Collections.Generic;
using System.Web.Mvc;

using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers; 
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Controllers
{
    [HttpTraceFilter]
    public class HomeController : AppController
    {

        public ActionResult Index()
        {
            return View();
        } 
       

    }
}