using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class servico_Lista : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            //c = (Cliente)Session["usuario"];
            //if (c == null)
            //{
            //    Response.Redirect("home.aspx", false);
            //}
            //else
            //{
            //    using (DatabaseEntities context = new DatabaseEntities())
            //    {
            //        c = context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).FirstOrDefault();
            //        if (c.Funcionario == null)
            //        {
            //            Response.Redirect("home.aspx", false);
            //        }
            //    }
            //}
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Servico s = new Servico
                    {
                        descricao = txt_Descricao.Text,
                        valor = double.Parse(txt_Valor.Text),
                        cnpjOficina = c.Funcionario.cnpjOficina
                    };
                    context.Servico.Add(s);
                    context.SaveChanges();

                    form_Cadastro.Visible = false;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_CadastrarServico_Click(object sender, EventArgs e)
        {
            form_Cadastro.Visible = true;
        }
    }
}