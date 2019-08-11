using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            using (var context = new DatabaseEntities())
            {
                Cliente c = new Cliente
                {
                    cpf = "a",
                    nome = "a",
                    telefone = "a",
                    email = "a",
                    login = "a",
                    senha = "a",
                    dataNascimento = DateTime.Now
                };
                context.Cliente.Add(c);
                context.SaveChanges();
            }

        }

        protected void btn_Esqueci_Senha_Click(object sender, EventArgs e)
        {

        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {

        }
    }
}