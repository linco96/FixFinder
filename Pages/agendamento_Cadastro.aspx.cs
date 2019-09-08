using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class agendamento_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            o = (Oficina)Session["oficina"];
            c = (Cliente)Session["usuario"];
            if (o == null)
            {
                Response.Redirect("oficina_Pesquisar.aspx", false);
            }
            else if (c == null)
            {
                Session["lastPage"] = "agendamento_Cadastro.aspx";
                Response.Redirect("login.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<Veiculo> veiculos = context.Veiculo.Where(v => v.cpfCliente.Equals(c.cpf)).ToList();
                    if (veiculos.Count == 0)
                    {
                        Session["lastPage"] = "agendamento_Cadastro.aspx";
                        Response.Redirect("veiculo_Cadastro.aspx");
                    }
                    else
                    {
                        if (Session["lastPage"] != null)
                            Session["lastPage"] = null;
                        preencherSelect();
                    }
                }
            }
        }

        protected void preencherSelect()
        {
            try
            {
                if (txt_Veiculo.Items.Count == 0)
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        ListItem item;

                        List<Veiculo> veiculos = context.Veiculo.Where(v => v.cpfCliente.Equals(c.cpf)).ToList();
                        foreach (Veiculo v in veiculos)
                        {
                            item = new ListItem();
                            item.Text = v.modelo + " - " + v.placa;
                            item.Value = v.idVeiculo.ToString();
                            txt_Veiculo.Items.Add(item);
                        }

                        txt_Horario.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_CarregarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime data;
                if (DateTime.TryParse(txt_Data.Text, out data))
                {
                    if (txt_Horario.Items.Count > 0)
                        txt_Horario.Items.Clear();
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        ListItem item;

                        List<Agendamento> agendamentos = context.Agendamento.Where(a => a.cnpjOficina.Equals(o.cnpj) && a.data.Equals(data)).ToList();

                        TimeSpan horario = o.horaAbertura;
                        TimeSpan horarioFechamento = o.horaFechamento;
                        TimeSpan intervalo = o.duracaoAtendimento;

                        if (agendamentos.Count == 0)
                        {
                            while (true)
                            {
                                if (horario + intervalo > horarioFechamento)
                                    break;

                                item = new ListItem();
                                item.Text = horario.Hours.ToString("00") + ":" + horario.Minutes.ToString("00");
                                item.Value = horario.ToString();
                                txt_Horario.Items.Add(item);

                                horario += intervalo;
                            }
                        }
                        else
                        {
                            while (true)
                            {
                                if (horario + intervalo > horarioFechamento)
                                    break;

                                if (verificarHorario(horario, agendamentos))
                                {
                                    item = new ListItem();
                                    item.Text = horario.Hours.ToString("00") + ":" + horario.Minutes.ToString("00");
                                    item.Value = horario.ToString();
                                    txt_Horario.Items.Add(item);
                                }

                                horario += intervalo;
                            }
                        }
                        while (true)
                        {
                            if (horario + intervalo > horarioFechamento)
                                break;

                            if (verificarHorario(horario, agendamentos))
                            {
                                item = new ListItem();
                                item.Text = horario.Hours.ToString("00") + ":" + horario.Minutes.ToString("00");
                                item.Value = horario.ToString();
                                txt_Horario.Items.Add(item);
                            }

                            horario += intervalo;
                        }

                        txt_Horario.Enabled = true;
                    }
                }
                else
                {
                    if (txt_Horario.Items.Count > 0)
                        txt_Horario.Items.Clear();
                    txt_Horario.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected bool verificarHorario(TimeSpan horario, List<Agendamento> agendamentos)
        {
            int instancias = 0;
            foreach (Agendamento a in agendamentos)
            {
                if (a.hora.Equals(horario))
                    instancias++;

                if (instancias == o.capacidadeAgendamentos)
                    return false;

                instancias = 0;
            }
            return true;
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
        }
    }
}