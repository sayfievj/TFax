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
    
    public partial class Profile_Links
    {
        public long Id { get; set; }
        public Nullable<long> ProfileId { get; set; }
        public string LinkTitle { get; set; }
        public string LinkURL { get; set; }
        public Nullable<long> Status_LinkId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedIP { get; set; }
    
        public virtual Profile_Master Profile_Master { get; set; }
        public virtual Status_Links Status_Links { get; set; }
    }
}