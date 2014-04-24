using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Antlr.Runtime.Tree;

using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.COMMON.ViewModels;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.Helpers
{
    using System.Data.Entity;

    public static class UIHelper
    {

        private static IUnitOfWork UnitOfWork
        {
            get { return DependencyResolver.Current.GetService<IUnitOfWork>(); }
        }

        public static string GetTageId(this HtmlHelper helper, string tageName)
        {
            return HtmlHelper.GenerateIdFromName(tageName);
        }

        public static MvcHtmlString LoadCLEditor(this HtmlHelper helper, string elements)
        {
            return helper.Partial("EditorTemplates/CLEditor", elements);
        }

        public static MvcHtmlString GetPageContent(this HtmlHelper helper, ContentPage contentPage, long? id = null)
        {
            var pageId = GetContentPageID(contentPage);
            var reposiroty = new BaseRepository<TextContent>(UnitOfWork);
            var context = helper.ViewContext.RequestContext;
            var cultureId = context.RouteData.Values.GetCultureId();
            var model = id == null
                ? reposiroty.Find(f => f.PageID == pageId && f.LanguageId == cultureId)
                : reposiroty.Find(f => f.PageID == pageId && f.Id == id && f.LanguageId == cultureId);

            return model == null
                ? MvcHtmlString.Empty
                : helper.Partial("EditorTemplates/TextContent", model);
        }

        public static MvcHtmlString RenderCountry(this HtmlHelper helper, int id = 0)
        {
            var model = CacheHelper.GetStore<IQueryable<Country>>(AppSettings.STORE.COUNRY_STORE_KEY);

            if (model == null)
            {
                model = (id > 0)
                    ? UnitOfWork.Db.Set<Country>().Where(w => w.Id == id)
                    : UnitOfWork.Db.Set<Country>();

                CacheHelper.SetStore(model, AppSettings.STORE.COUNRY_STORE_KEY);
            }

            return helper.Partial("Partials/_Country", model);
        }

        public static IHtmlString GetActiveMembers(this HtmlHelper helper)
        {
            var activeMembers = UnitOfWork.Db.Set<MemberMaster>()
                .Where(w => w.IsActivated == true)
                .ToList();

            return helper.Raw(String.Format("Active Members ({0})", activeMembers.Count));
        }

        public static IHtmlString GetReviews(this HtmlHelper helper)
        {
            var reviews = UnitOfWork.Db.Set<Review>()
                .Where(w => w.Status_ReviewId == (long?)ReviewStatus.Active)
                .ToList();

            return helper.Raw(String.Format("Reviews ({0})", reviews.Count));
        }

        public static MvcHtmlString GetId(this HtmlHelper helper)
        {
            var model = (dynamic)helper.ViewData.Model;

            return model.Id > 0
                ? helper.Hidden("Id", (object)model.Id)
                : MvcHtmlString.Empty;
        }

        public static SelectList GetPageContentList(object dataValue = null)
        {
            var items = (long[])Enum.GetValues(typeof(ContentPage)).Clone();
            var dic = items.ToDictionary(key => GetContentPageID(key), value => value);

            return new SelectList(dic, "Value", "Key", dataValue);
        }

        public static IList<SelectListItem> GetListItems(this HtmlHelper helper, int itemsCount = 10)
        {
            var list = new List<SelectListItem>();

            for (int i = 0; i < itemsCount; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = (i + 1).ToString(CultureInfo.InvariantCulture),
                    Value = (i + 1).ToString(CultureInfo.InvariantCulture)
                });
            }
             
            return list;
        }

        public static string GetContentPageID(string id)
        {
            long value;

            return Int64.TryParse(id, out value)
                ? Enum.GetName(typeof(ContentPage), value)
                : String.Empty;
        }

        public static string GetContentPageID(long id)
        {
            return Enum.GetName(typeof(ContentPage), id);
        }

        public static string GetContentPageID(ContentPage contentPage)
        {
            return GetContentPageID((long)contentPage);
        }
    }
}