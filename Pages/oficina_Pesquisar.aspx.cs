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

                        HtmlGenericControl card;
                        HtmlGenericControl row;
                        HtmlGenericControl container;
                        HtmlGenericControl section1;
                        HtmlGenericControl section2;

                        Image img;
                        HtmlGenericControl title;
                        Label lbl1;
                        Label lbl2;
                        HtmlGenericControl biggie;
                        Button btn;
                        foreach (KeyValuePair<Oficina, Element> res in resultados)
                        {
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
    }
}