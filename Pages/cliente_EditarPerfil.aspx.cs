using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class cliente_EditarPerfil : System.Web.UI.Page
    {
        private Cliente c;

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
                if (!alterar)
                {
                    txt_Nome.Text = c.nome;
                    txt_Telefone.Text = c.telefone;
                    txt_Email.Text = c.email;
                    date_DataNascimento.Text = c.dataNascimento.ToString("yyyy-MM-dd");
                    alterar = true;
                }
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
        }

        protected void btn_Salvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Senha.Text.Trim().Length > 0 || txt_SenhaNova.Text.Trim().Length > 0 || txt_SenhaNovaConfirma.Text.Length > 0)
                {
                    if (txt_Senha.Text.Trim().Length == 0 || txt_SenhaNova.Text.Trim().Length == 0 || txt_SenhaNovaConfirma.Text.Length == 0)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Para realizar a alteração da senha informe a sua senha atual e a nova. Caso não deseje alterar a sua senha deixe os campos vazios";
                        pnl_Alert.Visible = true;
                    }
                    else if (c.senha != txt_Senha.Text)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Senha atual incorreta";
                        pnl_Alert.Visible = true;
                    }
                    else if (txt_SenhaNova.Text.Equals(txt_SenhaNovaConfirma.Text))
                    {
                        using (DatabaseEntities context = new DatabaseEntities())
                        {
                            Cliente temp = context.Cliente.Where(cliente => c.cpf == cliente.cpf).FirstOrDefault();
                            temp.nome = txt_Nome.Text;
                            temp.telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                            temp.email = txt_Email.Text;
                            temp.dataNascimento = DateTime.Parse(date_DataNascimento.Text);
                            temp.senha = txt_SenhaNova.Text;

                            context.SaveChanges();

                            Session["usuario"] = temp;

                            txt_Senha.Text = "";
                            txt_SenhaNova.Text = "";
                            txt_SenhaNovaConfirma.Text = "";

                            lbl_Alert.Text = "Alterações realizadas com sucesso";
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-success";
                        }
                    }
                    else
                    {
                        lbl_Alert.Text = "As senhas informadas não coincidem";
                        pnl_Alert.Visible = true;
                        pnl_Alert.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        Cliente temp = context.Cliente.Where(cliente => c.cpf == cliente.cpf).FirstOrDefault();
                        temp.nome = txt_Nome.Text;
                        temp.telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                        temp.email = txt_Email.Text;
                        temp.dataNascimento = DateTime.Parse(date_DataNascimento.Text);

                        context.SaveChanges();

                        Session["usuario"] = temp;

                        lbl_Alert.Text = "Alterações realizadas com sucesso";
                        pnl_Alert.Visible = true;
                        pnl_Alert.CssClass = "alert alert-success";
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

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}