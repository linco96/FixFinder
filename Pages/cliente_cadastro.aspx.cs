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
            if (txt_Senha.Text.Length < 6)
            {
                lbl_Alert.Text = "A senha deve ter seis ou mais caracteres";
                pnl_Alert.Visible = true;
            }

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
                    lbl_Alert.Visible = true;
                }
                else if (context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).ToList().Count > 0)
                {
                }
                else
                {
                    context.Cliente.Add(c);
                    context.SaveChanges();
                    pnl_Alert.Visible = false;
                }
            }
        }
    }
}