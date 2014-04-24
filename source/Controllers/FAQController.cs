using System;
using System.Linq;
using System.Web.Mvc;

using PagedList;

using TFax.Web.CORE;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.DAL;
using TFax.Web.CORE.COMMON.Enums;

namespace TFax.Web.Controllers
{
    [HttpTraceFilter]
    public class FAQController : DbController<TextContent>
    {
        //
        // GET: /FAQ/
        public ActionResult Index(int page = 1)
        {
            var cultureId=RouteData.Values.GetCultureId();
            var faqs = DbRepository.FindAll(f => f.StatusId == (long)ContentStatus.FAQ_CONTENT && f.LanguageId == cultureId);

            return View(faqs.OrderByDescending(o => o.Id).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }

    }
}