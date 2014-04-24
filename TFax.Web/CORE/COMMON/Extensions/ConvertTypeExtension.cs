using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Helpers;

namespace TFax.Web
{
    public static class ConvertTypeExtension
    {

        public static T ConvertTo<T>(this object value, T defaultValue = default(T))
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static string ToBase64String(this byte[] data)
        {
            if (data == null)
            {
                var path = HttpContext.Current.Server.MapPath("~/Images/no_image.png");

                data = File.ReadAllBytes(Path.GetFullPath(path));
            }

            return String.Format("data:image/png;base64,{0}", Convert.ToBase64String(data));
        }

        public static string ToBase64ImageString(this byte[] data, int w, int h)
        {
            if (data == null)
            {
                var path = HttpContext.Current.Server.MapPath("~/Images/no_image.png");

                data = File.ReadAllBytes(Path.GetFullPath(path));
            }

            var image = new WebImage(data).Resize(w, h);
            data = image.GetBytes("image/png");

            return String.Format("data:image/png;base64,{0}", Convert.ToBase64String(data));
        }
    }
}