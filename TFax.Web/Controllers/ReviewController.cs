using System;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using TFax.Web.CORE;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Controllers
{
    [HttpTraceFilter]
    [AppAuthorize]
    public class ReviewController : DbController<Review>
    {

        private void LoadViewBags()
        {

            //address
            ViewBag.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name");
            ViewBag.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name");
            ViewBag.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name");

        }

        [AllowAnonymous]
        public ActionResult Index(int page = 1, int size = AppSettings.SCAFFOLD.PAGE_SIZE)
        {
            var reviews = DbRepository.All
                .Include(i => i.Status_Reviews)
                .Include(i => i.MemberMaster)
                .Include(i => i.Profile_Master)
                .OrderByDescending(review => review.Id)
                .ToPagedList(page, size);
            var model = new ReviewSearchViewModel
            {
                Objects = reviews
            };

            LoadViewBags();

            return View(model);
        }

        public ActionResult Account(int page = 1, int size = AppSettings.SCAFFOLD.PAGE_SIZE)
        {
            var profile = MembershipHelper.Current.Profile;
            var reviews = profile != null ? profile.Reviews : new Review[] { };
            var model = new ReviewSearchViewModel
            {
                Objects = reviews.ToPagedList(page, size)
            };

            LoadViewBags();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Seek(int page = 1, int size = AppSettings.SCAFFOLD.PAGE_SIZE)
        {
            var reviews = DbRepository.All
                .Include(i => i.Status_Reviews)
                .Include(i => i.MemberMaster)
                .Include(i => i.Profile_Master)
                .Where(f => f.Status_ReviewId == (long?)ReviewStatus.Pending)
                .OrderByDescending(review => review.Id)
                .ToPagedList(page, size);
            var model = new ReviewSearchViewModel
            {
                Objects = reviews.ToPagedList(page, size)
            };

            LoadViewBags();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult New(int page = 1, int size = AppSettings.SCAFFOLD.PAGE_SIZE)
        {
            var reviews = DbRepository.All
                .Include(i => i.Status_Reviews)
                .Include(i => i.MemberMaster)
                .Include(i => i.Profile_Master)
                .Where(f => f.Status_ReviewId == (long?)ReviewStatus.Pending)
                .OrderByDescending(review => review.Id)
                .ToPagedList(page, size);
            var model = new ReviewSearchViewModel
            {
                Objects = reviews.ToPagedList(page, size)
            };

            LoadViewBags();

            return View(model);
        }

        public ActionResult Submit()
        {
            var model = new ReviewViewModel
            {
                Member = MembershipHelper.Current.GetMember(),
                Review = new Review()
            };
            return View(model);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Submit(ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Review.ProfileId = MembershipHelper.Current.ProfileId;
                model.Review.CreatedDate = DateTime.Now;
                model.Review.CreatedIP = UtilHelper.GetVisitorIPAddress();
                model.Review.Status_ReviewId = (int)ReviewStatus.Pending;

                DbRepository.Insert(model.Review);

                TempData["ReviewStatus"] = String.Format("Your message has been saved.Please wait to if approving an {0} administration", AppSettings.APP_TITLE);

                return RedirectToAction("Submit");
            }

            ModelState.AddModelError("", "An error has occurred.");

            return View(model);
        }

        public ActionResult Edit(long? id)
        {
            var review = DbRepository.Find(id);

            if (review == null)
                return new HttpNotFoundResult();

            var model = new ReviewViewModel
            {
                Member = MembershipHelper.Current.GetMember(),
                Review = review
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(long? id, ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Review.ModifiedDate = DateTime.Now;
                model.Review.ModifiedIP = UtilHelper.GetVisitorIPAddress();

                DbRepository.Update(model.Review);

                TempData["ReviewStatus"] = String.Format("Your review modifying successfully.");

                return RedirectToAction("Edit");
            }

            ModelState.AddModelError("", "An error has occurred.");

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Details(long? id)
        {
            var review = DbRepository.Find(id);

            if (review == null)
                return new HttpNotFoundResult();

            return View(review);
        }

        [HttpPost]
        public ActionResult Accept(long? id)
        {
            var review = DbRepository.Find(id);

            if (review == null)
                return new HttpNotFoundResult();

            if (review.Status_ReviewId != (long?)ReviewStatus.Pending)
            {
                review.Status_ReviewId = (long?)ReviewStatus.Active;
                review.ReviewDate = DateTime.Now;
                review.ReviewerId = MembershipHelper.Current.Id;

                DbRepository.Update(review);
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public ActionResult Reject(long? id)
        {
            var review = DbRepository.Find(id);

            if (review == null)
                return new HttpNotFoundResult();

            if (review.Status_ReviewId != (long?)ReviewStatus.Trash)
            {
                review.Status_ReviewId = (long?)ReviewStatus.Trash;
                review.ReviewDate = DateTime.Now;
                review.ReviewerId = MembershipHelper.Current.Id;

                DbRepository.Update(review);
            }

            return RedirectToAction("Details", new { id });
        }

        public ActionResult Contact()
        {
            return View();
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult Top()
        {
            var models = DbRepository.FindAll(f => f.Status_ReviewId == (long?)ReviewStatus.Active)
                .OrderByDescending(o => o.Id)
                .Take(AppSettings.SCAFFOLD.PAGE_SIZE)
                .ToList();

            return PartialView("Partials/_TopReviews", models);
        }

        public ActionResult Delete(long? id)
        {
            var review = DbRepository.Find(id);

            if (review == null)
                return new HttpNotFoundResult();

            DbRepository.Delete(review);

            TempData["DeleteStatus"] = "Review has ben deleted.";

            TempData["DeleteStatus"] = String.Format("'{0}' review have been deleted.", review.ReviewTitle);

            return RedirectToAction("Account");
        }
    }
}