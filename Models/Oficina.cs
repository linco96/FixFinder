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
    
    public partial class Oficina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Oficina()
        {
            this.Agendamento = new HashSet<Agendamento>();
            this.Avaliacao = new HashSet<Avaliacao>();
            this.Compra = new HashSet<Compra>();
            this.DiaFuncionamento = new HashSet<DiaFuncionamento>();
            this.Fornecedor = new HashSet<Fornecedor>();
            this.FotoOficina = new HashSet<FotoOficina>();
            this.Funcionario = new HashSet<Funcionario>();
            this.Orcamento = new HashSet<Orcamento>();
            this.PagamentoAssinatura = new HashSet<PagamentoAssinatura>();
            this.Produto = new HashSet<Produto>();
            this.RequisicaoFuncionario = new HashSet<RequisicaoFuncionario>();
            this.Servico = new HashSet<Servico>();
        }
    
        public string cnpj { get; set; }
        public string nome { get; set; }
        public Nullable<double> reputacao { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public System.TimeSpan duracaoAtendimento { get; set; }
        public int capacidadeAgendamentos { get; set; }
        public byte statusAssinatura { get; set; }
        public string descricao { get; set; }
        public string chavePublica { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agendamento> Agendamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Compra> Compra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiaFuncionamento> DiaFuncionamento { get; set; }
        public virtual Endereco Endereco { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fornecedor> Fornecedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FotoOficina> FotoOficina { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funcionario> Funcionario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orcamento> Orcamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PagamentoAssinatura> PagamentoAssinatura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produto> Produto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequisicaoFuncionario> RequisicaoFuncionario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servico> Servico { get; set; }
    }
}
