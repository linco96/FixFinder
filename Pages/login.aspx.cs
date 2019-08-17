using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;
using FixFinder.Controls;

namespace FixFinder.Pages
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnl_Alert.Visible = false;

            Header_Control h = new Header_Control();
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            String login = txt_NomeUsuario.Text.ToUpper();
            using (var context = new DatabaseEntities())
            {
                try
                {
                    Cliente cliente = context.Cliente.Where(c => (c.login.ToUpper() == login && c.senha == txt_Senha.Text)).FirstOrDefault<Cliente>();
                    if (cliente == null)
                    {
                        lbl_Alert.Text = "Usuário e/ou senha incorretos.";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        Session["usuario"] = cliente;
                        //Response.Redirect("paginaDestino.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
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