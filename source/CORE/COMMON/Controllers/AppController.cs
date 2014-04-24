using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.Controllers
{
    [HandleError]
    public class AppController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        protected AppController()
        {
            _unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected IBaseRepository<T> GetRepository<T>() where T : class
        {
            return new BaseRepository<T>(_unitOfWork);
        }

        #region Language

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult LanguageBar()
        {
            var languages = UnitOfWork.Db.Set<Language>();
            var cultureCode = RouteData.Values.GetCultureCode();

            ViewBag.Culture = CultureHelper.GetCulture(cultureCode);

            return PartialView("Partials/_Language", languages);
        }

        [AllowAnonymous]
        public ActionResult ChangeLanguage(long? id, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToHome();

            var cultureCode = RouteData.Values.GetCultureCode();
            var currentCulture = CultureHelper.GetCulture(cultureCode);
            var newCulture = CultureHelper.GetCulture(id);
            var newReturnUrl = returnUrl.StartsWith(String.Concat("/", currentCulture.Code))
                ? returnUrl.Replace(String.Concat("/", currentCulture.Code), String.Concat("/", newCulture.Code))
                : String.Concat("/", currentCulture.Code, returnUrl);

            return RedirectToLocal(newReturnUrl);
        }
        #endregion

        #region Account

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Account()
        {
            if (MembershipHelper.Current.IsAuthenticated)
                return new EmptyResult();

            var model = new RegisterViewModel();
            {
                ViewBag.RoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>().Where(w => w.Id != (long) Role.Admin),"Id", "Name");
            }

            return PartialView("Partials/_Account", model);
        }

        #endregion

        [AllowAnonymous]
        protected ActionResult RedirectToLocal(string returnUrl = "/")
        {
            return Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToHome();
        }

        [AllowAnonymous]
        protected ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}