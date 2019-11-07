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
                oficinaAtivas();
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
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                List<DataPointArea> dataPoints = new List<DataPointArea>();
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
                                    dataPoints.Add(dtp);

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

                                    foreach (DataPointArea dtpArea in dataPoints)
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
                                jsonGrafico = JsonConvert.SerializeObject(dataPoints);
                            }
                            break;

                        case "qtdPesquisa":
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                List<DataPointArea> dataPoints = new List<DataPointArea>();
                                List<LogPesquisa> pesquisas = context.LogPesquisa.Where(l => l.data >= dtIncio && l.data <= dtFim).ToList();

                                DateTime dt = dtIncio;
                                DataPointArea dtp;
                                double mili;

                                dt = new DateTime(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);

                                pesquisas = pesquisas.OrderBy(p => p.data).ToList();

                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                    dtp = new DataPointArea(mili, 0);
                                    dataPoints.Add(dtp);

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                        break;
                                    else
                                        dt = dt.AddMonths(1);
                                }

                                foreach (LogPesquisa log in pesquisas)
                                {
                                    dt = log.data;
                                    dt = new DateTime(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpA in dataPoints)
                                    {
                                        if (dtpA.X == mili)
                                        {
                                            dtpA.Y++;
                                            break;
                                        }
                                    }
                                }
                                jsonGrafico = JsonConvert.SerializeObject(dataPoints);
                            }
                            break;

                        case "usuarioAtivo":
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                List<DataPointArea> dataPoints = new List<DataPointArea>();
                                List<Log_Login> loginTemp = context.Log_Login.Where(l => l.data >= dtIncio && l.data <= dtFim).ToList();
                                List<Cliente> clientes = context.Cliente.ToList();

                                List<Log_Login> loginUnico = new List<Log_Login>();
                                Log_Login logTemp;
                                //Pega o login unico no mes
                                foreach (Log_Login log in loginTemp)
                                {
                                    logTemp = loginUnico.Find(l => l.cpfCliente.Equals(log.cpfCliente) && l.data.Month == log.data.Month && l.data.Year == log.data.Year);
                                    if (logTemp == null)
                                    {
                                        loginUnico.Add(log);
                                    }
                                }

                                //Gera grafico
                                DateTime dt = dtIncio;
                                DataPointArea dtp;
                                double mili;
                                dt = new DateTime(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                    dtp = new DataPointArea(mili, 0);
                                    dataPoints.Add(dtp);

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                        break;
                                    else
                                        dt = dt.AddMonths(1);
                                }

                                loginUnico = loginUnico.OrderBy(l => l.data).ToList();
                                int loginUnicoMes;

                                if (clientes.Count > 0)
                                {
                                    foreach (DataPointArea temp in dataPoints)
                                    {
                                        loginUnicoMes = 0;
                                        foreach (Log_Login log in loginUnico)
                                        {
                                            dt = log.data;
                                            dt = new DateTime(new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
                                            mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                            if (temp.X == mili)
                                            {
                                                loginUnicoMes++;
                                                temp.Y = (double)loginUnicoMes / (double)clientes.Count;
                                            }
                                        }
                                    }
                                }
                                jsonGrafico = JsonConvert.SerializeObject(dataPoints);
                                break;
                            }
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

        private void oficinaAtivas()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<Oficina> oficinaAtiva = context.Oficina.Where(o => o.statusAssinatura == 1).ToList();
                    List<Oficina> oficinaInativa = context.Oficina.Where(o => o.statusAssinatura == 0).ToList();

                    lbl_Ativas.Text = oficinaAtiva.Count + " Ativas";
                    lbl_Inativas.Text = oficinaInativa.Count + " Inativas";
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger mt-3";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + " " + ex.StackTrace + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }
    }
}