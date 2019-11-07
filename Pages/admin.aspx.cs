using FixFinder.Models;
using Newtonsoft.Json;
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
                            List<DataPointArea> dataPoints1 = new List<DataPointArea>();
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                List<Log_Login> logsTemp = context.Log_Login.Where(a => a.data >= dtIncio && a.data <= dtFim).ToList();
                                //Seta as variaveis
                                DateTime dt = dtIncio;
                                DataPointArea dtp;
                                double mili;
                                dt = new DateTime(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);

                                logsTemp = logsTemp.OrderBy(l => l.data).ToList();

                                //Preenche os datapoints
                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                    dtp = new DataPointArea(mili, 0);
                                    dataPoints1.Add(dtp);

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                        break;
                                    else
                                        dt = dt.AddMonths(1);
                                }

                                List<Log_Login> lista = new List<Log_Login>();
                                Log_Login temp;
                                foreach (Log_Login log in logsTemp)
                                {
                                    dt = log.data;
                                    dt = new DateTime(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in dataPoints1)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            temp = lista.Find(l => l.cpfCliente.Equals(log.cpfCliente) && l.data.Month == log.data.Month && l.data.Year == log.data.Year);

                                            if (temp == null)
                                            {
                                                dtpArea.Y++;
                                                lista.Add(log);
                                                break;
                                            }
                                        }
                                    }
                                }
                                jsonGrafico = JsonConvert.SerializeObject(dataPoints1);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger mt-3";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + " " + ex.StackTrace + "Por favor entre em contato com o suporte";
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