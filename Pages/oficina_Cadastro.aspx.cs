using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class oficina_Cadastro : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                c = (Cliente)Session["usuario"];

                if (c == null)
                {
                    Response.Redirect("login.aspx", false);
                }

                Funcionario f;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                }

                if (f != null)
                {
                    Response.Redirect("home.aspx", false);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txt_HorarioAberturaUtil.Text.Length == 0 || txt_HorarioFechamentoUtil.Text.Length == 0) && (txt_HorarioAberturaSabado.Text.Length == 0 || txt_HorarioFechamentoSabado.Text.Length == 0) && (txt_HorarioAberturaDomingo.Text.Length == 0 || txt_HorarioFechamentoDomingo.Text.Length == 0))
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "Informe os horários de abertura e fechamento de pelo menos um dia das semana";
                }
                else
                {
                    using (var context = new DatabaseEntities())
                    {
                        Oficina o = new Oficina
                        {
                            cnpj = txt_CNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", ""),
                            nome = txt_Nome.Text,
                            telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                            email = txt_Email.Text,
                            capacidadeAgendamentos = int.Parse(num_Agendamentos.Text),
                            statusAssinatura = 1,
                            duracaoAtendimento = TimeSpan.Parse(txt_TempoAtendimento.Text)
                        };

                        Endereco endereco = new Endereco()
                        {
                            cep = txt_CEP.Text.Replace("-", ""),
                            cidade = txt_Cidade.Text,
                            cnpjOficina = o.cnpj,
                            complemento = txt_Complemento.Text,
                            logradouro = txt_Rua.Text,
                            numero = int.Parse(txt_Numero.Text),
                            uf = txt_UF.Text,
                            bairro = txt_Bairro.Text
                        };

                        if (context.Oficina.Where(oficina => oficina.cnpj.Equals(o.cnpj)).FirstOrDefault() != null)
                        {
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Uma oficina com este CNPJ já existe";
                        }
                        else
                        {
                            context.Oficina.Add(o);
                            context.Endereco.Add(endereco);

                            DiaFuncionamento dia;
                            if (txt_HorarioAberturaUtil.Text.Length > 0 && txt_HorarioFechamentoUtil.Text.Length > 0)
                            {
                                dia = new DiaFuncionamento()
                                {
                                    diaSemana = "util",
                                    cnpjOficina = o.cnpj,
                                    horaAbertura = TimeSpan.Parse(txt_HorarioAberturaUtil.Text),
                                    horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoUtil.Text)
                                };
                                context.DiaFuncionamento.Add(dia);
                                context.SaveChanges();
                            }
                            if (txt_HorarioAberturaSabado.Text.Length > 0 && txt_HorarioFechamentoSabado.Text.Length > 0)
                            {
                                dia = new DiaFuncionamento()
                                {
                                    diaSemana = "sabado",
                                    cnpjOficina = o.cnpj,
                                    horaAbertura = TimeSpan.Parse(txt_HorarioAberturaSabado.Text),
                                    horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoSabado.Text)
                                };
                                context.DiaFuncionamento.Add(dia);
                                context.SaveChanges();
                            }
                            if (txt_HorarioAberturaDomingo.Text.Length > 0 && txt_HorarioFechamentoDomingo.Text.Length > 0)
                            {
                                dia = new DiaFuncionamento()
                                {
                                    diaSemana = "domingo",
                                    cnpjOficina = o.cnpj,
                                    horaAbertura = TimeSpan.Parse(txt_HorarioAberturaDomingo.Text),
                                    horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoDomingo.Text)
                                };
                                context.DiaFuncionamento.Add(dia);
                                context.SaveChanges();
                            }

                            Funcionario f = new Funcionario
                            {
                                cargo = "Gerente",
                                cpf = c.cpf,
                                cnpjOficina = o.cnpj,
                            };

                            context.Funcionario.Add(f);

                            context.SaveChanges();

                            autorizar(o);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }

        protected async void btn_CarregarEndereco_Click(object sender, EventArgs e)
        {
            if (txt_CEP.Text.Length > 0)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://viacep.com.br/ws/" + txt_CEP.Text + "/piped/");
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode && !responseString.Equals("erro:true"))
                {
                    txt_Rua.Text = responseString.Split('|')[1].Split(':')[1];
                    txt_Bairro.Text = responseString.Split('|')[3].Split(':')[1];
                    txt_Cidade.Text = responseString.Split('|')[4].Split(':')[1];
                    txt_UF.Text = responseString.Split('|')[5].Split(':')[1];
                    alert_CEP.Visible = false;
                }
                else
                {
                    txt_Rua.Text = "";
                    txt_Bairro.Text = "";
                    txt_Cidade.Text = "";
                    txt_UF.Text = "";
                    txt_CEP.Text = "";
                    alert_CEP.Visible = true;
                }
            }
        }

        protected async void autorizar(Oficina o)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    HttpClient client = new HttpClient();
                    CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();
                    StringBuilder str = new StringBuilder("");
                    str.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    str.Append("<authorizationRequest><reference>req-" + o.cnpj + "</reference>");
                    str.Append("<permissions><code>CREATE_CHECKOUTS</code><code>RECEIVE_TRANSACTION_NOTIFICATIONS</code><code>SEARCH_TRANSACTIONS</code><code>MANAGE_PAYMENT_PRE_APPROVALS</code><code>DIRECT_PAYMENT</code></permissions>");
                    str.Append("<redirectURL>https://localhost:44382/Pages/oficina_CadastroCont.aspx</redirectURL><notificationURL>https://localhost:44382/Pages/oficina_CadastroCont.aspx</notificationURL></authorizationRequest>");

                    var content = new StringContent(str.ToString(), Encoding.UTF8, "text/xml");
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/xml;charset=ISO-8859-1");

                    var response = await client.PostAsync("https://ws.sandbox.pagseguro.uol.com.br/v2/authorizations/request/?appId=" + cred.appId + "&appKey=" + cred.appKey, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        string code = xml.GetElementsByTagName("code")[0].InnerXml;

                        Response.Redirect("https://sandbox.pagseguro.uol.com.br/v2/authorization/request.jhtml?code=" + code);
                    }
                    else
                    {
                        removeOficina(o);

                        string erro = xml.GetElementsByTagName("message")[0].InnerXml;
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Error: " + erro + " Por favor entre em contato com o suporte.";
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }

        protected async void redirectAuth(Oficina o, string code)
        {
        }

        protected void removeOficina(Oficina o)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Funcionario f = context.Funcionario.Where(func => func.cnpjOficina.Equals(o.cnpj)).FirstOrDefault();
                context.Funcionario.Remove(f);
                context.SaveChanges();

                List<DiaFuncionamento> dias = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(o.cnpj)).ToList();
                foreach (DiaFuncionamento dia in dias)
                {
                    context.DiaFuncionamento.Remove(dia);
                }
                context.SaveChanges();

                Endereco e = context.Endereco.Where(end => end.cnpjOficina.Equals(o.cnpj)).FirstOrDefault();
                context.Endereco.Remove(e);
                context.SaveChanges();

                o = context.Oficina.Where(of => of.cnpj.Equals(o.cnpj)).FirstOrDefault();
                context.Oficina.Remove(o);
                context.SaveChanges();
            }
        }
    }
}