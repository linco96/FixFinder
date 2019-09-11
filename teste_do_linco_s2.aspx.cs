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
                Session["usuario"] = context.Cliente.Where(cliente => cliente.cpf.Equals("11111111111")).FirstOrDefault();
                Session["orcamento"] = context.Orcamento.Where(o => o.idOrcamento.Equals("1")).FirstOrDefault();
            }
            Response.Redirect("/Pages/orcamento_Avaliar.aspx", false);
        }
    }
}