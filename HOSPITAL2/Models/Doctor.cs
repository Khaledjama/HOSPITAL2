//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HOSPITAL2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctor()
        {
            this.patient = new HashSet<patient>();
        }
    
        public string Name { get; set; }
        public string Adress { get; set; }
        public string telephone { get; set; }
        public int Id_dep { get; set; }
        public int Id { get; set; }
        public System.TimeSpan EndAt { get; set; }
        public System.TimeSpan startat { get; set; }
        public int num_reveal { get; set; }
        public string serial_num { get; set; }
    
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<patient> patient { get; set; }
    }
}
