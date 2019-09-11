using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class funcionario_Editar : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private static bool alterar;

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
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    if (f == null)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else
                    {
                        Funcionario func = (Funcionario)Session["funcionarioEditar"];
                        func = context.Funcionario.Where(funcionario => funcionario.cpf.Equals(func.cpf)).FirstOrDefault();
                        if (func != null)
                        {
                            if (!alterar)
                            {
                                txt_CPF.Text = func.cpf;
                                txt_Nome.Text = func.Cliente.nome;
                                txt_Cargo.Text = func.cargo;
                                txt_Salario.Text = func.salario.ToString();
                                ddl_Banco.SelectedValue = func.banco.ToString();
                                txt_Agencia.Text = func.agencia;
                                txt_Conta.Text = func.conta;
                                alterar = true;
                            }
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
                        else
                        {
                            Response.Redirect("home.aspx");
                        }
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
                    f = context.Funcionario.Where(func => func.cpf.Equals(f.cpf)).FirstOrDefault();
                    f.cargo = txt_Cargo.Text;
                    f.salario = double.Parse(txt_Salario.Text);
                    f.banco = int.Parse(ddl_Banco.SelectedValue);
                    f.agencia = txt_Agencia.Text;
                    f.conta = txt_Conta.Text;
                    context.SaveChanges();

                    Session["funcionario"] = null;
                    alterar = false;
                    Response.Redirect("funcionario_Lista.aspx", false);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            alterar = false;
            Session["funcionarioEditar"] = null;
            Response.Redirect("funcionario_Lista.aspx", false);
        }
    }
}