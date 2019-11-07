using FixFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class oficina_Pesquisar : System.Web.UI.Page
    {
        private static List<KeyValuePair<Oficina, Element>> resultadosAtuais;
        private static bool sortMode;

        private static double distanceFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (resultadosAtuais == null)
            {
                resultadosAtuais = new List<KeyValuePair<Oficina, Element>>();
                logo.Visible = true;
            }
            else if (resultadosAtuais.Count > 0)
            {
                logo.Visible = false;
                foreach (KeyValuePair<Oficina, Element> res in resultadosAtuais)
                {
                    inserirResultado(res);
                }
                if (div_Resultados.Controls.Count == 0)
                {
                    logo.Visible = true;
                    pnl_Alert.CssClass = "alert alert-danger mt-3";
                    lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                    pnl_Alert.Visible = true;
                }
                else
                {
                    pnl_Alert.Visible = false;
                }
            }
            else
            {
                logo.Visible = true;
            }

            if (distanceFilter == 0)
                distanceFilter = 50;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //ARRUMAR BUTTON DE ORDENAR
            if (sortMode)
            {
                btn_OrdenarDistancia.CssClass = "btn btn-outline-primary";
                btn_OrdenarNota.CssClass = "btn btn-primary";
            }
            else
            {
                btn_OrdenarDistancia.CssClass = "btn btn-primary";
                btn_OrdenarNota.CssClass = "btn btn-outline-primary";
            }

            //ATIVAR O BUTTON DE FILTRO SELECIONADO
            List<Button> filterButtons = new List<Button>() { btn_Filtro1, btn_Filtro2, btn_Filtro3, btn_Filtro4 };
            foreach (Button btn in filterButtons)
            {
                if (Double.Parse(btn.CommandArgument) == distanceFilter)
                {
                    btn.CssClass = "dropdown-item active";
                }
                else
                {
                    btn.CssClass = "dropdown-item";
                }
            }
        }

        protected void inserirResultado(KeyValuePair<Oficina, Element> res)
        {
            try
            {
                if (Double.Parse(res.Value.distance.text.Replace(".", ",").Split(' ')[0]) <= distanceFilter)
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        Oficina o = context.Oficina.Where(oficina => oficina.cnpj.Equals(res.Key.cnpj)).FirstOrDefault();

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

                        FotoOficina picture;
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
                        lbl1.Text = o.Endereco.logradouro + ", " + o.Endereco.numero.ToString() + " - " + o.Endereco.bairro + ", " + o.Endereco.cidade + " - " + o.Endereco.uf.ToUpper() + ", " + o.Endereco.cep + "<br />";
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

        protected async void btn_Pesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    //LIMPA RESULTADOS
                    resultadosAtuais.Clear();
                    div_Resultados.Controls.Clear();

                    //CRIA STRING DESTINOS
                    List<Oficina> oficinas = context.Oficina.ToList();
                    StringBuilder destinos = new StringBuilder();
                    Endereco end;
                    for (int i = 0; i < oficinas.Count; i++)
                    {
                        end = oficinas[i].Endereco;
                        destinos.Append(end.numero.ToString());
                        destinos.Append("+");
                        destinos.Append(end.logradouro.Trim().Replace(" ", "+"));
                        destinos.Append("+");
                        destinos.Append(end.cidade.Trim().Replace(" ", "+"));
                        destinos.Append("+");
                        destinos.Append(end.uf.ToUpper());
                        if (i < oficinas.Count - 1)
                        {
                            destinos.Append("|");
                        }
                    }

                    //REALIZAR REQUEST
                    String kelly = context.Key.FirstOrDefault().idChave;
                    SearchResult searchResult;
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync("https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=" + txt_Pesquisa.Text.Trim().Replace(" ", "+") + "&destinations=" + destinos.ToString() + "&key=" + kelly);
                    var responseString = await response.Content.ReadAsStringAsync();
                    searchResult = JsonConvert.DeserializeObject<SearchResult>(responseString);
                    //MONTA AS PARADA
                    if (searchResult.status.Equals("OK"))
                    {
                        List<KeyValuePair<Oficina, Element>> resultados = new List<KeyValuePair<Oficina, Element>>();
                        for (int i = 0; i < oficinas.Count; i++)
                        {
                            if (searchResult.rows[0].elements[i].status.Equals("OK"))
                                resultados.Add(new KeyValuePair<Oficina, Element>(oficinas[i], searchResult.rows[0].elements[i]));
                        }
                        if (resultados.Count > 0)
                        {
                            if (sortMode)
                            {
                                resultados = resultados.OrderByDescending(p => p.Key.reputacao).ToList();
                            }
                            else
                            {
                                resultados.Sort((p1, p2) => p1.Value.distance.value.CompareTo(p2.Value.distance.value));
                            }
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

                            FotoOficina picture;
                            foreach (KeyValuePair<Oficina, Element> res in resultados)
                            {
                                if (Double.Parse(res.Value.distance.text.Replace(".", ",").Split(' ')[0]) <= distanceFilter)
                                {
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
                                    logo.Visible = false;
                                    pnl_Alert.Visible = false;
                                }
                            }
                            if (div_Resultados.Controls.Count == 0)
                            {
                                logo.Visible = true;
                                pnl_Alert.CssClass = "alert alert-danger mt-3";
                                lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                                pnl_Alert.Visible = true;
                            }
                        }
                        else
                        {
                            logo.Visible = true;
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                            pnl_Alert.Visible = true;
                        }
                    }
                    else
                    {
                        logo.Visible = true;
                        pnl_Alert.CssClass = "alert alert-danger mt-3";
                        lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                        pnl_Alert.Visible = true;
                    }
                }
                log_Pesquisar();
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger mt-3";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        private void log_Pesquisar()
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Cliente c = (Cliente)Session["usuario"];
                LogPesquisa log;
                if (c == null)
                    log = new LogPesquisa()
                    {
                        data = DateTime.Now
                    };
                else
                    log = new LogPesquisa()
                    {
                        data = DateTime.Now,
                        cpfCliente = c.cpf
                    };
                context.LogPesquisa.Add(log);
                context.SaveChanges();
            }
        }

        protected async void btn_CarregarEndereco_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    string kelly = context.Key.FirstOrDefault().idChave;

                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + txt_LatLon.Text + "&key=" + kelly);
                    var responseString = await response.Content.ReadAsStringAsync();
                    Address addr = JsonConvert.DeserializeObject<Address>(responseString);
                    if (addr.status.Equals("OK"))
                        txt_Pesquisa.Text = addr.results[0].formatted_address;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
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

        protected void btn_OrdenarDistancia_Click(object sender, EventArgs e)
        {
            if (sortMode)
            {
                sortMode = false;
                btn_OrdenarDistancia.CssClass = "btn btn-primary";
                btn_OrdenarNota.CssClass = "btn btn-outline-primary";
                if (resultadosAtuais.Count > 0)
                {
                    resultadosAtuais.Sort((p1, p2) => p1.Value.distance.value.CompareTo(p2.Value.distance.value));
                    div_Resultados.Controls.Clear();
                    foreach (KeyValuePair<Oficina, Element> res in resultadosAtuais)
                    {
                        inserirResultado(res);
                    }
                    if (div_Resultados.Controls.Count == 0)
                    {
                        logo.Visible = true;
                        pnl_Alert.CssClass = "alert alert-danger mt-3";
                        lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        pnl_Alert.Visible = false;
                    }
                }
            }
        }

        protected void btn_OrdenarNota_Click(object sender, EventArgs e)
        {
            if (!sortMode)
            {
                sortMode = true;
                btn_OrdenarDistancia.CssClass = "btn btn-outline-primary";
                btn_OrdenarNota.CssClass = "btn btn-primary";
                if (resultadosAtuais.Count > 0)
                {
                    resultadosAtuais = resultadosAtuais.OrderByDescending(p => p.Key.reputacao).ToList();
                    div_Resultados.Controls.Clear();
                    foreach (KeyValuePair<Oficina, Element> res in resultadosAtuais)
                    {
                        inserirResultado(res);
                    }
                    if (div_Resultados.Controls.Count == 0)
                    {
                        logo.Visible = true;
                        pnl_Alert.CssClass = "alert alert-danger mt-3";
                        lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        pnl_Alert.Visible = false;
                    }
                }
            }
        }

        protected void btn_Filtro_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                distanceFilter = Double.Parse(btn.CommandArgument);
                if (resultadosAtuais.Count > 0)
                {
                    div_Resultados.Controls.Clear();
                    foreach (KeyValuePair<Oficina, Element> res in resultadosAtuais)
                    {
                        inserirResultado(res);
                    }
                    if (div_Resultados.Controls.Count == 0)
                    {
                        logo.Visible = true;
                        pnl_Alert.CssClass = "alert alert-danger mt-3";
                        lbl_Alert.Text = "Nenhuma oficina encontrada. Verifique os parâmetros de pesquisa informados";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        pnl_Alert.Visible = false;
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
    }
}