using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Helpers;

namespace TFax.Web.Controllers
{
    public class ErrorController : AppController
    {

        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult Render(long? id)
        {
            var path = String.Format("~/Views/Error/Status/{0}.cshtml", id);

            if (System.IO.File.Exists(Server.MapPath(path)))
                return PartialView(path);

            return RedirectToAction("Index");
        }

    }
}