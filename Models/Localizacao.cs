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
    
    public partial class Localizacao
    {
        public int idLocalizacao { get; set; }
        public string logradouro { get; set; }
        public int numero { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cpfCliente { get; set; }
    
        public virtual Cliente Cliente { get; set; }
    }
}
