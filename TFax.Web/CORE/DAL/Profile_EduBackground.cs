//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TFax.Web.CORE.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profile_EduBackground
    {
        public long Id { get; set; }
        public Nullable<long> MemberId { get; set; }
        public string College_University { get; set; }
        public string GraduateYear { get; set; }
        public string Major { get; set; }
        public string Degree { get; set; }
        public string Proof_Title { get; set; }
        public byte[] Proof_File { get; set; }
        public Nullable<long> Status_EduBackgroundId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedIP { get; set; }
    
        public virtual MemberMaster MemberMaster { get; set; }
        public virtual Status_EduBackground Status_EduBackground { get; set; }
    }
}
