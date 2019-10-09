using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace FixFinder.Pages
{
    public partial class orcamento_Chat : System.Web.UI.Page
    {
        private Cliente c;
        private static Thread t1;
        private static Orcamento orcamento;
        private static List<Mensagem> msgLidas;
        public static bool postback;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Session.Clear();
                Response.Redirect("login.aspx", false);
            }
            else
            {
                orcamento = (Orcamento)Session["orcamento"];
                if (orcamento != null)
                {
                    pnl_Alert.Visible = false;

                    if (!IsPostBack)
                    {
                        preencher_Oficina();
                        postback = false;

                        //t1 = new Thread(new ThreadStart(run));
                        //t1.Start();
                    }
                    else
                    {
                        postback = true;
                    }
                }
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Erro: Orçamento inválido. Por favor entre em contato com o suporte";
                    pnl_Alert.Visible = true;
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (orcamento != null)
            {
                preencher_Mensagens();
            }
        }

        private void preencher_Mensagens()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    List<Mensagem> mensagens;
                    mensagens = context.Mensagem.Where(m => m.idOrcamento == orcamento.idOrcamento).ToList();
                    DateTime date;
                    String destinatario;
                    HtmlGenericControl divMensagem;

                    foreach (Mensagem msg in mensagens)
                    {
                        date = msg.data + msg.hora;
                        if (msg.remetente.Equals(c.cpf))
                        {
                            //Remetente
                            divMensagem = new HtmlGenericControl("DIV");
                            divMensagem.Attributes.Add("class", "bg-info ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-right text-light text-break");
                            divMensagem.InnerHtml = "<div class='font-italic'>Enviado em " + date.ToString("dd/MM/yyyy") + " às " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") + " - Eu</div><br />" + msg.mensagem1;
                        }
                        else
                        {
                            //Destinatario
                            if (orcamento.cpfCliente.Equals(c.cpf))
                                destinatario = "Mecânico";
                            else
                                destinatario = "Cliente";
                            divMensagem = new HtmlGenericControl("DIV");
                            divMensagem.Attributes.Add("class", "bg-secondary ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-left text-light text-break");
                            divMensagem.InnerHtml = "<div class='font-italic'>Enviado em " + date.ToString("dd/MM/yyyy") + " às " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") + " - " + destinatario + "</div><br />" + msg.mensagem1;
                        }
                        pnl_Mensagens.Controls.Add(divMensagem);
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

        private async void run()
        {
            try
            {
                List<Mensagem> mensagens;
                List<Mensagem> mensagensExbidas = new List<Mensagem>();
                DateTime date;
                String destinatario;
                HtmlGenericControl divMensagem;

                while (true)
                {
                    if (orcamento != null)
                    {
                        using (var context = new DatabaseEntities())
                        {
                            mensagens = context.Mensagem.Where(m => m.idOrcamento == orcamento.idOrcamento).ToList();

                            foreach (Mensagem msg in mensagens)
                            {
                                if (!lst_Contains(mensagensExbidas, msg))
                                {
                                    date = msg.data + msg.hora;
                                    if (msg.remetente.Equals(c.cpf))
                                    {
                                        //Remetente
                                        divMensagem = new HtmlGenericControl("DIV");
                                        divMensagem.Attributes.Add("class", "bg-info ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-right text-light text-break");
                                        divMensagem.InnerHtml = "<div class='font-italic'>Enviado em " + date.ToString("dd/MM/yyyy") + " às " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") + " - Eu</div><br />" + msg.mensagem1;
                                    }
                                    else
                                    {
                                        //Destinatario
                                        if (orcamento.cpfCliente.Equals(c.cpf))
                                            destinatario = "Mecânico";
                                        else
                                            destinatario = "Cliente";
                                        divMensagem = new HtmlGenericControl("DIV");
                                        divMensagem.Attributes.Add("class", "bg-secondary ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-left text-light text-break");
                                        divMensagem.InnerHtml = "<div class='font-italic'>Enviado em " + date.ToString("dd/MM/yyyy") + " às " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") + " - " + destinatario + "</div><br />" + msg.mensagem1;
                                    }
                                    pnl_Mensagens.Controls.Add(divMensagem);
                                    mensagensExbidas.Add(msg);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean lst_Contains(List<Mensagem> mensagens, Mensagem mensagem)
        {
            foreach (Mensagem msg in mensagens)
            {
                if (msg.idMensagem.Equals(mensagem.idMensagem))
                    return true;
            }

            return false;
        }

        private void preencher_Oficina()
        {
            try
            {
                if (orcamento != null)
                {
                    using (var context = new DatabaseEntities())
                    {
                        Oficina oficina = context.Oficina.Where(o => o.cnpj.Equals(orcamento.cnpjOficina)).FirstOrDefault();
                        Veiculo veiculo = context.Veiculo.Where(v => v.idVeiculo == orcamento.idVeiculo).FirstOrDefault();
                        Cliente MecanicoResponsavel = context.Cliente.Where(mec => mec.cpf.Equals(orcamento.cpfFuncionario)).FirstOrDefault();
                        if (oficina != null && veiculo != null && MecanicoResponsavel != null)
                        {
                            lbl_Oficina.InnerText = oficina.nome;
                            lbl_Status.Text = orcamento.status;
                            lbl_Veiculo.Text = veiculo.modelo + " - " + veiculo.placa;
                            lbl_MecanicoResponsavel.Text = MecanicoResponsavel.nome;
                        }
                        else
                        {
                            lbl_Oficina.InnerText = "Erro Oficina";
                            lbl_Status.Text = "";
                            lbl_Veiculo.Text = "";
                            lbl_MecanicoResponsavel.Text = "";
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Erro: Ocorreu um erro com o Orçamento e/ou Oficina. Por favor entre em contato com o suporte";
                            pnl_Alert.Visible = true;
                        }
                    }
                }
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Erro: Sessao do orcamento foi perdida. Por favor entre em contato com o suporte";
                    pnl_Alert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_EnviarMSG_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    Orcamento orcamento = (Orcamento)Session["orcamento"];
                    if (orcamento != null)
                    {
                        Mensagem msg = new Mensagem()
                        {
                            data = DateTime.Today,
                            hora = DateTime.Now.TimeOfDay,
                            idOrcamento = orcamento.idOrcamento,
                            mensagem1 = txt_Mensagem.Text,
                            remetente = c.cpf,
                            status = 0
                        };
                        context.Mensagem.Add(msg);
                        context.SaveChanges();
                        txt_Mensagem.Text = "";
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Erro: A sessão do orçcamento foi perdida";
                        pnl_Alert.Visible = true;
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

        protected void Page_UnLoad(object sender, EventArgs e)
        {
        }

        protected void btn_Voltar_Click(object sender, EventArgs e)
        {
            //t1.Abort();
            if (orcamento.cpfCliente == c.cpf)
            {
                orcamento = null;
                //t1 = null;
                Session["orcamento"] = null;
                Response.Redirect("orcamento_ListaCliente.aspx", false);
            }
            else
            {
                orcamento = null;
                //t1 = null;
                Session["orcamento"] = null;
                Response.Redirect("orcamento_ListaOficina.aspx", false);
            }
        }

        protected void btn_GambiButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (orcamento != null)
            //    {
            //        using (var context = new DatabaseEntities())
            //        {
            //            if (msgLidas == null)
            //                msgLidas = new List<Mensagem>();
            //            List<Mensagem> mensagens = context.Mensagem.Where(m => m.idOrcamento == orcamento.idOrcamento).ToList();
            //            DateTime date;
            //            HtmlGenericControl divMensagem;
            //            String destinatario;
            //            foreach (Mensagem msg in mensagens)
            //            {
            //                //if (!lst_Contains(msgLidas, msg))
            //                //{
            //                date = msg.data + msg.hora;
            //                if (msg.remetente.Equals(c.cpf))
            //                {
            //                    //Remetente
            //                    divMensagem = new HtmlGenericControl("DIV");
            //                    divMensagem.Attributes.Add("class", "bg-info ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-right text-light text-break");
            //                    divMensagem.InnerHtml = "<div class='font-italic'>Enviado em " + date.ToString("dd/MM/yyyy") + " às " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") + " - Eu</div><br />" + msg.mensagem1;
            //                }
            //                else
            //                {
            //                    //Destinatario
            //                    if (orcamento.cpfCliente.Equals(c.cpf))
            //                        destinatario = "Mecânico";
            //                    else
            //                        destinatario = "Cliente";
            //                    divMensagem = new HtmlGenericControl("DIV");
            //                    divMensagem.Attributes.Add("class", "bg-secondary ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-left text-light text-break");
            //                    divMensagem.InnerHtml = "<div class='font-italic'>Enviado em " + date.ToString("dd/MM/yyyy") + " às " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") + " - " + destinatario + "</div><br />" + msg.mensagem1;
            //                }
            //                pnl_Mensagens.Controls.Add(divMensagem);
            //                msgLidas.Add(msg);
            //                //}
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    pnl_Alert.CssClass = "alert alert-danger";
            //    lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            //    pnl_Alert.Visible = true;
            //}
        }
    }
}