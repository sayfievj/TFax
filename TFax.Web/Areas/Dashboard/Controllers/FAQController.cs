using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data;
using System.Data.Entity;
using TFax.Web.CORE;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.DAL;
using TFax.Web.CORE.COMMON.Attributes;

namespace TFax.Web.Areas.Dashboard.Controllers
{
     [AppAuthorize]
     [HttpTraceFilter]
    public class FAQController : DbController<TextContent>
    {
        private readonly long FAQ_STATUS_ID = (long)ContentStatus.FAQ_CONTENT;

        // GET: /Dashboard/TextContent/
        public ActionResult Index(int page = 1)
        {
            var cultureId = RouteData.Values.GetCultureId();
            var textcontents = DbRepository.FindAll(f => f.StatusId == FAQ_STATUS_ID && f.LanguageId == cultureId).Include(t => t.Status_TextContents).Include(t => t.Language);

            return View(textcontents.OrderBy(o => o.Id).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }


        // GET: /Dashboard/TextContent/Details/5
        public ActionResult Details(long? id)
        {
            TextContent textcontent = DbRepository.Find(id);
            if (textcontent == null)
            {
                return HttpNotFound();
            }
            return View(textcontent);
        }

        // GET: /Dashboard/TextContent/Create
        public ActionResult Create()
        {
            var textContent = new TextContent();
            ViewBag.CreateDby = new SelectList(UnitOfWork.Db.Set<MemberMaster>(), "Id", "Email_Login");
            ViewBag.PageID = UIHelper.GetPageContentList();
            return View(textContent);
        }

        // POST: /Dashboard/TextContent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,PageID,TitleHTML,BodyHTML")] TextContent textcontent)
        {

            if (ModelState.IsValid)
            {
                textcontent.CreatedBy = MembershipHelper.Current.FullName;
                textcontent.CreatedIP = UtilHelper.GetVisitorIPAddress();
                textcontent.CreatedDate = DateTime.Now;
                textcontent.PageID = UIHelper.GetContentPageID(textcontent.PageID);
                textcontent.StatusId = FAQ_STATUS_ID;
                textcontent.LanguageId = RouteData.Values.GetCultureId();
                DbRepository.Insert(textcontent);

                return RedirectToAction("Index");
            }

            ViewBag.CreateDby = new SelectList(UnitOfWork.Db.Set<MemberMaster>(), "Id", "Email_Login", textcontent.CreatedBy);
            ViewBag.PageID = UIHelper.GetPageContentList(textcontent.PageID);
            return View(textcontent);
        }

        // GET: /Dashboard/TextContent/Edit/5
        public ActionResult Edit(long? id)
        {
            TextContent textcontent = DbRepository.Find(id);
            if (textcontent == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateDby = new SelectList(UnitOfWork.Db.Set<MemberMaster>(), "Id", "Email_Login", textcontent.CreatedBy);
            ViewBag.PageID = UIHelper.GetPageContentList(textcontent.PageID);

            return View(textcontent);
        }

        // POST: /Dashboard/TextContent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,PageID,TitleHTML,BodyHTML")] TextContent textcontent)
        {
            if (ModelState.IsValid)
            {
                textcontent.CreatedBy = MembershipHelper.Current.FullName;
                textcontent.CreatedIP = UtilHelper.GetVisitorIPAddress();
                textcontent.CreatedDate = DateTime.Now;
                textcontent.PageID = UIHelper.GetContentPageID(textcontent.PageID);
                textcontent.StatusId = FAQ_STATUS_ID;

                DbRepository.Update(textcontent);

                return RedirectToAction("Index");
            }
            ViewBag.CreateDby = new SelectList(UnitOfWork.Db.Set<MemberMaster>(), "Id", "Email_Login", textcontent.CreatedBy);
            ViewBag.PageID = UIHelper.GetPageContentList(textcontent.PageID);

            return View(textcontent);
        }

        // GET: /Dashboard/TextContent/Delete/5
        public ActionResult Delete(long? id)
        {
            TextContent textcontent = DbRepository.Find(id);
            if (textcontent == null)
            {
                return HttpNotFound();
            }
            return View(textcontent);
        }

        // POST: /Dashboard/TextContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TextContent textcontent = DbRepository.Find(id);

            DbRepository.Delete(textcontent);

            return RedirectToAction("Index");
        }

    }
}
