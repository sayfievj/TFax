using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Controllers
{
    [HttpTraceFilter]
    public class MemberMasterController : DbController<MemberMaster>
    {
         

        //
        // GET: /Member/
        public ActionResult Search(int page = 1, int size = AppSettings.SCAFFOLD.PAGE_SIZE)
        {
            var model = new SearchTutorViewModel
            {
                Objects = DbRepository.FindAll(f => f.MemberRoleId == (long?)Role.Tutor)
                        .OrderByDescending(o => o.RegisteredDate)
                        .ToPagedList(page, size)
            };
             
            return View(model);
        }


    }
}