using FixFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace FixFinder.Pages
{
    public partial class pagamento : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private Orcamento o;
        private HttpClient client;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            o = (Orcamento)Session["orcamento"];
            if (c == null)
                Response.Redirect("login.aspx", false);
            else if (o == null)
                Response.Redirect("orcamento_ListaCliente.aspx", false);
            else if (!o.status.Equals("Pagamento pendente"))
                Response.Redirect("orcamento_ListaCliente.aspx", false);
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Oficina oficina = context.Oficina.Where(of => of.cnpj.Equals(o.cnpjOficina)).FirstOrDefault();
                    lbl_Oficina.InnerText = oficina.nome;
                    if (oficina.reputacao == null)
                        lbl_Reputacao.Text = "-/10";
                    else
                        lbl_Reputacao.Text = oficina.reputacao + "/10";
                    lbl_Valor.InnerText = "Valor do pagamento: R$ " + o.valor.ToString("0.00");

                    if (client == null)
                        client = new HttpClient();

                    if (!IsPostBack)
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

                    //Dash Time
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    lbl_Nome.Text = c.nome;
                    if (f == null)
                    {
                        pnl_Oficina.Visible = false;
                        btn_CadastroOficina.Visible = true;
                        List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(r => r.cpfCliente.Equals(c.cpf)).ToList();
                        if (requisicoes.Count > 0)
                        {
                            pnl_Funcionario.Visible = true;
                            badge_Requisicoes.InnerHtml = requisicoes.Count.ToString();
                        }
                        else
                        {
                            pnl_Funcionario.Visible = false;
                        }
                    }
                    else
                    {
                        pnl_Oficina.Visible = true;
                        pnl_Funcionario.Visible = false;
                        btn_CadastroOficina.Visible = false;
                        lbl_Nome.Text += " | " + f.Oficina.nome;
                        if (f.cargo.ToLower().Equals("gerente"))
                        {
                            btn_Configuracoes.Visible = true;
                            btn_Funcionarios.Visible = true;
                        }
                        else
                        {
                            btn_Configuracoes.Visible = false;
                            btn_Funcionarios.Visible = false;
                        }
                    }
                }
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        protected void showError(string msg)
        {
            pnl_Alert.CssClass = "alert alert-danger";
            lbl_Alert.Text = msg;
            pnl_Alert.Visible = true;
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
                        showError("Erro no pagamento." + Environment.NewLine + "Por favor entre em contato com o suporte");
                    }
                    else
                    {
                        string bandeira = await reqBandeira(idSessao);
                        if (bandeira.Length == 0)
                        {
                            showError("Erro no pagamento. Verifique o número do cartão inserido");
                        }
                        else
                        {
                            int mes = int.Parse(txt_Vencimento.Text.Split('/')[0]);
                            int ano = int.Parse(txt_Vencimento.Text.Split('/')[1]);
                            if (ano < DateTime.Now.Year || (mes < DateTime.Now.Month && ano == DateTime.Now.Year))
                            {
                                showError("Erro no pagamento. Verifique a data de vencimento inserida");
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
                                    showError("Erro no pagamento. Verifique os dados inseridos");
                                }
                                else
                                {
                                    if (chk_Salvar.Checked)
                                    {
                                        context.Cartao.Add(card);
                                        context.SaveChanges();
                                    }

                                    XmlDocument transaction = await reqPagar(token);
                                    if (transaction == null)
                                    {
                                        showError("Erro no pagamento. Por favor entre em contato com o suporte e/ou verifique os dados inseridos");
                                    }
                                    else
                                    {
                                        Pagamento p = new Pagamento()
                                        {
                                            idOrcamento = o.idOrcamento,
                                            status = "1",
                                            data = DateTime.Parse(transaction.GetElementsByTagName("date")[0].InnerXml),
                                            valor = o.valor,
                                            code = transaction.GetElementsByTagName("code")[0].InnerXml
                                        };
                                        context.Pagamento.Add(p);
                                        o = context.Orcamento.Where(orc => orc.idOrcamento == o.idOrcamento).FirstOrDefault();
                                        o.status = "Pagamento em processamento";
                                        context.SaveChanges();

                                        Session["orcamento"] = null;
                                        Session["message"] = "Seu pagamento está agora em processamento";
                                        Response.Redirect("orcamento_ListaCliente.aspx", false);
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
                    { "amount", o.valor.ToString() },
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

        protected async Task<XmlDocument> reqPagar(string token)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Oficina oficina = context.Oficina.Where(of => of.cnpj.Equals(o.cnpjOficina)).FirstOrDefault();

                CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();
                var values = new Dictionary<string, string>{
                    { "payment.mode", "default" },
                    { "payment.method", "creditCard" },
                    { "currency", "BRL" },
                    { "item[1].id", o.idOrcamento.ToString() },
                    { "item[1].description", "Orçamento" },
                    { "item[1].amount", o.valor.ToString("0.00").Replace(",",".") },
                    { "item[1].quantity", "1" },
                    { "notificationURL", "https://localhost:44382/Pages/notification.aspx" },
                    { "reference", "ORC-" + o.idOrcamento.ToString() },
                    { "sender.name", c.nome },
                    { "sender.CPF", c.cpf },
                    { "sender.areaCode", c.telefone.Substring(0, 2)  },
                    { "sender.phone", c.telefone.Substring(2, c.telefone.Length - 2) },
                    { "sender.email", "c81915925526377422156@sandbox.pagseguro.com.br" },
                    { "shipping.address.street", "Av. Brig. Faria Lima"  },
                    { "shipping.address.number", "1384"  },
                    { "shipping.address.complement", "5o andar"  },
                    { "shipping.address.district", "Jardim Paulistano" },
                    { "shipping.address.postalCode", "01452002"  },
                    { "shipping.address.city", "Sao Paulo" },
                    { "shipping.address.state", "SP"  },
                    { "shipping.address.country", "BRA"  },
                    { "shipping.type", "3"  },
                    { "shipping.cost", "0.00"  },
                    { "installment.value", o.valor.ToString("0.00").Replace(",",".") },
                    { "installment.quantity", "1" },
                    { "installment.noInterestInstallmentQuantity", "2"  },
                    { "creditCard.token", token  },
                    { "creditCard.holder.name", c.nome  },
                    { "creditCard.holder.CPF", c.cpf  },
                    { "creditCard.holder.birthDate", c.dataNascimento.ToString().Split(' ')[0]  },
                    { "creditCard.holder.areaCode", c.telefone.Substring(0, 2)  },
                    { "creditCard.holder.phone", c.telefone.Substring(2, c.telefone.Length - 2)  },
                    { "billingAddress.street", "Av. Brig. Faria Lima"  },
                    { "billingAddress.number", "1384"  },
                    { "billingAddress.complement", "5o andar"  },
                    { "billingAddress.district", "Jardim Paulistano" },
                    { "billingAddress.postalCode", "01452002"  },
                    { "billingAddress.city", "Sao Paulo"  },
                    { "billingAddress.state", "SP"  },
                    { "billingAddress.country", "BRA"  },
                    { "primaryReceiver.publicKey", oficina.chavePublica }
                };
                var content = new FormUrlEncodedContent(values);
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/vnd.pagseguro.com.br.v3+xml"));

                var response = await client.PostAsync("https://ws.sandbox.pagseguro.uol.com.br/transactions?appId=" + cred.appId + "&appKey=" + cred.appKey, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseString);

                    return xml;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    return null;
                }
            }
        }
    }
}