using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class relatorio : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private static String tituloGrafico;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];

            if (c == null)
                Response.Redirect("login.aspx", false);
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
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
                    if (f != null && f.cargo.ToLower().Equals("gerente"))
                    {
                        pnl_Alert.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("home.aspx", false);
                    }
                }
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        protected void btn_GerarGrafico_Click(object sender, EventArgs e)
        {
            if (validacaoDatas())
            {
            }
        }

        private Boolean validacaoDatas()
        {
            DateTime dtInicio, dtFim;
            dtInicio = DateTime.Parse(txt_DataInicio.Text);
            dtFim = DateTime.Parse(txt_DataFim.Text);

            if (dtInicio >= DateTime.Today)
            {
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "A data de início tem que ser menor do que a data de hoje";
                return false;
            }

            if (dtFim > DateTime.Today)
            {
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "A data de fim tem que ser menor ou igual a data de hoje";
                return false;
            }

            if (dtInicio >= dtFim)
            {
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "A data de início tem que ser menor do que a data fim";
                return false;
            }

            return true;
        }
    }
}