using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using PagedList;

using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Areas.Dashboard.Controllers
{
    [HttpTraceFilter]
    [AppAuthorize]
    public class StateController : DbController<State>
    {
        // GET: /Dashboard/State/
        public ActionResult Index(int id = 0, int page = 1)
        {
            var states = UnitOfWork.Db.Set<State>().Include(s => s.Country);

            if (id > 0)
                states = states.Where(w => w.CountryId == id);

            return View(states.OrderBy(o => o.Id).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }

        // GET: /Dashboard/State/Details/5
        public ActionResult Details(long? id)
        { 
            State state = DbRepository.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // GET: /Dashboard/State/Create
        public ActionResult Create()
        {
            var state = new State();
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name");
            return View(state);
        }

        // POST: /Dashboard/State/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ZipCode,CountryId")] State state)
        {
            if (ModelState.IsValid)
            {
                DbRepository.Insert(state);

                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", state.CountryId);
            return View(state);
        }

        // GET: /Dashboard/State/Edit/5
        public ActionResult Edit(long? id)
        { 
            State state = DbRepository.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", state.CountryId);
            return View(state);
        }

        // POST: /Dashboard/State/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ZipCode,CountryId")] State state)
        {
            if (ModelState.IsValid)
            {
                DbRepository.Update(state);

                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", state.CountryId);
            return View(state);
        }

        // GET: /Dashboard/State/Delete/5
        public ActionResult Delete(long? id)
        { 
            State state = DbRepository.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // POST: /Dashboard/State/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            State state = DbRepository.Find(id);
            DbRepository.Delete(state);

            return RedirectToAction("Index");
        }

    }
}
