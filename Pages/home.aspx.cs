using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FixFinder.Models;
using Newtonsoft.Json;

namespace FixFinder.Pages
{
    public partial class home : System.Web.UI.Page
    {
        private Funcionario f;
        private Cliente c;
        private static List<KeyValuePair<Oficina, Element>> resultadosAtuais;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    pnl_Alert.Visible = false;
                    div_Resultados.Controls.Clear();
                    if (resultadosAtuais != null)
                    {
                        Panel card;
                        Panel row;
                        Panel container;
                        Panel section;

                        Image img;
                        HtmlGenericControl title;
                        Label lbl1;
                        Label lbl2;
                        HtmlGenericControl biggie;
                        Button btn;
                        int cont = 0;

                        FotoOficina picture;
                        foreach (KeyValuePair<Oficina, Element> res in resultadosAtuais)
                        {
                            if (Double.Parse(res.Value.distance.text.Replace(".", ",").Split(' ')[0]) <= 10)
                            {
                                cont++;
                                card = new Panel();
                                card.CssClass = "card mb-3";

                                row = new Panel();
                                row.CssClass = "row no-gutters";

                                //IMAGE
                                container = new Panel();
                                container.CssClass = "col-md-3 border-right text-center";

                                img = new Image();
                                img.Style.Add("width", "100%");
                                img.Style.Add("height", "100%");
                                img.Style.Add("object-fit", "contain");
                                img.CssClass = "Responsive image";
                                picture = context.FotoOficina.Where(pic => pic.cnpjOficina.Equals(res.Key.cnpj)).FirstOrDefault();
                                if (picture != null)
                                {
                                    String base64 = Convert.ToBase64String(picture.foto);
                                    img.ImageUrl = "data:Image/png;base64," + base64;
                                    img.CssClass = "Responsive image bg-dark";
                                }
                                else
                                {
                                    img.ImageUrl = "~/Content/no-image.png";
                                }
                                container.Controls.Add(img);
                                row.Controls.Add(container);

                                //INFO
                                container = new Panel();
                                container.CssClass = "col-md-9";

                                //HEAD
                                section = new Panel();
                                section.CssClass = "card-header bg-light p-1";

                                //NOME
                                title = new HtmlGenericControl("h5");
                                title.Attributes.Add("class", "card-title mt-2 ml-3");

                                lbl1 = new Label();
                                lbl1.CssClass = "align-middle";
                                lbl1.Text = res.Key.nome;
                                title.Controls.Add(lbl1);

                                //NOTA
                                lbl1 = new Label();
                                lbl1.CssClass = "float-right";

                                lbl2 = new Label();
                                lbl2.CssClass = "align-middle";
                                if (res.Key.reputacao == null)
                                    lbl2.Text = "-/10 ";
                                else
                                    lbl2.Text = res.Key.reputacao.ToString().Replace(",", ".") + "/10 ";
                                lbl1.Controls.Add(lbl2);

                                img = new Image();
                                img.CssClass = "align-middle";
                                img.ImageUrl = "../Content/star_24.png";
                                lbl1.Controls.Add(img);
                                title.Controls.Add(lbl1);
                                title.Controls.Add(new LiteralControl("<br/>"));

                                //DISTANCIA
                                biggie = new HtmlGenericControl("small");
                                biggie.Attributes.Add("class", "card-title text-muted");
                                biggie.InnerHtml = res.Value.distance.text + " de distância";
                                title.Controls.Add(biggie);

                                section.Controls.Add(title);
                                container.Controls.Add(section);

                                //BODY
                                section = new Panel();
                                section.CssClass = "card-body";

                                //ENDERECO
                                lbl1 = new Label();
                                lbl1.CssClass = "card-text h5 mb-5";
                                lbl1.Text = res.Key.Endereco.logradouro + ", " + res.Key.Endereco.numero.ToString() + " - " + res.Key.Endereco.bairro + ", " + res.Key.Endereco.cidade + " - " + res.Key.Endereco.uf.ToUpper() + ", " + res.Key.Endereco.cep + "<br />";
                                section.Controls.Add(lbl1);

                                //DESCRICAO
                                if (res.Key.descricao != null)
                                {
                                    lbl1 = new Label();
                                    lbl1.CssClass = "card-text font-italic text-muted";
                                    lbl1.Text = "<br/>\"" + res.Key.descricao + "\"<br />";
                                    section.Controls.Add(lbl1);
                                }

                                //BIUTON
                                btn = new Button();
                                btn.CssClass = "btn btn-success mt-4";
                                btn.Text = "Solicitar agendamento";
                                btn.Click += new EventHandler(btn_SolicitarAgendamento_Click);
                                btn.CommandArgument = res.Key.cnpj;
                                btn.ID = "btn_SolicitarAgendamento" + res.Key.cnpj;
                                section.Controls.Add(btn);
                                container.Controls.Add(section);
                                row.Controls.Add(container);

                                card.Controls.Add(row);
                                div_Resultados.Controls.Add(card);
                                pnl_Alert.Visible = false;
                            }

                            if (cont == 0)
                            {
                                div_Resultados.Controls.Clear();
                                pnl_Alert.CssClass = "alert alert-danger mt-3";
                                lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                                pnl_Alert.Visible = true;
                            }
                        }
                        if (div_Resultados.Controls.Count == 0)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                            pnl_Alert.Visible = true;
                        }
                    }
                    //Codigo dashboard
                    Funcionario f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
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

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //bbk
        }

        private async void recomendar_Oficinas()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    //CRIA DESTINOS
                    List<Oficina> oficinas = context.Oficina.ToList();
                    StringBuilder destinos = new StringBuilder();
                    Endereco endereco;

                    if (oficinas.Count > 0)
                    {
                        for (int i = 0; i < oficinas.Count; i++)
                        {
                            endereco = oficinas[i].Endereco;
                            destinos.Append(endereco.numero.ToString());
                            destinos.Append("+");
                            destinos.Append(endereco.logradouro.Trim().Replace(" ", "+"));
                            destinos.Append("+");
                            destinos.Append(endereco.cidade.Trim().Replace(" ", "+"));
                            destinos.Append("+");
                            destinos.Append(endereco.uf.ToUpper());
                            if (i < oficinas.Count - 1)
                            {
                                destinos.Append("|");
                            }
                        }
                        //Realiza a pesquisa
                        String chave = context.Key.FirstOrDefault().idChave;
                        SearchResult search;
                        HttpClient cliente = new HttpClient();
                        var response = await cliente.GetAsync("https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=" + txt_LatLon.Text + "&destinations=" + destinos.ToString() + "&key=" + chave);
                        var responseString = await response.Content.ReadAsStringAsync();
                        search = JsonConvert.DeserializeObject<SearchResult>(responseString);
                        if (search.status.Equals("OK"))
                        {
                            preencher_Recomendacoes(search, oficinas);
                        }
                        else
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                            pnl_Alert.Visible = true;
                        }
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger mt-3";
                        lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                        pnl_Alert.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger mt-3";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        private async void preencher_Recomendacoes(SearchResult search, List<Oficina> oficinas)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    List<KeyValuePair<Oficina, Element>> resultados = new List<KeyValuePair<Oficina, Element>>();

                    for (int i = 0; i < oficinas.Count; i++)
                    {
                        if (search.rows[0].elements[i].status.Equals("OK"))
                            resultados.Add(new KeyValuePair<Oficina, Element>(oficinas[i], search.rows[0].elements[i]));
                    }
                    if (resultados.Count > 0)
                    {
                        resultados = resultados.OrderByDescending(p => p.Key.reputacao).ToList();
                        resultadosAtuais = resultados;
                        Panel card;
                        Panel row;
                        Panel container;
                        Panel section;

                        Image img;
                        HtmlGenericControl title;
                        Label lbl1;
                        Label lbl2;
                        HtmlGenericControl biggie;
                        Button btn;
                        int cont = 0;
                        FotoOficina picture;
                        foreach (KeyValuePair<Oficina, Element> res in resultados)
                        {
                            if (Double.Parse(res.Value.distance.text.Replace(".", ",").Split(' ')[0]) <= 10)
                            {
                                cont++;
                                card = new Panel();
                                card.CssClass = "card mb-3";

                                row = new Panel();
                                row.CssClass = "row no-gutters";

                                //IMAGE
                                container = new Panel();
                                container.CssClass = "col-md-3 border-right text-center";

                                img = new Image();
                                img.Style.Add("width", "100%");
                                img.Style.Add("height", "100%");
                                img.Style.Add("object-fit", "contain");
                                img.CssClass = "Responsive image";
                                picture = context.FotoOficina.Where(pic => pic.cnpjOficina.Equals(res.Key.cnpj)).FirstOrDefault();
                                if (picture != null)
                                {
                                    String base64 = Convert.ToBase64String(picture.foto);
                                    img.ImageUrl = "data:Image/png;base64," + base64;
                                    img.CssClass = "Responsive image bg-dark";
                                }
                                else
                                {
                                    img.ImageUrl = "~/Content/no-image.png";
                                }
                                container.Controls.Add(img);
                                row.Controls.Add(container);

                                //INFO
                                container = new Panel();
                                container.CssClass = "col-md-9";

                                //HEAD
                                section = new Panel();
                                section.CssClass = "card-header bg-light p-1";

                                //NOME
                                title = new HtmlGenericControl("h5");
                                title.Attributes.Add("class", "card-title mt-2 ml-3");

                                lbl1 = new Label();
                                lbl1.CssClass = "align-middle";
                                lbl1.Text = res.Key.nome;
                                title.Controls.Add(lbl1);

                                //NOTA
                                lbl1 = new Label();
                                lbl1.CssClass = "float-right";

                                lbl2 = new Label();
                                lbl2.CssClass = "align-middle";
                                if (res.Key.reputacao == null)
                                    lbl2.Text = "-/10 ";
                                else
                                    lbl2.Text = res.Key.reputacao.ToString().Replace(",", ".") + "/10 ";
                                lbl1.Controls.Add(lbl2);

                                img = new Image();
                                img.CssClass = "align-middle";
                                img.ImageUrl = "../Content/star_24.png";
                                lbl1.Controls.Add(img);
                                title.Controls.Add(lbl1);
                                title.Controls.Add(new LiteralControl("<br/>"));

                                //DISTANCIA
                                biggie = new HtmlGenericControl("small");
                                biggie.Attributes.Add("class", "card-title text-muted");
                                biggie.InnerHtml = res.Value.distance.text + " de distância";
                                title.Controls.Add(biggie);

                                section.Controls.Add(title);
                                container.Controls.Add(section);

                                //BODY
                                section = new Panel();
                                section.CssClass = "card-body";

                                //ENDERECO
                                lbl1 = new Label();
                                lbl1.CssClass = "card-text h5 mb-5";
                                lbl1.Text = res.Key.Endereco.logradouro + ", " + res.Key.Endereco.numero.ToString() + " - " + res.Key.Endereco.bairro + ", " + res.Key.Endereco.cidade + " - " + res.Key.Endereco.uf.ToUpper() + ", " + res.Key.Endereco.cep + "<br />";
                                section.Controls.Add(lbl1);

                                //DESCRICAO
                                if (res.Key.descricao != null)
                                {
                                    lbl1 = new Label();
                                    lbl1.CssClass = "card-text font-italic text-muted";
                                    lbl1.Text = "<br/>\"" + res.Key.descricao + "\"<br />";
                                    section.Controls.Add(lbl1);
                                }

                                //BIUTON
                                btn = new Button();
                                btn.CssClass = "btn btn-success mt-4";
                                btn.Text = "Solicitar agendamento";
                                btn.Click += new EventHandler(btn_SolicitarAgendamento_Click);
                                btn.CommandArgument = res.Key.cnpj;
                                btn.ID = "btn_SolicitarAgendamento" + res.Key.cnpj;
                                section.Controls.Add(btn);
                                container.Controls.Add(section);
                                row.Controls.Add(container);

                                card.Controls.Add(row);
                                div_Resultados.Controls.Add(card);
                                pnl_Alert.Visible = false;
                            }
                        }
                        if (cont == 0)
                        {
                            div_Resultados.Controls.Clear();
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                            pnl_Alert.Visible = true;
                        }
                        if (div_Resultados.Controls.Count == 0)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                            pnl_Alert.Visible = true;
                        }
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger mt-3";
                        lbl_Alert.Text = "Nenhuma oficina encontrada perto de você";
                        pnl_Alert.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger mt-3";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_SolicitarAgendamento_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Oficina o = context.Oficina.Where(oficina => oficina.cnpj.Equals(btn.CommandArgument)).FirstOrDefault();
                    Session["oficina"] = o;
                    Response.Redirect("agendamento_Cadastro.aspx", false);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        protected async void btn_GambiButton_Click(object sender, EventArgs e)
        {
            switch (txt_LatLon.Text)
            {
                case "User denied the request for Geolocation.":
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "É preciso que você aceite a utilização da localização para podermos recomendar oficinas próximas";
                    pnl_Alert.Visible = true;
                    break;

                case "Location information is unavailable.":
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Erro na sua localização";
                    pnl_Alert.Visible = true;
                    break;

                case "The request to get user location timed out.":
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Permita a utilização da localização e recarregue a página para ver oficinas recomendadas";
                    pnl_Alert.Visible = true;
                    break;

                case "An unknown error occurred.":
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Um erro inesperado ococrreu";
                    pnl_Alert.Visible = true;
                    break;

                case "Geolocation is not supported by this browser.":
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Seu navegador não suporta a localização dinamica";
                    pnl_Alert.Visible = true;
                    break;

                case "":
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Não há nada para exibir";
                    pnl_Alert.Visible = true;
                    break;

                case null:
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Não há nada para exibir";
                    pnl_Alert.Visible = true;
                    break;

                default:
                    recomendar_Oficinas();
                    break;
            }
        }
    }
}