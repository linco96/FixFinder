using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder
{
    public partial class dashboard1 : System.Web.UI.Page
    {
        private Funcionario f;
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];

            using (DatabaseEntities context = new DatabaseEntities())
            {
                Funcionario f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                lbl_Nome.Text = c.nome;
                if (f == null)
                {
                    pnl_Oficina.Visible = false;
                    btn_CadastroOficina.Visible = true;

                    List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(r => r.cpfCliente.Equals(c.cpf)).ToList();
                    if (requisicoes.Count > 0)
                    {
                        pnl_Funcionario.Visible = true;
                        badge_Requisicoes.InnerHtml = requisicoes.Count.ToString();
                    }
                    else
                    {
                        pnl_Funcionario.Visible = false;
                    }
                }
                else
                {
                    pnl_Oficina.Visible = true;
                    pnl_Funcionario.Visible = false;
                    btn_CadastroOficina.Visible = false;
                    lbl_Nome.Text += " | " + f.Oficina.nome;
                    if (f.cargo.ToLower().Equals("gerente"))
                    {
                        btn_Configuracoes.Visible = true;
                        btn_Funcionarios.Visible = true;
                    }
                    else
                    {
                        btn_Configuracoes.Visible = false;
                        btn_Funcionarios.Visible = false;
                    }
                }
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}