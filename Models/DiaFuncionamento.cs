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
    
    public partial class DiaFuncionamento
    {
        public string cnpjOficina { get; set; }
        public System.TimeSpan horaAbertura { get; set; }
        public System.TimeSpan horaFechamento { get; set; }
        public string diaSemana { get; set; }
    
        public virtual Oficina Oficina { get; set; }
    }
}
