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
    
    public partial class HTTPTraceListener
    {
        public int Id { get; set; }
        public string RequestUrl { get; set; }
        public string TraceMessage { get; set; }
        public Nullable<System.DateTime> TraceDate { get; set; }
        public string UserName { get; set; }
        public string UserHostAddress { get; set; }
        public string UserAgent { get; set; }
        public string UserHostName { get; set; }
        public string CategoryName { get; set; }
    }
}