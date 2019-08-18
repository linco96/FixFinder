using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder
{
    public partial class teste_do_linco_s2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void butao_Click(object sender, EventArgs e)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Session["usuario"] = context.Cliente.Where(cliente => cliente.cpf.Equals("04643512946")).FirstOrDefault();
            }
            Response.Redirect("dashboard.aspx", false);
        }
    }
}