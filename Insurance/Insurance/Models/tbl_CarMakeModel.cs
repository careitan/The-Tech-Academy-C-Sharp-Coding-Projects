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
    
    public partial class tbl_CarMakeModel
    {
        public int Id { get; set; }
        public int CarMakeId { get; set; }
        public int CarModelId { get; set; }
        public int MakeRisk { get; set; }
        public int ModelRisk { get; set; }
    
        public virtual tlkp_CarMake tlkp_CarMake { get; set; }
        public virtual tlkp_CarModel tlkp_CarModel { get; set; }
        public virtual tbl_Quotes tbl_Quotes { get; set; }
    }
}
