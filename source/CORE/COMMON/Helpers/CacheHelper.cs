using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.Helpers
{ 
    public class CacheHelper
    {  
        public static T GetStore<T>(string key)
        {
            return HttpContext.Current.Cache[key] != null
                ? (T)HttpContext.Current.Cache[key]
                : default(T);
        }

        public static void SetStore<T>(T t, string key)
        {
            HttpContext.Current.Cache[key] = t; 
        }

        public static void Remove(string key)
        { 
            HttpContext.Current.Cache.Remove(key);
        }
         
    }
}