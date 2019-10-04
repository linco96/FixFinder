using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class orcamento_Chat : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Session.Clear();
                Response.Redirect("login.aspx", false);
            }
            else
            {
                Orcamento orcamento = (Orcamento)Session["orcamento"];
                if (orcamento != null)
                {
                    pnl_Alert.Visible = false;
                    if (!IsPostBack)
                        preencher_Oficina(orcamento);
                }
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Erro: Orçamento inválido. Por favor entre em contato com o suporte";
                    pnl_Alert.Visible = true;
                }
            }
        }

        private void preencher_Oficina(Orcamento orcamento)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    Oficina oficina = context.Oficina.Where(o => o.cnpj.Equals(orcamento.cnpjOficina)).FirstOrDefault();
                    Veiculo veiculo = context.Veiculo.Where(v => v.idVeiculo == orcamento.idVeiculo).FirstOrDefault();
                    Cliente MecanicoResponsavel = context.Cliente.Where(mec => mec.cpf.Equals(orcamento.cpfFuncionario)).FirstOrDefault();
                    if (oficina != null && veiculo != null && MecanicoResponsavel != null)
                    {
                        lbl_Oficina.InnerText = oficina.nome;
                        lbl_Status.Text = orcamento.status;
                        lbl_Veiculo.Text = veiculo.modelo + " - " + veiculo.placa;
                        lbl_MecanicoResponsavel.Text = MecanicoResponsavel.nome;
                    }
                    else
                    {
                        lbl_Oficina.InnerText = "Erro Oficina";
                        lbl_Status.Text = "";
                        lbl_Veiculo.Text = "";
                        lbl_MecanicoResponsavel.Text = "";
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Erro: Ocorreu um erro com o Orçamento e/ou Oficina. Por favor entre em contato com o suporte";
                        pnl_Alert.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }
    }
}