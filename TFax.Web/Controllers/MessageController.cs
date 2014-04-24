using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using TFax.Web.CORE;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Controllers
{
    [HttpTraceFilter]
    [AppAuthorize]
    public class MessageController : DbController<MyMessage>
    {
        //
        // GET: /Message/
        public ActionResult Index(long? status = null, int page = 1, int size = AppSettings.SCAFFOLD.PAGE_SIZE)
        {
            var messages = MembershipHelper.Current.CheckRole(Role.Admin)
                ? DbRepository.All
                : DbRepository.FindAll(message => message.SenderId == MembershipHelper.Current.Id);

            if (status != null)
            {
                switch ((MessageStatus)status)
                {
                    case MessageStatus.Read:
                        messages = messages.Where(w => w.StatusReadId == status);
                        break;
                    case MessageStatus.Delete:
                        messages = messages.Where(w => w.StatusDeleteId == status);
                        break;
                    case MessageStatus.Label:
                        messages = messages.Where(w => w.StatusLabelId == status);
                        break;
                }
            }

            ViewBag.Status = status;
            ViewBag.Statuses = new SelectList(UnitOfWork.Db.Set<Status_Messages>(), "Id", "Name", status);

            return View(messages.OrderByDescending(o => o.Id).ToPagedList(page, size));
        }

        public ActionResult Review(long? id)
        {
            var message = new MyMessage
            {
                SenderId = MembershipHelper.Current.Id
            };

            ViewBag.CcId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login");
            ViewBag.ToId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login");

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Review(long? id, MyMessage message)
        {
            if (ModelState.IsValid)
            {
                message.DateSent = DateTime.Now;

                DbRepository.Insert(message);

                return RedirectToAction("Details", "Review", new { id });
            }

            ViewBag.CcId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login", message.CcId);
            ViewBag.ToId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login", message.ToId);


            return View(message);
        }

        public ActionResult Send(long? id)
        {
            var message = new MyMessage
            {
                SenderId = MembershipHelper.Current.Id
            };

            ViewBag.CcId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login");
            ViewBag.ToId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login");

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Send(long? id, MyMessage message)
        {
            if (ModelState.IsValid)
            {
                message.DateSent = DateTime.Now;

                DbRepository.Insert(message);

                TempData["SendMessageStatus"] = "Your message has been sent.";

                return RedirectToAction("Send");
            }

            ViewBag.CcId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login", message.CcId);
            ViewBag.ToId = new SelectList(UnitOfWork.Db.Set<MemberMaster>().Where(w => w.Id != message.SenderId), "Id", "Email_Login", message.ToId);


            return View(message);
        }


        public ActionResult Delete(long? id)
        {
            var message = DbRepository.Find(id);

            if (message == null)
                return new HttpNotFoundResult();

            DbRepository.Delete(message);

            TempData["DeleteStatus"] = String.Format("'{0}' message have been deleted.", message.Subject);

            return RedirectToAction("Index");
        }

        public ActionResult Details(long? id)
        {
            var message = DbRepository.Find(id);

            if (message == null)
                return new HttpNotFoundResult();

            return View(message);
        }
    }
}