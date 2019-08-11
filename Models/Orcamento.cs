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
    
    public partial class Orcamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orcamento()
        {
            this.ServicosOrcamento = new HashSet<ServicosOrcamento>();
        }
    
        public int idOrcamento { get; set; }
        public double valor { get; set; }
        public System.DateTime data { get; set; }
        public string status { get; set; }
        public string cpfFuncionario { get; set; }
        public string cnpjOficina { get; set; }
        public int idVeiculo { get; set; }
    
        public virtual Oficina Oficina { get; set; }
        public virtual Veiculo Veiculo { get; set; }
        public virtual Pagamento Pagamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServicosOrcamento> ServicosOrcamento { get; set; }
    }
}
