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
    
    public partial class Worker
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string telephone { get; set; }
        public int Id_dep { get; set; }
        public int Id { get; set; }
    
        public virtual Department Department { get; set; }
    }
}
