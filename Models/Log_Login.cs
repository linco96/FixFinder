//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FixFinder.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log_Login
    {
        public int idLogin { get; set; }
        public string cpfCliente { get; set; }
        public System.DateTime data { get; set; }
    
        public virtual Cliente Cliente { get; set; }
    }
}