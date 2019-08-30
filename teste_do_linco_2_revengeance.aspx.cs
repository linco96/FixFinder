using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder
{
    public partial class teste_do_linco_2_revengeance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Session["usuario"] = context.Cliente.Where(cliente => cliente.cpf.Equals("06850142909")).FirstOrDefault();
            }
            Response.Redirect("Pages/orcamento_Cadastro.aspx", false);
        }
    }
}