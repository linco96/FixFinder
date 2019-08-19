using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class fornecedor_Cadastro : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx-", false);
            }
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        if (context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault() != null)
                        {
                            //Response.Redirect("home.aspx", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    Fornecedor f = new Fornecedor
                    {
                        cnpj = txt_CNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", ""),
                        razaoSocial = txt_Nome.Text,
                        email = txt_Email.Text,
                        telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")
                    };
                    context.Fornecedor.Add(f);
                    context.SaveChanges();
                    pnl_Alert.Visible = true;
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "Fornecedor cadastrada com sucesso";
                    Response.AddHeader("REFRESH", "2; url=fornecedor_Lista.aspx");
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