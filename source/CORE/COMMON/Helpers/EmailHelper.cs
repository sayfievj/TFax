using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.Helpers
{
    using System.Net.Mail;
    using System.Web.Helpers;
    using System.Text;

    public class EmailHelper
    {

        public const string TO_EMAIL = "elyor@outlook.com";

        public static bool SendEmail(string to, string subject, string body)
        {
            try
            {
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.Send(to, String.Format("{0} - {1}", AppSettings.APP_TITLE, subject), body);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SendExceptionEmail(Exception exp)
        {
            if (!AppSettingsHelper.GetValue<bool>(AppSettings.KEYS.SEND_EXCEPTION_EMAIL_ENABLED))
                return;

            var subject = String.Format("{0} - Exception", AppSettings.APP_TITLE);
            var body = exp.ToString();

            SendEmail(TO_EMAIL, subject, body);
        }

        public static bool SendProfileEmail(MemberMaster master)
        {
            var baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var profileUrl = String.Concat(baseUrl, urlHelper.Action("Profile", "Account"));
            var subject = String.Format(@"{0} - Forgot Password", AppSettings.APP_TITLE);
            var body = string.Format(@"<p>Dear {0},</p><p>Your username is: {1}.</a><p>You may change your password at the following address:{2}</p><p>Thanks,{3}.</p>", master.GetFullName(), master.Email_Login, profileUrl, AppSettings.APP_TITLE);

            return SendEmail(master.Email_Login, subject, body);
        }

        public static bool SendVerifyEmail(MemberMaster master)
        {
            var baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var verifyUrl = String.Concat(baseUrl, urlHelper.Action("Active", "Account", new { area = "", id = master.Id, code = master.ActivationCode }));
            var subject = String.Format("{0} - Verify Your Email", AppSettings.APP_TITLE);
            var body = String.Format(@"<p>Dear {0},</p><p>Click here to verify your email address and active your account today</p><p><a href='{1}'>Verify your email & active your account</a></p><p>You may active your member status at the following address:{1}</p><p>Thanks,{2}.</p>", master.GetFullName(), verifyUrl, AppSettings.APP_TITLE);

            return SendEmail(master.Email_Login, subject, body);
        }


        public static bool SendInfoEmail(MemberMaster master)
        {
            var sb = new StringBuilder();
            var baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var verifyUrl = String.Concat(baseUrl, urlHelper.Action("Active", "Account", new { area = "", id = master.Id, code = master.ActivationCode }));
            var detailsUrl = String.Concat(baseUrl, urlHelper.Action("Details", "Account", new { area = "", id = master.Id }));

            sb.Append("<table border='0'>");
            sb.AppendFormat("<tr><th>Email:</th><td>{0}</td></tr>", master.Email_Login);
            sb.AppendFormat("<tr><th>Password:</th><td>{0}</td></tr>", master.Email_Login);
            sb.AppendFormat("<tr><th>Full Name:</th><td>{0} {1}</td></tr>", master.FirstName, master.LastName);
            sb.AppendFormat("<tr><th>Address:</th><td>{0},{1},{2},{3}</td></tr>", master.Country.Name, master.State.Name, master.City.Name, master.Address);
            sb.AppendFormat("<tr><th>Date:</th><td>{0}</td></tr>", master.RegisteredDate.GetValueOrDefault().ToLongDateString());
            sb.AppendFormat("<tr><th>Is Active:</th><td>{0}</td></tr>", master.IsActivated == true ? "YES" : "NO");
            sb.Append("</table>");
            sb.AppendFormat("<p><If your account is not activated click here -{0} </p> or read more in your details page - {1}", verifyUrl, detailsUrl);

            var subject = String.Format("{0} - Member Info", AppSettings.APP_TITLE);
            var body = sb.ToString();

            return SendEmail(master.Email_Login, subject, body);
        }

    }
}