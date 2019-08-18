using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class veiculo_Cadastro : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            String placa;

            if (txt_PlacaAntiga.Text != "")
                placa = txt_PlacaAntiga.Text.Replace("-", "");
            else if (txt_PlacaNova.Text != "")
                placa = txt_PlacaNova.Text.Replace(" ", "");
            else
                placa = "";

            if (placa != "" && txt_Ano.Text != "" && txt_Marca.Text != "" && txt_Modelo.Text != "")
            {
                cadastrarVeiculo(new Veiculo()
                {
                    ano = int.Parse(txt_Ano.Text),
                    marca = txt_Marca.Text,
                    modelo = txt_Modelo.Text,
                    placa = placa,
                    cpfCliente = c.cpf
                });
            }
            else
            {
                lbl_Alert.Text = "Preencha todos os campos!";
                pnl_Alert.CssClass = "alert alert-danger";
                pnl_Alert.Visible = true;
            }
        }

        protected void radio_ModeloPlaca_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tipoPlaca = radio_ModeloPlaca.SelectedItem.Value;

            switch (tipoPlaca)
            {
                case "nova":
                    txt_PlacaNova.Visible = true;
                    txt_PlacaAntiga.Visible = false;
                    txt_PlacaAntiga.Text = "";
                    break;

                case "antiga":
                    txt_PlacaAntiga.Visible = true;
                    txt_PlacaNova.Visible = false;
                    txt_PlacaNova.Text = "";
                    break;

                default:
                    lbl_Alert.Text = "Opcao de placa inválida!";
                    pnl_Alert.Visible = true;
                    break;
            }
        }

        private void cadastrarVeiculo(Veiculo veiculo)
        {
            using (var context = new DatabaseEntities())
            {
                try
                {
                    if (context.Veiculo.Where(v => v.cpfCliente.Equals(veiculo.cpfCliente) && v.placa.Equals(veiculo.placa)).ToList().Count > 0)
                    {
                        lbl_Alert.Text = "Veículo já está cadastrado!";
                        pnl_Alert.CssClass = "alert alert-danger";
                        pnl_Alert.Visible = true;
                    }
                    else
                    {
                        context.Veiculo.Add(veiculo);
                        context.SaveChanges();
                        lbl_Alert.Text = "Veículo cadastrado!";
                        pnl_Alert.CssClass = "alert alert-success";
                        pnl_Alert.Visible = true;
                        Response.AddHeader("REFRESH", "3; url=veiculo_Lista.aspx");
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