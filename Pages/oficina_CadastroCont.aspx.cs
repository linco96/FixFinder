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
            string type = Request["notificationType"];
            if (type.Equals("applicationAuthorization"))
            {
                string code = Request["notificationCode"];
                getNotification(code);
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

                        string status = xml.GetElementsByTagName("status")[0].InnerXml;
                        int idOrcamento = int.Parse(xml.GetElementsByTagName("reference")[0].InnerXml.Split('-')[1]);
                        Pagamento p = context.Pagamento.Where(pag => pag.idOrcamento == idOrcamento).FirstOrDefault();
                        Orcamento o;
                        switch (status)
                        {
                            case "1":
                                p.status = "1";
                                break;

                            case "2":
                                p.status = "2";
                                break;

                            case "3":
                                p.status = "3";
                                o = context.Orcamento.Where(orc => orc.idOrcamento == p.idOrcamento).FirstOrDefault();
                                o.status = "Concluído";
                                context.SaveChanges();
                                break;

                            case "7":
                                o = context.Orcamento.Where(orc => orc.idOrcamento == p.idOrcamento).FirstOrDefault();
                                o.status = "Pagamento pendente";
                                context.Pagamento.Remove(p);
                                context.SaveChanges();
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        //Erro
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}