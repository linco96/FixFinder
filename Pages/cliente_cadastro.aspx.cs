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
                    }
                    else if (context.Cliente.Where(cliente => cliente.email.Equals(c.email)).ToList().Count > 0)
                    {
                        lbl_Alert.Text = "Um usuário com o e-mail informado já existe";
                        pnl_Alert.Visible = true;
                    }
                    else if (context.Cliente.Where(cliente => cliente.email.Equals(c.email)).ToList().Count > 0)
                    {
                        lbl_Alert.Text = "Um usuário com o login informado já existe";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        context.Cliente.Add(c);
                        context.SaveChanges();
                        pnl_Alert.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }
    }
}