using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class produto_Lista : System.Web.UI.Page
    {
        private Cliente cliente;
        private Funcionario funcionario;

        protected void Page_Load(object sender, EventArgs e)
        {
            cliente = (Cliente)Session["usuario"];
            if (cliente == null)
                Response.Redirect("login.aspx", false);
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        funcionario = context.Funcionario.Where(f => f.cpf.Equals(cliente.cpf)).FirstOrDefault();
                        if (funcionario != null)
                        {
                            if (context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault() != null)
                                preencher_Tabela();
                            else
                                Response.Write("<script>alert('erro no BD - Oficina nao cadastrada');</script>");
                        }
                        else
                        {
                            Response.Redirect("home.aspx", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void preencher_Tabela()
        {
        }

        protected void btn_CadastrarProduto_Click(object sender, EventArgs e)
        {
        }
    }
}