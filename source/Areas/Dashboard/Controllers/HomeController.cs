using System.Web.Mvc;

using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;

namespace TFax.Web.Areas.Dashboard.Controllers
{
    [AppAuthorize]
    [HttpTraceFilter]
    public class HomeController : AppController
    {
        //
        // GET: /Dashboard/Home/
        public ActionResult Index()
        {
            return View();
        }
    }
}