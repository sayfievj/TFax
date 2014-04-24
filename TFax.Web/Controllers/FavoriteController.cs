using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFax.Web.CORE.COMMON.Controllers;

namespace TFax.Web.Controllers
{
    public class FavoriteController : AppController
    {
        //
        // GET: /Favorite/

        public ActionResult Index()
        {
            return View();
        }

    }
}
