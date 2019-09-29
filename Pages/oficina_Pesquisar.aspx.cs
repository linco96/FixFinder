using FixFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class oficina_Pesquisar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Pesquisar_Click(object sender, EventArgs e)
        {
        }

        protected async void btn_CarregarEndereco_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    string kelly = context.Key.FirstOrDefault().idChave;

                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + txt_Pesquisa.Text + "&key=" + kelly);
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