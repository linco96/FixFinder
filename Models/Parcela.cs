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
    
    public partial class Parcela
    {
        public int idParcela { get; set; }
        public double valor { get; set; }
        public System.DateTime data { get; set; }
        public int parcela1 { get; set; }
        public int idOrcamento { get; set; }
    
        public virtual Pagamento Pagamento { get; set; }
    }
}
