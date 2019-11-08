using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace FixFinder.Pages
{
    public partial class oficina_CadastroCont : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "https://sandbox.pagseguro.uol.com.br");
            string code = Request["notificationCode"];
            getNotification(code);
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
                        lbl_Alert.Text = "Oficina autorizada!";
                    }
                    else
                    {
                        //erro
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
    }
}