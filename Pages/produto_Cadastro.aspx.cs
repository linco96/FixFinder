using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class produto_Cadastro : System.Web.UI.Page
    {
        private Compra compra;
        private Cliente cliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            compra = (Compra)Session["compra"];
            cliente = (Cliente)Session["usuario"];
            if (cliente == null)
            {
                Response.Redirect("home.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    cliente = context.Cliente.Where(c => c.cpf.Equals(cliente.cpf)).FirstOrDefault();
                    if (cliente.Funcionario == null)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else if (compra != null)
                    {
                        txt_Quantidade.ReadOnly = true;
                        txt_Quantidade.Text = "0";
                    }
                }
            }
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    double? precoVenda = null;
                    DateTime? dataValidade = null;
                    if (txt_PrecoVenda.Text.Length > 0)
                        precoVenda = double.Parse(txt_PrecoVenda.Text);
                    if (txt_Validade.Text.Length > 0)
                        dataValidade = DateTime.Parse(txt_Validade.Text);

                    Produto p = new Produto
                    {
                        descricao = txt_Descricao.Text,
                        quantidade = int.Parse(txt_Quantidade.Text),
                        precoCompra = double.Parse(txt_PrecoCompra.Text),
                        precoVenda = precoVenda,
                        marca = txt_Marca.Text,
                        validade = dataValidade,
                        categoria = txt_Categoria.Text,
                        cnpjOficina = cliente.Funcionario.cnpjOficina,
                        ativo = 1
                    };

                    context.Produto.Add(p);
                    context.SaveChanges();

                    if (compra != null)
                    {
                        Response.Redirect("compra_Cadastrar.aspx", false);
                    }
                    else
                    {
                        Response.Redirect("produto_Lista.aspx", false);
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

        protected void btn_Voltar_Click(object sender, EventArgs e)
        {
            if (compra != null)
            {
                Response.Redirect("compra_Cadastrar.aspx", false);
            }
            else
            {
                Response.Redirect("produto_Lista.aspx", false);
            }
        }
    }
}