using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class oficina_cadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    Oficina o = new Oficina
                    {
                        cnpj = txt_CNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", ""),
                        nome = txt_Nome.Text,
                        telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                        email = txt_Email.Text,
                        capacidadeAgendamentos = int.Parse(num_Agendamentos.Text),
                        statusAssinatura = 1
                    };

                    if (context.Oficina.Where(oficina => oficina.cnpj.Equals(o.cnpj)).ToList().Count > 0)
                    {
                        pnl_Alert.Visible = true;
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Uma oficina com este CNPJ já existe";
                    }
                    else
                    {
                        context.Oficina.Add(o);
                        context.SaveChanges();
                        pnl_Alert.Visible = true;
                        pnl_Alert.CssClass = "alert alert-success";
                        lbl_Alert.Text = "Oficina cadastrada com sucesso";
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }
    }
}