using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using PagedList;

using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Controllers
{
    public class HTTPTraceListenerController : DbController<HTTPTraceListener>
    { 
        //
        // GET: /HTTPTraceListener/

        public ActionResult Index(int page = 1)
        {
            var traces = UnitOfWork.Db.Set<HTTPTraceListener>();

            return View(traces.OrderByDescending(o => o.TraceDate).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }


        //
        // GET: /HTTPTraceListener/id
        public ActionResult Details(long? id)
        {
            var trace = DbRepository.Find(id);
            if (trace == null)
            {
                return new HttpNotFoundResult();
            }

            return View(trace);
        }

    }
}
