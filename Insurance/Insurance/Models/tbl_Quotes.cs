//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Insurance.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Quotes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarMakeModelId { get; set; }
        public int Year { get; set; }
        public bool IsFullCoverage { get; set; }
        public bool DUI { get; set; }
        public int SpeedingTickets { get; set; }
        public Nullable<decimal> Premium { get; set; }
    
        public virtual tbl_CarMakeModel tbl_CarMakeModel { get; set; }
        public virtual tbl_Users tbl_Users { get; set; }
    }
}
