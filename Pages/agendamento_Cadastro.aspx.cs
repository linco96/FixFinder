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
                    Funcionario f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();

                    if (f != null && f.cnpjOficina.Equals(o.cnpj) && f.cargo.ToLower() == "gerente")
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else
                    {
                        List<Agendamento> agendamentos = context.Agendamento.Where(a => a.cnpjOficina.Equals(o.cnpj) && a.cpfCliente.Equals(c.cpf)).ToList();
                        bool existe = false;
                        if (agendamentos.Count > 0)
                        {
                            foreach (Agendamento a in agendamentos)
                            {
                                if (a.status.Equals("Confirmação pendente") || a.status.Equals("Confirmado"))
                                {
                                    existe = true;
                                    break;
                                }
                            }
                        }

                        if (existe)
                        {
                            Response.Redirect("agendamento_ListaCliente.aspx", false);
                        }
                        else
                        {
                            List<Veiculo> veiculos = context.Veiculo.Where(v => v.cpfCliente.Equals(c.cpf)).ToList();
                            if (veiculos.Count == 0)
                            {
                                Session["lastPage"] = "agendamento_Cadastro.aspx";
                                Response.Redirect("veiculo_Cadastro.aspx");
                            }
                            else
                            {
                                o = context.Oficina.Where(of => of.cnpj.Equals(o.cnpj)).FirstOrDefault();
                                if (Session["lastPage"] != null)
                                    Session["lastPage"] = null;
                                preencherSelect();
                                lbl_Oficina.InnerText = o.nome;
                                if (o.reputacao == null)
                                    lbl_Reputacao.Text = "-/10";
                                else
                                    lbl_Reputacao.Text = o.reputacao.ToString().Replace(",", ".") + "/10";
                                lbl_Endereco.InnerHtml = o.Endereco.logradouro + ", " + o.Endereco.numero.ToString() + "<br />" + o.Endereco.cep + " - " + o.Endereco.cidade + " " + o.Endereco.uf.ToUpper();
                            }
                        }
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
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    DateTime data;
                    if (DateTime.TryParse(txt_Data.Text, out data))
                    {
                        int diaSemana = (int)data.DayOfWeek;
                        DiaFuncionamento diaFuncionamento;

                        if (diaSemana == 0)
                        {
                            diaFuncionamento = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(o.cnpj) && d.diaSemana.Equals("domingo")).FirstOrDefault();
                        }
                        else if (diaSemana >= 1 && diaSemana <= 5)
                        {
                            diaFuncionamento = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(o.cnpj) && d.diaSemana.Equals("util")).FirstOrDefault();
                        }
                        else
                        {
                            diaFuncionamento = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(o.cnpj) && d.diaSemana.Equals("sabado")).FirstOrDefault();
                        }

                        if (diaFuncionamento != null)
                        {
                            if (data.Date.CompareTo(DateTime.Now.Date) >= 0)
                            {
                                if (txt_Horario.Items.Count > 0)
                                    txt_Horario.Items.Clear();

                                ListItem item;

                                List<Agendamento> agendamentos = context.Agendamento.Where(a => a.cnpjOficina.Equals(o.cnpj) && a.data.Equals(data)).ToList();

                                TimeSpan horario = diaFuncionamento.horaAbertura;
                                TimeSpan horarioFechamento = diaFuncionamento.horaFechamento;
                                TimeSpan intervalo = o.duracaoAtendimento;

                                if (data.Date.Equals(DateTime.Now.Date))
                                {
                                    TimeSpan now = DateTime.Now.TimeOfDay;
                                    while (true)
                                    {
                                        if (horario > now)
                                            break;
                                        horario += intervalo;
                                    }
                                }

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

                                if (txt_Horario.Items.Count > 0)
                                {
                                    txt_Horario.Enabled = true;
                                    pnl_Alert.Visible = false;
                                }
                                else
                                {
                                    pnl_Alert.CssClass = "alert alert-danger";
                                    lbl_Alert.Text = "Selecione uma data válida";
                                    pnl_Alert.Visible = true;
                                    txt_Horario.Enabled = false;
                                }
                            }
                            else
                            {
                                pnl_Alert.CssClass = "alert alert-danger";
                                lbl_Alert.Text = "Selecione uma data válida";
                                pnl_Alert.Visible = true;
                                if (txt_Horario.Items.Count > 0)
                                    txt_Horario.Items.Clear();
                                txt_Horario.Enabled = false;
                            }
                        }
                        else
                        {
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "A oficina selecionada não abre no dia da semana especificado";
                            pnl_Alert.Visible = true;
                            if (txt_Horario.Items.Count > 0)
                                txt_Horario.Items.Clear();
                            txt_Horario.Enabled = false;
                        }
                    }
                    else
                    {
                        if (txt_Horario.Items.Count > 0)
                            txt_Horario.Items.Clear();
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

        protected bool verificarHorario(TimeSpan horario, List<Agendamento> agendamentos)
        {
            int instancias = 0;
            foreach (Agendamento a in agendamentos)
            {
                if (a.hora.Equals(horario))
                    instancias++;
            }
            if (instancias >= o.capacidadeAgendamentos)
                return false;
            else
                return true;
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Horario.Items.Count == 0)
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Informe uma data e um horário válidos";
                    pnl_Alert.Visible = true;
                }
                else
                {
                    DateTime data = DateTime.Parse(txt_Data.Text + " " + txt_Horario.SelectedValue);
                    if (data.CompareTo(DateTime.Now) < 0)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Informe uma data e um horário válidos";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        using (DatabaseEntities context = new DatabaseEntities())
                        {
                            Agendamento a = new Agendamento()
                            {
                                data = data.Date,
                                hora = data.TimeOfDay,
                                cnpjOficina = o.cnpj,
                                cpfCliente = c.cpf,
                                idVeiculo = int.Parse(txt_Veiculo.SelectedValue),
                                status = "Confirmação pendente"
                            };
                            context.Agendamento.Add(a);
                            context.SaveChanges();
                            Response.Redirect("agendamento_ListaCliente.aspx", false);
                        }
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
    }
}