using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class cadastroCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txt_Senha.Text.Equals(txt_SenhaConfirma.Text))
                {
                    lbl_Alert.Text = "As senhas informadas não coincidem";
                    pnl_Alert.Visible = true;
                    pnl_Alert.CssClass = "alert alert-danger";
                }
                else
                {
                    Cliente c = new Cliente
                    {
                        cpf = txt_CPF.Text.Replace(".", "").Replace("-", ""),
                        nome = txt_Nome.Text,
                        telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                        email = txt_Email.Text,
                        login = txt_Login.Text,
                        senha = txt_Senha.Text,
                        dataNascimento = DateTime.Parse(date_DataNascimento.Text)
                    };

                    using (var context = new DatabaseEntities())
                    {
                        if (context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).ToList().Count > 0)
                        {
                            lbl_Alert.Text = "Um usuário com o CPF informado já existe";
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-danger";
                        }
                        else if (context.Cliente.Where(cliente => cliente.email.Equals(c.email)).ToList().Count > 0)
                        {
                            lbl_Alert.Text = "Um usuário com o e-mail informado já existe";
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-danger";
                        }
                        else if (context.Cliente.Where(cliente => cliente.email.Equals(c.email)).ToList().Count > 0)
                        {
                            lbl_Alert.Text = "Um usuário com o login informado já existe";
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-danger";
                        }
                        else
                        {
                            context.Cliente.Add(c);
                            context.SaveChanges();
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-success";
                            lbl_Alert.Text = "Usuário cadastrado com sucesso";
                        }
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