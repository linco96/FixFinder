using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class orcamento_Avaliar : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Session["orcamento"] = null;
                Response.Redirect("login.aspx", false);
            }
            else
            {
                Orcamento orcamento = (Orcamento)Session["orcamento"];
                if (orcamento != null && orcamento.cpfCliente == c.cpf)
                {
                    if (!IsPostBack)
                        preencher_Orcamento(orcamento);
                }
                else
                {
                    Response.Redirect("orcamento_ListaCliente.aspx", false);
                }
            }
        }

        private void preencher_Orcamento(Orcamento orcamento)
        {
        }
    }
}