using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class agendamento_Cadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Parse(txt_Data.Text + " " + txt_Horario.Text);
            pnl_Alert.Visible = true;
            lbl_Alert.Text = dt.ToString();
        }
    }
}