using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using DotNetOpenAuth;

using PagedList;

using TFax.Web.CORE;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Areas.Dashboard.Controllers
{
    [HttpTraceFilter]
    [AppAuthorize]
    public class CityController : DbController<City>
    {
        // GET: /Dashboard/City/
        public ActionResult Index(int id = 0, int page = 1)
        {
            var cities = UnitOfWork.Db.Set<City>().Include(c => c.Country).Include(c => c.State);

            if (id > 0)
                cities = cities.Where(w => w.StateId == id);

            return View(cities.OrderBy(o => o.Id).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }

        // GET: /Dashboard/City/Details/5
        public ActionResult Details(long? id)
        {
            
            City city = DbRepository.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: /Dashboard/City/Create
        public ActionResult Create()
        {
            var city = new City();
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name");
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name");
            return View(city);
        }

        // POST: /Dashboard/City/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StateId,CountryId")] City city)
        {
            if (ModelState.IsValid)
            {
                DbRepository.Insert(city);

                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", city.CountryId);
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", city.StateId);
            return View(city);
        }

        // GET: /Dashboard/City/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = DbRepository.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", city.CountryId);
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", city.StateId);
            return View(city);
        }

        // POST: /Dashboard/City/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StateId,CountryId")] City city)
        {
            if (ModelState.IsValid)
            {
                DbRepository.Insert(city);

                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", city.CountryId);
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", city.StateId);
            return View(city);
        }

        // GET: /Dashboard/City/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = DbRepository.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: /Dashboard/City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            City city = DbRepository.Find(id);

            DbRepository.Delete(city);

            return RedirectToAction("Index");
        }

    }
}
