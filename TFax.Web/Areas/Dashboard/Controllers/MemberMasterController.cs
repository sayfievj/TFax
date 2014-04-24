using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web.Helpers;
using System.Web.Mvc;

using PagedList;

using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.DAL;
using AutoMapper;
using TFax.Web.CORE.COMMON.ViewModels;

namespace TFax.Web.Areas.Dashboard.Controllers
{
    [HttpTraceFilter]
    [AppAuthorize]
    public class MemberMasterController : DbController<MemberMaster>
    {


        // GET: /Dashboard/MemberMaster/
        public ActionResult Index(int page = 1)
        {
            var membermasters = UnitOfWork.Db.Set<MemberMaster>()
                .Include(m => m.MemberRole)
                .Include(m => m.City)
                .Include(m => m.Country)
                .Include(m => m.State);

            return View(membermasters.OrderBy(o => o.Id).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }

        // GET: /Dashboard/MemberMaster/Details/5
        public ActionResult Details(long? id)
        {

            MemberMaster membermaster = DbRepository.Find(id);
            if (membermaster == null)
            {
                return HttpNotFound();
            }
            return View(membermaster);
        }

        // GET: /Dashboard/MemberMaster/Create
        public ActionResult Create()
        {
            var membermaster = new MemberMaster();

            ViewBag.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name");
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name");
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name");
            ViewBag.MemberRoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>(), "Id", "Name");

            var model = Mapper.Map<MemberViewModel>(membermaster);

            return View(model);
        }

        // POST: /Dashboard/MemberMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email_Login,Password,FirstName,LastName,Address,Street,CityId,StateId,CountryId,MemberClass,IsActivated,MemberRoleId")] MemberViewModel model)
        {
            var membermaster = Mapper.Map<MemberMaster>(model);

            if (ModelState.IsValid)
            {

                membermaster.RegisteredDate = DateTime.Now;
                membermaster.RegisteredIP = UtilHelper.GetVisitorIPAddress();
                membermaster.ActivationCode = Crypto.HashPassword(membermaster.Password);
                membermaster.ActivationCodeExpired = DateTime.Now.AddDays(AppSettingsHelper.GetValue<int>(AppSettings.KEYS.ACCOUNT_ACTIVATION_EXPIRED_DAYS));

                DbRepository.Insert(membermaster);

                EmailHelper.SendInfoEmail(membermaster);

                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name", membermaster.CityId);
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", membermaster.CountryId);
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", membermaster.StateId);
            ViewBag.MemberRoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>(), "Id", "Name", membermaster.MemberRoleId);

            return View(model);
        }

        // GET: /Dashboard/MemberMaster/Edit/5
        public ActionResult Edit(long? id)
        {
            MemberMaster membermaster = DbRepository.Find(id);
            if (membermaster == null)
            {
                return HttpNotFound();
            }

            ViewBag.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name", membermaster.CityId);
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", membermaster.CountryId);
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", membermaster.StateId);
            ViewBag.MemberRoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>(), "Id", "Name", membermaster.MemberRoleId);

            var model = Mapper.Map<MemberViewModel>(membermaster);

            return View(model);
        }

        // POST: /Dashboard/MemberMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email_Login,Password,FirstName,LastName,Address,Street,CityId,StateId,CountryId,MemberClass,IsActivated,MemberRoleId")] MemberViewModel model)
        {
            var membermaster = DbRepository.Find(model.Id);

            if (ModelState.IsValid)
            {
                Mapper.Map(model, membermaster);

                membermaster.RegisteredDate = DateTime.Now;
                membermaster.RegisteredIP = UtilHelper.GetVisitorIPAddress();
                membermaster.ActivationCode = Crypto.HashPassword(membermaster.Password);
                membermaster.ActivationCodeExpired = DateTime.Now.AddDays(AppSettingsHelper.GetValue<int>(AppSettings.KEYS.ACCOUNT_ACTIVATION_EXPIRED_DAYS));

                DbRepository.Update(membermaster);

                EmailHelper.SendInfoEmail(membermaster);

                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name", membermaster.CityId);
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", membermaster.CountryId);
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", membermaster.StateId);
            ViewBag.MemberRoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>(), "Id", "Name", membermaster.MemberRoleId);

            return View(model);
        }

        // GET: /Dashboard/MemberMaster/Delete/5
        public ActionResult Delete(long? id)
        {
            MemberMaster membermaster = DbRepository.Find(id);
            if (membermaster == null)
            {
                return HttpNotFound();
            }
            return View(membermaster);
        }

        // POST: /Dashboard/MemberMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MemberMaster membermaster = DbRepository.Find(id);


            DbRepository.Delete(membermaster);

            return RedirectToAction("Index");
        }

    }
}
