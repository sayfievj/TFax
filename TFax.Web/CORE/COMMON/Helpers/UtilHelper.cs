using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using TFax.Web.CORE.COMMON.Enums;

namespace TFax.Web.CORE.COMMON.Helpers
{
    public class UtilHelper
    {
        /// <summary>
        /// method to get Client ip address
        /// </summary>
        /// <returns></returns>
        public static string GetVisitorIPAddress()
        {
            var isLAN = HttpContext.Current.Request.IsLocal;
            var visitorIPAddress = string.Empty;

            if (!isLAN)
            {
                visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (String.IsNullOrEmpty(visitorIPAddress))
                    visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(visitorIPAddress))
                    visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

                if (!string.IsNullOrEmpty(visitorIPAddress) && visitorIPAddress.Trim() != "::1")
                {
                    return visitorIPAddress;
                }
            }
            else
            {

                //This is for Local(LAN) Connected ID Address
                var stringHostName = Dns.GetHostName();

                //Get Ip Host Entry
                var ipHostEntries = Dns.GetHostEntry(stringHostName);

                //Get Ip Address From The Ip Host Entry Address List
                var arrIpAddress = ipHostEntries.AddressList;

                try
                {
                    visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                }
                catch
                {
                    try
                    {
                        visitorIPAddress = arrIpAddress[0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            arrIpAddress = Dns.GetHostAddresses(stringHostName);
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            visitorIPAddress = "127.0.0.1";
                        }
                    }
                }
            }

            return visitorIPAddress;
        }
         

    }
}