using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

using Microsoft.Practices.Unity.Utility;
using PagedList;
using TFax.Web.CORE;
using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Attributes;
using TFax.Web.CORE.COMMON.Controllers;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.Controllers
{
    [AppAuthorize]
    [HttpTraceFilter]
    public class AccountController : DbController<MemberMaster>
    {
        private void LoadViewBags()
        {

            //type
            ViewBag.SearchTutorId = new SelectList(Enum.GetValues(typeof(SearchTutorType))
                        .Cast<long>()
                        .Select(s => new KeyValuePair<string, long?>(Enum.GetName(typeof(SearchTutorType), s), s)), "Value", "Key");
        }

        #region JSON

        [HttpPost]
        public ActionResult DeleteEduBackground(long? id)
        {
            var eduRepository = GetRepository<Profile_EduBackground>();
            {
                var edu = eduRepository.Find(id);

                if (edu == null)
                    return Json(null, JsonRequestBehavior.AllowGet);

                eduRepository.Delete(edu);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteLink(long? id)
        {
            var eduRepository = GetRepository<Profile_Links>();
            {
                var edu = eduRepository.Find(id);

                if (edu == null)
                    return Json(null, JsonRequestBehavior.AllowGet);

                eduRepository.Delete(edu);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteSubject(long? id)
        {
            var eduRepository = GetRepository<Profile_Subjects>();
            {
                var edu = eduRepository.Find(id);

                if (edu == null)
                    return Json(null, JsonRequestBehavior.AllowGet);

                eduRepository.Delete(edu);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        public ActionResult Index()
        {

            #region Init model fields
            //message
            var messages = MembershipHelper.Current.CheckRole(Role.Admin)
              ? UnitOfWork.Db.Set<MyMessage>()
              : UnitOfWork.Db.Set<MyMessage>()
                  .Where(message => message.SenderId == MembershipHelper.Current.Id);

            messages = messages.Where(message => message.StatusReadId == null)
                .OrderByDescending(message => message.Id)
                .Take(AppSettings.SCAFFOLD.PAGE_SIZE);

            //favorites
            var favorites = UnitOfWork.Db.Set<MyFavorite>()
               .Where(w => w.ProfileID == MembershipHelper.Current.ProfileId)
               .OrderByDescending(favorite => favorite.Id)
               .Take(AppSettings.SCAFFOLD.PAGE_SIZE);

            //reviews
            var reviews = MembershipHelper.Current.CheckRole(Role.Admin)
             ? UnitOfWork.Db.Set<Review>()
             : UnitOfWork.Db.Set<Review>()
                 .Where(message => message.ProfileId == MembershipHelper.Current.ProfileId);

            reviews = reviews.Where(message => message.Status_ReviewId == (long?)ReviewStatus.Active)
                .OrderByDescending(message => message.Id)
                .Take(AppSettings.SCAFFOLD.PAGE_SIZE);

            #endregion

            var model = new AccountViewModel
            {
                Messages = messages,
                Favorites = favorites,
                Reviews = reviews
            };

            LoadViewBags();

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel();

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipHelper.Current.IsValid(model.UserName, model.Password))
                {
                    if (MembershipHelper.Current.CheckActive(model.UserName))
                    {

                        var result = MembershipHelper.Current.SignIn(model);

                        if (!result)
                            ModelState.AddModelError("", "An error has occurred.");
                        else
                        {
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your account not activated.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();

            ViewBag.RoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>().Where(w => w.Id != (long)Role.Admin), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var member = MembershipHelper.Current.SignUp(model);

                if (member == null)
                    ModelState.AddModelError("", "An error has occurred.");
                else
                {
                    TempData["SignUpStatus"] = "Your account creation successfully.Please check your verification e-mail.";

                    EmailHelper.SendVerifyEmail(member);

                    return RedirectToAction("Register");
                }
            }

            ViewBag.RoleId = new SelectList(UnitOfWork.Db.Set<MemberRole>().Where(w => w.Id != (long)Role.Admin), "Id", "Name", model.RoleId);

            return View(model);
        }

        public new ActionResult Profile()
        {
            var member = MembershipHelper.Current.GetMember();
            {
                dynamic address = new ExpandoObject();
                {
                    address.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name", member.CityId);
                    address.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", member.CountryId);
                    address.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", member.StateId);
                }
                ViewBag.Member = address;
            }
            var model = new ProfileViewModel
            {
                IsTutor = (member.MemberRoleId == (long)Role.Tutor),
                Member = member
            };

            if (!model.Member.Profile_Master.Any())
                model.Member.Profile_Master.Add(new Profile_Master());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(ProfileViewModel model)
        {
            var member = DbRepository.Find(model.Member.Id);
            var isValid = ModelState.IsValid;

            if (isValid)
            {

                #region Init

                member.RegisteredDate = DateTime.Now;
                member.RegisteredIP = UtilHelper.GetVisitorIPAddress();
                member.FirstName = model.Member.FirstName;
                member.LastName = model.Member.LastName;
                member.Address = model.Member.Address;
                member.Street = model.Member.Street;
                member.CityId = model.Member.CityId;
                member.StateId = model.Member.StateId;
                member.CountryId = model.Member.CountryId;
                member.MemberClass = model.Member.MemberClass;

                #endregion

                var profile = member.Profile_Master.ElementAt(0);

                #region Edu

                for (var i = 0; i < model.Member.Profile_EduBackground.Count; i++)
                {
                    var modelEdu = model.Member.Profile_EduBackground.ElementAt(i);

                    var entityEdu = new Profile_EduBackground
                    {
                        CreatedDate = DateTime.Now,
                        CreatedIP = UtilHelper.GetVisitorIPAddress(),
                        College_University = modelEdu.College_University,
                        Degree = modelEdu.Degree,
                        GraduateYear = modelEdu.GraduateYear,
                        Major = modelEdu.Major,
                        Proof_Title = modelEdu.Proof_Title
                    };

                    if (model.EduFiles != null && model.EduFiles[i] != null)
                        entityEdu.Proof_File = new BinaryReader(model.EduFiles[i].InputStream).ReadBytes((int)model.EduFiles[i].InputStream.Length);

                    member.Profile_EduBackground.Add(entityEdu);
                }

                #endregion

                #region Master

                for (var i = 0; i < model.Member.Profile_Master.Count; i++)
                {

                    var modelProf = model.Member.Profile_Master.ElementAt(i);
                    var entityProf = (modelProf.Id == 0)
                        ? new Profile_Master()
                        : member.Profile_Master.FirstOrDefault(f => f.Id == modelProf.Id);

                    if (entityProf != null)
                    {
                        //links
                        foreach (Profile_Links modLink in modelProf.Profile_Links)
                        {
                            var entLink = new Profile_Links
                            {
                                CreatedDate = DateTime.Now,
                                CreatedIP = UtilHelper.GetVisitorIPAddress(),
                                LinkTitle = modLink.LinkTitle,
                                LinkURL = modLink.LinkURL
                            };
                            entityProf.Profile_Links.Add(entLink);
                        }

                        //subject
                        foreach (Profile_Subjects w in modelProf.Profile_Subjects)
                        {
                            var entSbj = new Profile_Subjects
                            {
                                CreatedDate = DateTime.Now,
                                CreatedIP = UtilHelper.GetVisitorIPAddress(),
                                Representative = w.Representative,
                                Subject = w.Subject
                            };
                            entityProf.Profile_Subjects.Add(entSbj);
                        }

                        if (entityProf.Id == 0)
                        {
                            entityProf.CreatedDate = DateTime.Now;
                            entityProf.CreatedIP = UtilHelper.GetVisitorIPAddress();
                            member.Profile_Master.Add(entityProf);
                        }
                    }
                }

                #endregion

                if (model.CoverFile != null)
                    member.Cover_File = new BinaryReader(model.CoverFile.InputStream).ReadBytes((int)model.CoverFile.InputStream.Length);

                if (!String.IsNullOrWhiteSpace(model.ChangePassword.OldPassword) &&
                    !String.IsNullOrWhiteSpace(model.ChangePassword.NewPassword))
                {

                    if (member.Password == model.ChangePassword.OldPassword)
                    {
                        member.Password = model.ChangePassword.NewPassword;
                        isValid = true;
                    }
                    else
                    {
                        ModelState.AddModelError("", "The old password and current password do not match.");
                        isValid = false;
                    }
                }

                if (isValid)
                {

                    DbRepository.Update(member);

                    TempData["ProfileStatus"] = "Your profile has been changed.";

                    EmailHelper.SendProfileEmail(member);

                    return RedirectToAction("Profile");
                }
            }

            dynamic address = new ExpandoObject();
            {
                address.CityId = new SelectList(UnitOfWork.Db.Set<City>(), "Id", "Name", member.CityId);
                address.CountryId = new SelectList(UnitOfWork.Db.Set<Country>(), "Id", "Name", member.CountryId);
                address.StateId = new SelectList(UnitOfWork.Db.Set<State>(), "Id", "Name", member.StateId);
            }
            ViewBag.Member = address;

            ModelState.AddModelError("", "An error has occurred.");

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            MembershipHelper.Current.SignOut();

            return RedirectToHome();
        }

        [AllowAnonymous]
        public ActionResult Active(long? id, string code)
        {
            var member = DbRepository.Find(id);

            if (member == null)
                return new HttpNotFoundResult();

            if (member.IsActivated == null &&
                Crypto.VerifyHashedPassword(code, member.Password))
            {
                member.IsActivated = true;

                DbRepository.Update(member);
                TempData["ActiveStatus"] = true;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(long? id)
        {
            var member = DbRepository.Find(id);

            if (member == null)
                return new HttpNotFoundResult();

            return View(member);
        }
    }
}