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
    
    public partial class Status_TextContents
    {
        public Status_TextContents()
        {
            this.TextContents = new HashSet<TextContent>();
        }
    
        public long Id { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<TextContent> TextContents { get; set; }
    }
}
