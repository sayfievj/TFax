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
    
    public partial class Country
    {
        public Country()
        {
            this.Cities = new HashSet<City>();
            this.MemberMasters = new HashSet<MemberMaster>();
            this.States = new HashSet<State>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public string CodeISO3 { get; set; }
        public string TimeZone { get; set; }
    
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<MemberMaster> MemberMasters { get; set; }
        public virtual ICollection<State> States { get; set; }
    }
}
