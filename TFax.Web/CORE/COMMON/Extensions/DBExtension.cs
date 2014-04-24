using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFax.Web.CORE.COMMON.Enums;
using TFax.Web.CORE.DAL;

namespace TFax.Web
{
    public static class DBExtension
    {

        public static string GetFullName(this MemberMaster master)
        {
            return String.IsNullOrWhiteSpace(master.FirstName) || string.IsNullOrWhiteSpace(master.LastName)
                ? master.Email_Login
                : String.Concat(master.FirstName, " ", master.LastName);
        }

        public static string GetLocation(this MemberMaster master)
        {
            return String.Format("{0}({1}){4}{2},{3}", master.CountryId != null ? master.Country.Name : String.Empty, master.CountryId != null ? master.Country.CodeISO3 : String.Empty, master.CityId != null ? master.City.Name : String.Empty, master.StateId != null ? master.State.Name : String.Empty,Environment.NewLine);
        }

        public static string GetColor(this MemberMaster master)
        {
            return master.MemberRoleId == (long?)Role.Tutor ? "red" : "green";
        }

    }
}