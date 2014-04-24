using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.Helpers
{

    public class CultureHelper
    {
        private static IUnitOfWork UnitOfWork
        {
            get { return DependencyResolver.Current.GetService<IUnitOfWork>(); }
        } 

        public static Language GetCulture(string code)
        { 
            return UnitOfWork.Db.Set<Language>().FirstOrDefault(f => f.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        public static Language GetCulture(long? id)
        {
            return UnitOfWork.Db.Set<Language>().Find(id ?? (long?)Culture.English);
        }

        public static string DefaultCulture
        {
            get { return "en-US"; }
        }
         
    }
}