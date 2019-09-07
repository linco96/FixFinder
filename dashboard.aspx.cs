using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder
{
    public partial class dashboard1 : System.Web.UI.Page
    {
        private Funcionario f;
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            Cliente c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("Pages/login.aspx", false);
            }
            else
            {
                lbl_Nome.Text = c.nome;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Funcionario f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    if (f == null)
                    {
                        pnl_Oficina.Visible = false;
                    }
                    else
                    {
                        pnl_Oficina.Visible = true;
                    }
                }
            }
        }
    }
}