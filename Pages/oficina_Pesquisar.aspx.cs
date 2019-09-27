using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class oficina_Pesquisar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Pesquisar_Click(object sender, EventArgs e)
        {
        }

        protected void btn_CarregarEndereco_Click(object sender, EventArgs e)
        {
            pnl_Alert.Visible = true;
            pnl_Alert.CssClass = "alert alert-success";
            lbl_Alert.Text = txt_Pesquisa.Text;
        }
    }
}