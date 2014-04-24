using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFax.Web
{
    using System.Web.Helpers;

    public class SmtpConfig
    {
        public static void Configure()
        {
            WebMail.EnableSsl = true;
            WebMail.From = "elyor@outlook.com";
            WebMail.SmtpServer = "smtp.live.com";
            WebMail.UserName = "elyor@outlook.com";
            WebMail.Password = "asp.netmvc";
            WebMail.SmtpPort = 587;
        }
    }
}