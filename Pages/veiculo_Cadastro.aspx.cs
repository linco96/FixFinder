using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class veiculo_Cadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
        }

        protected void radio_ModeloPlaca_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tipoPlaca = radio_ModeloPlaca.SelectedValue;

            switch (tipoPlaca)
            {
                case "nova":
                    break;

                case "antiga":
                    break;

                default:
                    break;
            }
        }
    }
}