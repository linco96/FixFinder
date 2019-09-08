using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class agendamento_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            o = (Oficina)Session["oficina"];
            c = (Cliente)Session["usuario"];
            if (o == null)
            {
                Response.Redirect("oficina_Pesquisar", false);
            }
            else if (c == null)
            {
            }
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
        }
    }
}