using System;
using FixFinder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class veiculo_Lista : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
                Response.Redirect("login.aspx", false);
        }

        protected void btn_CadastrarVeiculo_Click(object sender, EventArgs e)
        {
            Response.Redirect("veiculo_Cadastro.aspx", false);
        }

        private void preencher_Tabela()
        {
        }
    }
}