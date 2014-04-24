using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.ViewModels
{
     
    public class MemberViewModel
    {
         
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email_Login { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public string Street { get; set; }

        [Display(Name = "City")]
        public Nullable<long> CityId { get; set; }

        [Display(Name = "State")]
        public Nullable<long> StateId { get; set; }

        [Display(Name = "Country")]
        public Nullable<long> CountryId { get; set; }

        [Display(Name = "Class")]
        public string MemberClass { get; set; }

        [Display(Name = "Is Active?")]
        public Nullable<bool> IsActivated { get; set; }

        [Display(Name = "Role")]
        public long? MemberRoleId { get; set; }

    }
}