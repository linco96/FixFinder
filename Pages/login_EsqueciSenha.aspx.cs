using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;
using System.Windows;
using System.Net.Mail;

namespace FixFinder.Pages
{
    public partial class login_EsqueciSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnl_Alert.Visible = false;
        }

        protected void btn_EnviarEmail_Click(object sender, EventArgs e)
        {
            String email;
            using (var context = new DatabaseEntities())
            {
                try
                {
                    Cliente cliente = context.Cliente.Where(c => (c.cpf == txt_CPF.Text)).FirstOrDefault<Cliente>();

                    if (cliente != null)
                    {
                        MailMessage mail = new MailMessage();

                        mail.From = new MailAddress("noreply.fixfinder@gmail.com");
                        mail.To.Add(cliente.email);
                        mail.Subject = "Recuperação de Senha - Fix Finder";
                        mail.IsBodyHtml = true;
                        string htmlBody;
                        htmlBody = "Sua senha é " + cliente.senha;
                        mail.Body = htmlBody;
                        mail.Priority = MailPriority.Normal;
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        mail.BodyEncoding = System.Text.Encoding.UTF8;

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("noreply.fixfinder@gmail.com", "tccfixfinder");
                        smtp.Send(mail);

                        pnl_Alert.CssClass = "alert alert-success";
                        lbl_Alert.Text = "E-mail enviado.";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        lbl_Alert.Text = "CPF/E-mail não cadastrado!";
                        pnl_Alert.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }
    }
}