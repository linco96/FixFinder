using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class cliente_EditarPerfil : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            txt_Nome.Text = c.nome;
            txt_Telefone.Text = c.telefone;
            txt_Email.Text = c.email;
            date_DataNascimento.Text = c.dataNascimento.ToString();
        }

        protected void btn_Salvar_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    c = context.Cliente.Where(cliente => c.cpf == cliente.cpf).FirstOrDefault();
                    c.nome = txt_Nome.Text;
                    c.telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    c.email = txt_Email.Text;
                    c.dataNascimento = DateTime.Parse(date_DataNascimento.Text);

                    context.SaveChanges();
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