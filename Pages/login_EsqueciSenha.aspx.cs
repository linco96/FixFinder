using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;
using System.Windows;
using System.Net.Mail;
using FixFinder.Controls;

namespace FixFinder.Pages
{
    public partial class login_EsqueciSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnl_Alert.Visible = false;
            Header_Control h = new Header_Control();
        }

        protected void btn_EnviarEmail_Click(object sender, EventArgs e)
        {
            Boolean erroEmailCPF = false;
            Cliente cliente = null;
            String email = "";
            String CPF = "";

            pnl_Alert.Visible = false;

            if (txt_CPF.Text != "")
                CPF = txt_CPF.Text.Replace(".", "").Replace("-", "");

            if (txt_Email.Text != "")
                email = txt_Email.Text;

            if (CPF == "" && email == "")
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Forneça um e-mail ou CPF";
                pnl_Alert.Visible = true;
            }
            else
            {
                using (var context = new DatabaseEntities())
                {
                    try
                    {
                        if (CPF != "" && email == "")
                            cliente = context.Cliente.Where(c => (c.cpf == CPF)).FirstOrDefault<Cliente>();
                        else if (email != "" && CPF == "")
                            cliente = context.Cliente.Where(c => (c.email == email)).FirstOrDefault<Cliente>();
                        else
                        {
                            if (context.Cliente.Where(c => (c.email == email)).FirstOrDefault<Cliente>() != context.Cliente.Where(c => (c.cpf == CPF)).FirstOrDefault<Cliente>())
                            {
                                pnl_Alert.CssClass = "alert alert-danger";
                                lbl_Alert.Text = "O e-mail informado não corresponde ao CPF cadastrado. Forneça apenas um dos campos e tente novamente";
                                pnl_Alert.Visible = true;
                                erroEmailCPF = true;
                            }
                            else
                            {
                                cliente = context.Cliente.Where(c => (c.cpf == CPF)).FirstOrDefault<Cliente>();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('" + ex.Message + "');</script>");
                    }
                }
                if (cliente != null)
                {
                    Enviar_Email(cliente);
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "E-mail enviado.";
                    pnl_Alert.Visible = true;
                }
                else if (!erroEmailCPF)
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "CPF/E-mail não cadastrado!";
                    pnl_Alert.Visible = true;
                }
            }
        }

        protected void Enviar_Email(Cliente cliente)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("noreply.fixfinder@gmail.com");
                mail.To.Add(cliente.email);
                mail.Subject = "Recuperação de Senha - Fix Finder";
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody = "Olá <label style='font-weight: bold; color: #1E90FF'>" + cliente.nome + "</label>,<br /> Conforme solicitado, segue sua senha de acesso ao Fix Finder:<br /><br />Senha: " + cliente.senha + "<br /><br />Agora que voçê já lembrou sua senha de acesso, <br />pode consultar seus agendamento e acompanhar seus orçamentos normalmente.<br /><br />Equipe FixFinder agradece.";
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
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}