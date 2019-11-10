using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FixFinder.Pages
{
    public partial class oficina_CadastroCont : System.Web.UI.Page
    {
        private Cliente c;
        private HttpClient client;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            o = (Oficina)Session["oficina"];

            if (c == null || o == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                if (client == null)
                    client = new HttpClient();

                if (!IsPostBack)
                {
                    Response.AppendHeader("Access-Control-Allow-Origin", "https://sandbox.pagseguro.uol.com.br");
                    string code = Request["notificationCode"];
                    if (code == null)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Erro na autorização. Tente novamente mais tarde.<br /> (Para tentar novamente acesse Dashboard=>Configurações)";
                    }
                    else
                    {
                        getNotification(code);
                    }

                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        List<Cartao> cartoes = context.Cartao.Where(card => card.cpfCliente.Equals(c.cpf)).ToList();
                        if (cartoes.Count > 0)
                        {
                            ListItem item;
                            string mes;
                            foreach (Cartao card in cartoes)
                            {
                                if (card.mesVencimento < 10)
                                    mes = "0" + card.mesVencimento;
                                else
                                    mes = card.mesVencimento.ToString();

                                item = new ListItem();
                                item.Value = card.idCartao.ToString();
                                item.Text = card.bandeira.ToUpper() + " - " + card.numero.Substring(card.numero.Length - 4) + " - Vence em " + mes + "/" + card.anoVencimento;
                                txt_Cartao.Items.Add(item);
                            }
                        }
                        else
                        {
                            div_Quick.Visible = false;
                        }
                    }
                }
            }
        }

        protected async void getNotification(string code)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    HttpClient client = new HttpClient();
                    CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();

                    var response = await client.GetAsync("https://ws.sandbox.pagseguro.uol.com.br/v2/authorizations/notifications/" + code + "?appId=" + cred.appId + "&appKey=" + cred.appKey);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(responseString);

                        string cnpj = xml.GetElementsByTagName("reference")[0].InnerXml.Split('-')[1];
                        Oficina o = context.Oficina.Where(of => of.cnpj.Equals(cnpj)).FirstOrDefault();
                        o.chavePublica = xml.GetElementsByTagName("publicKey")[0].InnerXml;
                        context.SaveChanges();

                        pnl_Alert.CssClass = "alert alert-success";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Oficina autorizada. Agora você pode receber pagamentos de clientes";
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Erro na autorização. Tente novamente mais tarde.<br /> (Para tentar novamente acesse Dashboard=>Configurações)";
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

        protected void txt_Cartao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(txt_Cartao.SelectedValue);
                    Cartao card = context.Cartao.Where(cartao => cartao.idCartao == id).FirstOrDefault();
                    txt_NumeroCartao.Text = card.numero;
                    txt_Titular.Text = card.titular;

                    String mes;
                    if (card.mesVencimento < 10)
                        mes = "0" + card.mesVencimento;
                    else
                        mes = card.mesVencimento.ToString();
                    txt_Vencimento.Text = mes + "/" + card.anoVencimento;
                    div_Check.Visible = false;
                }
            }
            catch (Exception ex)
            {
                showError("Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte");
            }
        }

        protected async void btn_Pagar_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    string idSessao = await reqSessao();

                    if (idSessao.Length == 0)
                    {
                        showError("Erro na adesão." + Environment.NewLine + "Por favor entre em contato com o suporte");
                    }
                    else
                    {
                        string bandeira = await reqBandeira(idSessao);
                        if (bandeira.Length == 0)
                        {
                            showError("Erro na adesão. Verifique o número do cartão inserido");
                        }
                        else
                        {
                            int mes = int.Parse(txt_Vencimento.Text.Split('/')[0]);
                            int ano = int.Parse(txt_Vencimento.Text.Split('/')[1]);
                            if (ano < DateTime.Now.Year || (mes < DateTime.Now.Month && ano == DateTime.Now.Year))
                            {
                                showError("Erro na adesão. Verifique a data de vencimento inserida");
                            }
                            else
                            {
                                Cartao card = new Cartao()
                                {
                                    numero = txt_NumeroCartao.Text.Replace(" ", ""),
                                    bandeira = bandeira,
                                    mesVencimento = mes,
                                    anoVencimento = ano,
                                    titular = txt_Titular.Text,
                                    cpfCliente = c.cpf
                                };

                                string token = await reqToken(idSessao, card);
                                if (token.Length == 0)
                                {
                                    showError("Erro na adesão. Verifique os dados inseridos");
                                }
                                else
                                {
                                    if (chk_Salvar.Checked)
                                    {
                                        context.Cartao.Add(card);
                                        context.SaveChanges();
                                    }

                                    string codeAss = await reqAdesao(token);
                                    if (codeAss.Length == 0)
                                    {
                                        showError(/*"Erro na adesão. Por favor entre em contato com o suporte e/ou verifique os dados inseridos"*/codeAss);
                                    }
                                    else
                                    {
                                        showError(codeAss);
                                        //Oficina oficina = context.Oficina.Where(of => of.cnpj.Equals(o.cnpj)).FirstOrDefault();
                                        //oficina.codAssinatura = codeAss;
                                        //context.SaveChanges();

                                        //Session["oficina"] = null;
                                        //Response.Redirect("home.aspx", false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showError("Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte e/ou verifique os dados inseridos");
            }
        }

        protected async Task<string> reqSessao()
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();
                var values = new Dictionary<string, string>{
                    { "email", cred.email },
                    { "token", cred.token }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://ws.sandbox.pagseguro.uol.com.br/sessions?appId=" + cred.appId + "&appKey=" + cred.appKey, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseString);

                    return xml.GetElementsByTagName("id")[0].InnerXml;
                }
                else
                {
                    return "";
                }
            }
        }

        protected async Task<string> reqBandeira(string idSessao)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                var response = await client.GetAsync("https://df.uol.com.br//df-fe/mvc/creditcard/v1/getBin?tk=" + idSessao + "&creditCard=" + txt_NumeroCartao.Text.Substring(0, 7).Replace(" ", ""));
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && !responseString.Split('\"')[23].Equals("Error"))
                {
                    CartaoResposta resposta = JsonConvert.DeserializeObject<CartaoResposta>(responseString);

                    return resposta.bin.brand.name;
                }
                else
                {
                    return "";
                }
            }
        }

        protected async Task<string> reqToken(string idSessao, Cartao card)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();
                var values = new Dictionary<string, string>{
                    { "sessionId", idSessao },
                    { "amount", "49.90" },
                    { "cardNumber", card.numero },
                    { "cardBrand", card.bandeira },
                    { "cardCvv", txt_Cvv.Text },
                    { "cardExpirationMonth", card.mesVencimento.ToString() },
                    { "cardExpirationYear", card.anoVencimento.ToString() }
                };
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://df.uol.com.br/v2/cards/?email=" + cred.email + "&token=" + cred.token, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseString);

                    return xml.GetElementsByTagName("token")[0].InnerXml;
                }
                else
                {
                    return "";
                }
            }
        }

        protected async Task<string> reqAdesao(string token)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();

                    StringBuilder str = new StringBuilder("");
                    str.Append("{\"plan\": \"673503779E9E96CBB4DBEF9646FF65CB\",\"reference\": \"Teste\",");
                    str.Append("\"sender\": {\"name\": \"Vladimir Integracao\",\"email\": \"vladimirintegracao@sandbox.pagseguro.com.br\",\"hash\": \"werwerwerwerwer\",\"phone\": {\"areaCode\": \"11\",\"number\": \"20516250\"},");
                    str.Append("\"address\": {\"street\": \"Rua Vi Jose De Castro\",\"number\": \"99\",\"complement\": \"\",\"district\": \"It\",\"city\": \"Sao Paulo\",\"state\": \"SP\",\"country\": \"BRA\",\"postalCode\": \"06240300\"},\"documents\": [{\"type\": \"CPF\",\"value\": \"68951723003\"}]},");
                    str.Append("\"paymentMethod\": {\"type\": \"CREDITCARD\",\"creditCard\": {\"token\": \"b8eb02412a054772bbd7ab84aaacd72c\",\"holder\": {\"name\": \"Julian Teste\",\"birthDate\": \"04/12/1991\",\"documents\": [{\"type\": \"CPF\",\"value\": \"19333575090\"}],");
                    str.Append("\"phone\": {\"areaCode\": \"11\",\"number\": \"20516250\"},");
                    str.Append("\"billingAddress\": {\"street\": \"Rua Vi Jose De Castro\",\"number\": \"99\",\"complement\": \"\",\"district\": \"It\",\"city\": \"Sao Paulo\",\"state\": \"SP\",\"country\": \"BRA\",\"postalCode\": \"06240300\"}}}}}");

                    var content = new StringContent(str.ToString(), Encoding.UTF8, "application/json");
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/vnd.pagseguro.com.br.v1+json");
                    mediaType.CharSet = "ISO-8859-1";

                    client.DefaultRequestHeaders.Accept.Add(mediaType);

                    var response = await client.PostAsync("https://ws.sandbox.pagseguro.uol.com.br/pre-approvals?email=" + cred.email + "&token=" + cred.token, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return responseString;
                    }
                    else
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        return responseString;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void showError(string msg)
        {
            pnl_Alert.CssClass = "alert alert-danger";
            lbl_Alert.Text = msg;
            pnl_Alert.Visible = true;
        }
    }
}