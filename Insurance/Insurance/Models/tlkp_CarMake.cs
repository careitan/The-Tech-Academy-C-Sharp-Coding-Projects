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
    
    public partial class tlkp_CarMake
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tlkp_CarMake()
        {
            this.tbl_CarMakeModel = new HashSet<tbl_CarMakeModel>();
        }
    
        public int Id { get; set; }
        public string Manufacturer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_CarMakeModel> tbl_CarMakeModel { get; set; }
    }
}
