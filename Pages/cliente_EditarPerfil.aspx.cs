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
        protected void Page_Load(object sender, EventArgs e)
        {
            Cliente c = (Cliente)Session["usuario"];
            txt_Nome.Text = c.nome;
            txt_Telefone.Text = c.telefone;
            txt_Email.Text = c.email;
            date_DataNascimento.Text = c.dataNascimento.ToString();
        }

        protected void btn_Salvar_Click(object sender, EventArgs e)
        {
        }
    }
}