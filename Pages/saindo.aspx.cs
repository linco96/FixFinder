using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class saindo : System.Web.UI.Page
    {
        //TE DIZER SENHOR MATHEUS
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("oficina_Pesquisar.aspx", false);
        }
    }
}