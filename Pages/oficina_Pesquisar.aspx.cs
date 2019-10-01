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
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btn_Pesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
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
                        Dictionary<Oficina, Element> resultados = new Dictionary<Oficina, Element>();
                        for (int i = 0; i < oficinas.Count; i++)
                        {
                            resultados.Add(oficinas[i], searchResult.rows[0].elements[i]);
                        }

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
                            card = new Panel();
                            card.CssClass = "card mb-3";

                            row = new Panel();
                            row.CssClass = "row no-gutters";

                            //IMAGE
                            container = new Panel();
                            container.CssClass = "col-md-3 border-right text-center";

                            img = new Image();
                            img.Style.Add("max-width", "100%");
                            img.CssClass = "Responsive image";
                            picture = context.FotoOficina.Where(pic => pic.cnpjOficina.Equals(res.Key.cnpj)).FirstOrDefault();
                            if (picture != null)
                            {
                                String base64 = Convert.ToBase64String(picture.foto);
                                img.ImageUrl = "data:Image/png;base64," + base64;
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
                                lbl2.Text = "-/10";
                            else
                                lbl2.Text = res.Key.reputacao.ToString().Replace(",", ".") + "/10";
                            lbl1.Controls.Add(lbl2);

                            img = new Image();
                            img.CssClass = "align-top";
                            img.ImageUrl = "../Content/star_24.png";
                            lbl1.Controls.Add(img);
                            section.Controls.Add(lbl1);
                            section.Controls.Add(new LiteralControl("<br/>"));

                            //DISTANCIA
                            biggie = new HtmlGenericControl("small");
                            biggie.Attributes.Add("class", "card-title text-muted");
                            biggie.InnerHtml = res.Value.distance.text;
                            section.Controls.Add(biggie);
                            container.Controls.Add(section);

                            //BODY
                            section = new Panel();
                            section.CssClass = "card-body";

                            //ENDERECO
                            lbl1 = new Label();
                            lbl1.CssClass = "card-text";
                            lbl1.Text = res.Key.Endereco.logradouro + ", " + res.Key.Endereco.numero.ToString() + "<br />" + res.Key.Endereco.cep + " - " + res.Key.Endereco.cidade + " " + res.Key.Endereco.uf.ToUpper();
                            section.Controls.Add(lbl1);

                            //DESCRICAO
                            if (res.Key.descricao != null)
                            {
                                lbl1 = new Label();
                                lbl1.CssClass = "card-text";
                                lbl1.Text = res.Key.descricao;
                                section.Controls.Add(lbl1);
                            }

                            //BIUTON
                            btn = new Button();
                            btn.CssClass = "btn btn-primary";
                            btn.Text = "Solicitar agendamento";
                            btn.Click += new EventHandler(btn_SolicitarAgendamento_Click);
                            btn.CommandArgument = res.Key.cnpj;
                            section.Controls.Add(btn);
                            container.Controls.Add(section);
                            row.Controls.Add(container);

                            card.Controls.Add(row);
                            div_Resultados.Controls.Add(card);
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
        }
    }
}