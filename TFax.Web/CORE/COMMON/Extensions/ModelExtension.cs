using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using TFax.Web.CORE.BLL.Infrastructure;

namespace TFax.Web
{
    public static class ModelExtension
    {

        public static object GetPropValue(this object sourceData, string propName)
        {
            return sourceData.GetType().GetProperty(propName).GetValue(sourceData, null);
        }

        public static T GetPropValue<T>(this object sourceData, string propName)
        {
            var result = sourceData.GetPropValue(propName);

            return result != null ? (T)result : default(T);
        }

        public static void UpdateCollectionFromModel<T>(this ICollection<T> domainCollection,
                                                IQueryable<T> objects, long[] newValues)
                                                where T : class
        {
            if (newValues == null)
            {
                domainCollection.Clear();
                return;
            }

            for (var i = domainCollection.Count - 1; i >= 0; i--)
            {
                var domainObject = domainCollection.ElementAt(i);
                if (!newValues.Contains(domainObject.GetPropValue<long>("Id")))
                    domainCollection.Remove(domainObject);
            }

            foreach (var newId in newValues)
            {
                var domainObject = domainCollection.FirstOrDefault(t => t.GetPropValue<long>("Id") == newId);
                if (domainObject != null)
                    continue;

                domainObject = objects.FirstOrDefault(t => t.GetPropValue<long>("Id") == newId);
                if (domainObject == null)
                {
                    continue;
                }

                domainCollection.Add(domainObject);
            }
        }

    }
}