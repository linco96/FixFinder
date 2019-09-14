using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Controls
{
    public partial class Header_Control : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Cliente c = (Cliente)Session["usuario"];
            if (c != null)
            {
                btn_Dashboard.Visible = true;
                btn_Sair.Visible = true;
                btn_Entrar.Visible = false;
            }
            else
            {
                btn_Dashboard.Visible = false;
                btn_Sair.Visible = false;
                btn_Entrar.Visible = true;
            }
        }

        protected void btn_Pesquisar_Click(object sender, EventArgs e)
        {
            Response.Redirect("oficina_Pesquisar.aspx", false);
        }

        protected void btn_Entrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Pages/login.aspx", false);
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("oficina_Pesquisar.aspx", false);
        }

        protected void btn_Dashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Pages/home.aspx", false);
        }
    }
}