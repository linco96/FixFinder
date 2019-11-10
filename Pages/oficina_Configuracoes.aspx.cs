using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace FixFinder.Pages
{
    public partial class oficina_Configuracoes : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;
        private Oficina oficina;
        private List<DiaFuncionamento> listaDias;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        funcionario = context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();
                        if (funcionario != null)
                        {
                            oficina = context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault();
                            if (oficina != null)
                            {
                                pnl_Alert.Visible = false;
                                pnl_AlerSalvar.Visible = false;
                                if (!IsPostBack)
                                    preencher_Campos();
                                lbl_Nome.Text = c.nome;

                                if (!IsPostBack)
                                {
                                    Response.AppendHeader("Access-Control-Allow-Origin", "https://sandbox.pagseguro.uol.com.br");
                                    string code = Request["notificationCode"];
                                    if (code != null)
                                    {
                                        getNotification(code);
                                    }
                                }

                                if (oficina.chavePublica == null)
                                {
                                    div_Auth.Visible = true;
                                }
                                else
                                {
                                    div_Auth.Visible = false;
                                }

                                //if (oficina.statusAssinatura == 1)
                                //{
                                //    div_CancelarAss.Visible = true;
                                //    div_NoAss.Visible = false;
                                //}
                                //else
                                //{
                                //    div_CancelarAss.Visible = false;
                                //    div_NoAss.Visible = true;
                                //}

                                if (funcionario == null)
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
                                    lbl_Nome.Text += " | " + funcionario.Oficina.nome;
                                    if (funcionario.cargo.ToLower().Equals("gerente"))
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
                            else
                            {
                                Response.Redirect("home.aspx", false);
                            }
                        }
                        else
                        {
                            Response.Redirect("home.aspx", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void preencher_Campos()
        {
            Endereco endereco;
            FotoOficina foto;
            try
            {
                using (var context = new DatabaseEntities())
                {
                    txt_OficinaCNPJ.Text = oficina.cnpj;
                    txt_OficinaNome.Text = oficina.nome;

                    endereco = context.Endereco.Where(e => e.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();

                    if (endereco != null)
                    {
                        txt_Rua.Text = endereco.logradouro;
                        txt_Numero.Text = endereco.numero.ToString();
                        if (endereco.complemento != null)
                            txt_Complemento.Text = endereco.complemento;
                        txt_CEP.Text = endereco.cep;
                        txt_Cidade.Text = endereco.cidade;
                        txt_UF.Text = endereco.uf;
                        txt_Bairro.Text = endereco.bairro;
                    }

                    txt_Telefone.Text = oficina.telefone;
                    txt_Email.Text = oficina.email;

                    if (oficina.descricao != null)
                        txt_Descrição.Text = oficina.descricao;

                    //HORA DE FUNCIONAMENTO
                    txt_HorarioAberturaUtil.Text = "";
                    txt_HorarioFechamentoUtil.Text = "";
                    txt_HorarioAberturaSabado.Text = "";
                    txt_HorarioFechamentoSabado.Text = "";
                    txt_HorarioAberturaDomingo.Text = "";
                    txt_HorarioFechamentoDomingo.Text = "";
                    listaDias = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj)).ToList();
                    foreach (DiaFuncionamento dia in listaDias)
                    {
                        switch (dia.diaSemana)
                        {
                            case "util":
                                txt_HorarioAberturaUtil.Text = dia.horaAbertura.Hours.ToString("00") + ":" + dia.horaAbertura.Minutes.ToString("00");
                                txt_HorarioFechamentoUtil.Text = dia.horaFechamento.Hours.ToString("00") + ":" + dia.horaFechamento.Minutes.ToString("00");
                                break;

                            case "sabado":
                                txt_HorarioAberturaSabado.Text = dia.horaAbertura.Hours.ToString("00") + ":" + dia.horaAbertura.Minutes.ToString("00");
                                txt_HorarioFechamentoSabado.Text = dia.horaFechamento.Hours.ToString("00") + ":" + dia.horaFechamento.Minutes.ToString("00");
                                break;

                            case "domingo":
                                txt_HorarioAberturaDomingo.Text = dia.horaAbertura.Hours.ToString("00") + ":" + dia.horaAbertura.Minutes.ToString("00");
                                txt_HorarioFechamentoDomingo.Text = dia.horaFechamento.Hours.ToString("00") + ":" + dia.horaFechamento.Minutes.ToString("00");
                                break;
                        }
                    }

                    txt_DuracaoAtendimento.Text = oficina.duracaoAtendimento.Hours.ToString("00") + ":" + oficina.duracaoAtendimento.Minutes.ToString("00");
                    txt_CapacidadeAtendimento.Text = oficina.capacidadeAgendamentos.ToString();

                    //Foto
                    foto = context.FotoOficina.Where(f => f.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();
                    if (foto != null)
                    {
                        String base64 = Convert.ToBase64String(foto.foto);
                        img_Oficina.ImageUrl = "data:Image/png;base64," + base64;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btn_CarregarImagem_Click(object sender, EventArgs e)
        {
            try
            {
                if (oficina != null)
                {
                    HttpPostedFile arquivo = fileUpload.PostedFile;
                    String arquivoNome = Path.GetFileName(arquivo.FileName);
                    String arquivoExtensao = Path.GetExtension(arquivoNome);
                    int arquivoTamanho = arquivo.ContentLength;

                    if (arquivoTamanho > 0)
                    {
                        if (arquivoExtensao.ToLower() == ".jpg" || arquivoExtensao.ToLower() == ".png" || arquivoExtensao.ToLower() == ".jpeg")
                        {
                            if (arquivoTamanho <= 5000000)
                            {
                                using (var context = new DatabaseEntities())
                                {
                                    FotoOficina foto = context.FotoOficina.Where(f => f.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();

                                    Stream stream = arquivo.InputStream;
                                    BinaryReader bReader = new BinaryReader(stream);

                                    if (foto == null)
                                    {
                                        foto = new FotoOficina()
                                        {
                                            cnpjOficina = oficina.cnpj,
                                            nomeFoto = "fotoOficina",
                                            foto = bReader.ReadBytes((int)stream.Length)
                                        };
                                        context.FotoOficina.Add(foto);
                                    }
                                    else
                                    {
                                        foto.foto = bReader.ReadBytes((int)stream.Length);
                                    }
                                    context.SaveChanges();
                                    String base64 = Convert.ToBase64String(foto.foto);
                                    img_Oficina.ImageUrl = "data:Image/png;base64," + base64;
                                }
                            }
                            else
                            {
                                pnl_Alert.Visible = true;
                                lbl_Alert.Text = "O arquivo não pode ser maior que 5MB";
                            }
                        }
                    }
                    else
                    {
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Apenas imagens (.png, . jpg e .jpeg) podem ser enviadas";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Erro no BD, Oficina nao encontrada no BD');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btn_Salvar_Click(object sender, EventArgs e)
        {
            if (oficina != null)
            {
                try
                {
                    int qtdAgendamento = 0;
                    int numeroEndereco = 0;
                    int lenCNPJ = txt_OficinaCNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", "").Length;
                    int lenCEP = txt_CEP.Text.Replace("-", "").Length;
                    qtdAgendamento = int.Parse(txt_CapacidadeAtendimento.Text);
                    numeroEndereco = int.Parse(txt_Numero.Text);
                    if ((txt_HorarioAberturaUtil.Text.Length == 0 || txt_HorarioFechamentoUtil.Text.Length == 0) && (txt_HorarioAberturaSabado.Text.Length == 0 || txt_HorarioFechamentoSabado.Text.Length == 0) && (txt_HorarioAberturaDomingo.Text.Length == 0 || txt_HorarioFechamentoDomingo.Text.Length == 0))
                    {
                        pnl_AlerSalvar.CssClass = "alert alert-danger";
                        pnl_AlerSalvar.Visible = true;
                        lblAlertSalvar.Text = "Informe os horários de abertura e fechamento de pelo menos um dia das semana";
                    }
                    else if (qtdAgendamento <= 0)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "A capacidade de agendamentos precisa ser maior que 0";
                    }
                    else if (numeroEndereco <= 0)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "O número do endereço precisa ser maior que 0";
                    }
                    else if (lenCNPJ != 14)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Favor preencher o CNPJ corretamente";
                    }
                    else if (lenCEP != 8)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Favor preencher o CEP corretamente";
                    }
                    else
                    {
                        using (var context = new DatabaseEntities())
                        {
                            Endereco endereco = context.Endereco.Where(end => end.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();

                            if (endereco != null)
                            {
                                endereco.cep = txt_CEP.Text.Replace("-", "");
                                endereco.cidade = txt_Cidade.Text;
                                endereco.cnpjOficina = oficina.cnpj;
                                endereco.complemento = txt_Complemento.Text;
                                endereco.logradouro = txt_Rua.Text;
                                endereco.numero = int.Parse(txt_Numero.Text);
                                endereco.uf = txt_UF.Text;
                                context.SaveChanges();
                                pnl_AlerSalvar.Visible = true;
                            }
                            else
                            {
                                pnl_AlerSalvar.Visible = true;
                                pnl_AlerSalvar.CssClass = "alert-danger";
                                lblAlertSalvar.Text = "O endereco nao foi localizado no banco de dados";
                            }

                            oficina = context.Oficina.Where(o => o.cnpj.Equals(oficina.cnpj)).FirstOrDefault();

                            if (oficina != null)
                            {
                                oficina.nome = txt_OficinaNome.Text;
                                oficina.telefone = txt_Telefone.Text.Replace("-", "").Replace("(", "").Replace(")", "");
                                oficina.email = txt_Email.Text;

                                oficina.descricao = txt_Descrição.Text;
                                oficina.capacidadeAgendamentos = int.Parse(txt_CapacidadeAtendimento.Text);
                                oficina.duracaoAtendimento = TimeSpan.Parse(txt_DuracaoAtendimento.Text);
                                context.SaveChanges();

                                //Alterar data e dia de funcionamento
                                DiaFuncionamento dia;

                                if (txt_HorarioAberturaUtil.Text.Length > 0 && txt_HorarioFechamentoUtil.Text.Length > 0)
                                {
                                    dia = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj) && d.diaSemana.Equals("util")).FirstOrDefault();
                                    if (dia != null)
                                    {
                                        dia.horaAbertura = TimeSpan.Parse(txt_HorarioAberturaUtil.Text);
                                        dia.horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoUtil.Text);
                                    }
                                    else
                                    {
                                        dia = new DiaFuncionamento()
                                        {
                                            diaSemana = "util",
                                            cnpjOficina = oficina.cnpj,
                                            horaAbertura = TimeSpan.Parse(txt_HorarioAberturaUtil.Text),
                                            horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoUtil.Text)
                                        };
                                    }
                                    context.DiaFuncionamento.Add(dia);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    dia = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj) && d.diaSemana.Equals("util")).FirstOrDefault();
                                    if (dia != null)
                                    {
                                        context.DiaFuncionamento.Remove(dia);
                                        context.SaveChanges();
                                    }
                                }

                                if (txt_HorarioAberturaSabado.Text.Length > 0 && txt_HorarioFechamentoSabado.Text.Length > 0)
                                {
                                    dia = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj) && d.diaSemana.Equals("sabado")).FirstOrDefault();
                                    if (dia != null)
                                    {
                                        dia.horaAbertura = TimeSpan.Parse(txt_HorarioAberturaSabado.Text);
                                        dia.horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoSabado.Text);
                                    }
                                    else
                                    {
                                        dia = new DiaFuncionamento()
                                        {
                                            diaSemana = "sabado",
                                            cnpjOficina = oficina.cnpj,
                                            horaAbertura = TimeSpan.Parse(txt_HorarioAberturaSabado.Text),
                                            horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoSabado.Text)
                                        };
                                    }
                                    context.DiaFuncionamento.Add(dia);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    dia = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj) && d.diaSemana.Equals("sabado")).FirstOrDefault();
                                    if (dia != null)
                                    {
                                        context.DiaFuncionamento.Remove(dia);
                                        context.SaveChanges();
                                    }
                                }

                                if (txt_HorarioAberturaDomingo.Text.Length > 0 && txt_HorarioFechamentoDomingo.Text.Length > 0)
                                {
                                    dia = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj) && d.diaSemana.Equals("domingo")).FirstOrDefault();
                                    if (dia != null)
                                    {
                                        dia.horaAbertura = TimeSpan.Parse(txt_HorarioAberturaDomingo.Text);
                                        dia.horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoDomingo.Text);
                                    }
                                    else
                                    {
                                        dia = new DiaFuncionamento()
                                        {
                                            diaSemana = "domingo",
                                            cnpjOficina = oficina.cnpj,
                                            horaAbertura = TimeSpan.Parse(txt_HorarioAberturaDomingo.Text),
                                            horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoDomingo.Text)
                                        };
                                    }
                                    context.DiaFuncionamento.Add(dia);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    dia = context.DiaFuncionamento.Where(d => d.cnpjOficina.Equals(oficina.cnpj) && d.diaSemana.Equals("domingo")).FirstOrDefault();
                                    if (dia != null)
                                    {
                                        context.DiaFuncionamento.Remove(dia);
                                        context.SaveChanges();
                                    }
                                }

                                pnl_AlerSalvar.Visible = true;
                            }
                            else
                            {
                                pnl_AlerSalvar.Visible = true;
                                pnl_AlerSalvar.CssClass = "alert-danger";
                                lblAlertSalvar.Text = "A oficina nao foi localizada no banco de dados";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
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

        protected void btn_Autorizar_Click(object sender, EventArgs e)
        {
            autorizar(oficina);
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
                    str.Append("<redirectURL>https://localhost:44382/Pages/oficina_Configuracoes.aspx</redirectURL><notificationURL>https://localhost:44382/Pages/oficina_Configuracoes.aspx</notificationURL></authorizationRequest>");

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

                        Response.Redirect("oficina_Configuracoes.aspx", false);
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                        lbl_Alert.Text = "Erro na autorização. Tente novamente mais tarde";
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

        protected async void btn_CancelarAssinatura_Click(object sender, EventArgs e)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                CredenciaisPagamento cred = context.CredenciaisPagamento.FirstOrDefault();
                HttpClient client = new HttpClient();
                MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/vnd.pagseguro.com.br.v3+json");
                mediaType.CharSet = "ISO-8859-1";
                client.DefaultRequestHeaders.Accept.Add(mediaType);

                var content = new StringContent("{\"status\":\"SUSPENDED\"}", Encoding.UTF8, "application/json");

                var response = await client.PutAsync("https://ws.sandbox.pagseguro.uol.com.br/pre-approvals/" + oficina.codAssinatura + "/status?token=" + cred.token + "&email=" + cred.email, null);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    oficina.statusAssinatura = 0;
                    oficina.codAssinatura = null;
                    pnl_Alert.CssClass = "alert alert-success";
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "Assinatura suspensa";
                }
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "Erro no cancelamento";
                }
            }
        }

        protected void btn_NoAss_Click(object sender, EventArgs e)
        {
        }
    }
}