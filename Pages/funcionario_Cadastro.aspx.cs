using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class funcionario_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    o = context.Oficina.Where(oficina => oficina.cnpj.Equals(c.Funcionario.cnpjOficina)).FirstOrDefault();
                    if (o == null || c.Funcionario.cargo.ToLower() != "gerente")
                    {
                        //mandar pra home
                    }
                }
            }
        }

        protected void btn_Registro_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    RequisicaoFuncionario req = new RequisicaoFuncionario
                    {
                        cpfCliente = txt_CPF.Text,
                        cargo = txt_Cargo.Text,
                        salario = double.Parse(txt_Salario.Text),
                        banco = int.Parse(ddl_Banco.SelectedValue),
                        agencia = txt_Agencia.Text,
                        conta = txt_Conta.Text,
                        cnpjOficina = o.cnpj
                    };
                    context.RequisicaoFuncionario.Add(req);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Puxar_Click(object sender, EventArgs e)
        {
        }
    }
}