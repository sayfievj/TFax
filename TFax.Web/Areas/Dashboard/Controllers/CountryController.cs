using System.Data;
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
    public class CountryController : DbController<Country>
    {
        // GET: /Dashboard/Country/
        public ActionResult Index(int page = 1)
        {
            var countries = UnitOfWork.Db.Set<Country>();

            return View(countries.OrderBy(o => o.Id).ToPagedList(page, AppSettings.SCAFFOLD.PAGE_SIZE));
        }

        // GET: /Dashboard/Country/Details/5
        public ActionResult Details(long? id)
        {
         
            Country country = DbRepository.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: /Dashboard/Country/Create
        public ActionResult Create()
        {
            var country = new Country();

            return View(country);
        }

        // POST: /Dashboard/Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CodeISO3,TimeZone")] Country country)
        {
            if (ModelState.IsValid)
            {
                DbRepository.Insert(country);

                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: /Dashboard/Country/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = DbRepository.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: /Dashboard/Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CodeISO3,TimeZone")] Country country)
        {
            if (ModelState.IsValid)
            {
                DbRepository.Update(country);

                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: /Dashboard/Country/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = DbRepository.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: /Dashboard/Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Country country = DbRepository.Find(id);
            DbRepository.Delete(country);

            return RedirectToAction("Index");
        }
    }
}
