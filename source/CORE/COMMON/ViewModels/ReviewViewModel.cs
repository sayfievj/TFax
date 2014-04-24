using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFax.Web.CORE.DAL;

namespace TFax.Web.CORE.COMMON.ViewModels
{
    public class ReviewViewModel
    {

        public Review Review { get; set; }

        public MemberMaster Member { get; set; }

    }
}