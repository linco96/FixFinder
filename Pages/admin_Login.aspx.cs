using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class admin_Login : System.Web.UI.Page
    {
        private Admin adm;

        protected void Page_Load(object sender, EventArgs e)
        {
            pnl_Alert.Visible = false;
            adm = (Admin)Session["admin"];
            if (adm != null)
            {
                Response.Redirect("admin.aspx", false);
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
            adm = null;
            String senha = encrypt(txt_Senha.Text);
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    adm = context.Admin.Where(a => a.login.ToUpper() == login && a.senha == senha).FirstOrDefault();
                    if (adm == null)
                    {
                        lbl_Alert.Text = "Usuário e/ou senha incorretos";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        Session["admin"] = adm;
                        Response.Redirect("admin.aspx", false);
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
    }
}