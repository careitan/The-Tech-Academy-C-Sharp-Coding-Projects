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
    
    public partial class v_UserQuotes
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public System.DateTime DOB { get; set; }
        public int UserId { get; set; }
        public int CarMakeModelId { get; set; }
        public int Year { get; set; }
        public bool IsFullCoverage { get; set; }
        public bool DUI { get; set; }
        public int SpeedingTickets { get; set; }
        public Nullable<decimal> Premium { get; set; }
    }
}
