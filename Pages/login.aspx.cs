using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;
using FixFinder.Controls;
using System.Text;
using System.Security.Cryptography;

namespace FixFinder.Pages
{
    public partial class login : System.Web.UI.Page
    {
        private Cliente cliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            pnl_Alert.Visible = false;
            cliente = (Cliente)Session["usuario"];
            if (cliente != null)
            {
                Response.Redirect("home.aspx", false);
            }
        }

        private String encrypt(String senha)
        {
            var stringHash = "";
            try
            {
                UnicodeEncoding encode = new UnicodeEncoding();
                byte[] hashBytes, mensagemBytes = encode.GetBytes(senha);
                SHA512Managed sha512Manager = new SHA512Managed();

                hashBytes = sha512Manager.ComputeHash(mensagemBytes);

                foreach (byte b in hashBytes)
                {
                    //hexadecimal em 2 caracteres
                    stringHash += String.Format("{0:x2}", b);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return stringHash;
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            String login = txt_NomeUsuario.Text.ToUpper();
            String senha = encrypt(txt_Senha.Text);
            cliente = null;
            using (var context = new DatabaseEntities())
            {
                try
                {
                    cliente = context.Cliente.Where(c => (c.login.ToUpper() == login && c.senha == senha)).FirstOrDefault<Cliente>();
                    if (cliente == null)
                    {
                        lbl_Alert.Text = "Usuário e/ou senha incorretos.";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        Session["usuario"] = cliente;
                        if (Session["lastPage"] == null)
                        {
                            log_Login();
                            Response.Redirect("home.aspx", false);
                        }
                        else
                        {
                            log_Login();
                            Response.Redirect((String)Session["lastPage"], false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void log_Login()
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                Log_Login log = new Log_Login()
                {
                    cpfCliente = cliente.cpf,
                    data = DateTime.Now
                };
                context.Log_Login.Add(log);
                context.SaveChanges();
            }
        }

        protected void btn_Esqueci_Senha_Click(object sender, EventArgs e)
        {
            Response.Redirect("login_EsqueciSenha.aspx", false);
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
            Response.Redirect("cliente_Cadastro.aspx", false);
        }
    }
}