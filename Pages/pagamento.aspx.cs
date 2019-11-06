using FixFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
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

        protected async Task<string> reqSplit(string token)
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
                    { "item[1].amount", o.valor.ToString() },
                    { "item[1].quantity", "1" },
                    { "notificationURL", "https://yourstore.com.br/notification" },
                    { "reference", "ORC" + o.idOrcamento.ToString() },
                    { "sender.name", c.nome },
                    { "sender.CPF", c.cpf },
                    { "sender.areaCode", c.telefone.Substring(0, 2)  },
                    { "sender.phone", c.telefone.Substring(2, c.telefone.Length - 2) },
                    { "sender.email", c.email },
                    { "shipping.address.street", "-"  },
                    { "shipping.address.number", "-"  },
                    { "shipping.address.complement", "-"  },
                    { "shipping.address.district", "-" },
                    { "shipping.address.postalCode", "-"  },
                    { "shipping.address.city", "-" },
                    { "shipping.address.state", "-"  },
                    { "shipping.address.country", "-"  },
                    { "shipping.type", "3"  },
                    { "shipping.cost", "0.00"  },
                    { "installment.value", o.valor.ToString() },
                    { "installment.quantity", "1" },
                    { "installment.noInterestInstallmentQuantity", "2"  },
                    { "creditCard.token", "61417e7e2617431f88aed64f833d6749"  },
                    { "creditCard.holder.name", "Customer Name"  },
                    { "creditCard.holder.CPF", "22111944785"  },
                    { "creditCard.holder.birthDate", "27/10/1987"  },
                    { "creditCard.holder.areaCode", "11"  },
                    { "creditCard.holder.phone", "56273440"  },
                    { "billingAddress.street", "-"  },
                    { "billingAddress.number", "-"  },
                    { "billingAddress.complement", "-"  },
                    { "billingAddress.district", "-" },
                    { "billingAddress.postalCode", "-"  },
                    { "billingAddress.city", "-"  },
                    { "billingAddress.state", "-"  },
                    { "billingAddress.country", "-"  },
                    { "primaryReceiver.publicKey", oficina.chavePublica }
                };
                var content = new FormUrlEncodedContent(values);
                content.Headers.Add("Accept", "application/vnd.pagseguro.com.br.v3+xml");
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

                var response = await client.PostAsync("https://ws.sandbox.pagseguro.uol.com.br/transactions?appId=" + cred.appId + "&appKey=" + cred.appKey, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseString);

                    return xml.GetElementsByTagName("reference")[0].InnerXml;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(responseString);

                    return xml.GetElementsByTagName("message")[0].InnerXml;
                }
            }
        }
    }
}