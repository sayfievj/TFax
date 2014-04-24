using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.ViewModels
{
    public class ProfileViewModel
    {
         
        public MemberMaster Member { get; set; }
         
        public bool IsTutor { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }

        public IList<HttpPostedFileBase> EduFiles { get; set; }

    }
}