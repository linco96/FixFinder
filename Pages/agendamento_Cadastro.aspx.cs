using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class agendamento_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            o = (Oficina)Session["oficina"];
            c = (Cliente)Session["usuario"];
            if (o == null)
            {
                Response.Redirect("oficina_Pesquisar.aspx", false);
            }
            else if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                if (Session["lastPage"] != null)
                    Session["lastPage"] = null;
                preencherSelect();
            }
        }

        protected void preencherSelect()
        {
            try
            {
                if (txt_Veiculo.Items.Count == 0 || txt_Horario.Items.Count == 0)
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        ListItem item;

                        List<Veiculo> veiculos = context.Veiculo.Where(v => v.cpfCliente.Equals(c.cpf)).ToList();
                        foreach (Veiculo v in veiculos)
                        {
                            item = new ListItem();
                            item.Text = v.modelo + " - " + v.placa;
                            item.Value = v.idVeiculo.ToString();
                            txt_Veiculo.Items.Add(item);
                        }
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

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
        }
    }
}