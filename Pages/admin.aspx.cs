using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class admin : System.Web.UI.Page
    {
        private Admin adm;
        public bool gerarGrafico;
        public static String jsonGrafico;

        protected void Page_Load(object sender, EventArgs e)
        {
            adm = (Admin)Session["admin"];
            if (adm == null)
            {
                Response.Redirect("admin_Login.aspx", false);
            }
            else
            {
                lbl_Nome.Text = adm.nome;
            }
        }

        protected void btn_GerarGrafico_Click(object sender, EventArgs e)
        {
            try
            {
                if (validacaoDatas())
                {
                    DateTime dtIncio = DateTime.Parse(txt_DataInicio.Text);
                    DateTime dtFim = DateTime.Parse(txt_DataFim.Text);
                    dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                    gerarGrafico = true;
                    switch (select_Grafico.SelectedValue)
                    {
                        case "usuariosUnicos":
                            Log_Login log;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger mt-3";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        private bool validacaoDatas()
        {
            DateTime dtInicio, dtFim;

            if (txt_DataInicio.Text != "" && txt_DataFim.Text != "")
            {
                dtInicio = DateTime.Parse(txt_DataInicio.Text);
                dtFim = DateTime.Parse(txt_DataFim.Text);

                if ((dtFim - dtInicio).TotalDays > 720)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "O intervalo máximo para a geraçao de gráfico é de 2 anos";
                    return false;
                }

                if (dtInicio < new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de início tem que ser maior que 01/01/2010";
                    return false;
                }

                if (dtInicio >= DateTime.Today)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de início tem que ser menor do que a data de hoje";
                    return false;
                }

                if (dtFim > DateTime.Today)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de fim tem que ser menor ou igual a data de hoje";
                    return false;
                }

                if (dtInicio >= dtFim)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de início tem que ser menor do que a data fim";
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}