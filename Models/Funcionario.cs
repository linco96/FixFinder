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
    
    public partial class Funcionario
    {
        public string cpf { get; set; }
        public string cargo { get; set; }
        public Nullable<int> banco { get; set; }
        public string agencia { get; set; }
        public string conta { get; set; }
        public Nullable<double> salario { get; set; }
        public string cnpjOficina { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Oficina Oficina { get; set; }
    }
}
