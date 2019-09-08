using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class produto_Editar : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;
        private Produto produto;
        private static bool alterar;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            produto = (Produto)Session["produto"];
            if (c == null)
                Response.Redirect("login.aspx", false);
            else if (produto == null)
                Response.Redirect("produto_lista.aspx", false);
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        funcionario = context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();
                        if (funcionario == null)
                        {
                            Response.Redirect("home.aspx", false);
                        }
                        else
                        {
                            if (context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault() != null)
                            {
                                produto = context.Produto.Where(p => p.idProduto == produto.idProduto).FirstOrDefault();
                                if (produto != null)
                                {
                                    if (!alterar)
                                    {
                                        txt_Descricao.Text = produto.descricao;
                                        txt_Quantidade.Text = produto.quantidade.ToString();
                                        txt_PrecoCompra.Text = produto.precoCompra.ToString();
                                        txt_PrecoVenda.Text = produto.precoVenda.ToString();
                                        txt_Marca.Text = produto.marca;
                                        txt_Validade.Text = produto.validade.ToString();
                                        txt_Categoria.Text = produto.categoria;
                                        alterar = true;
                                    }
                                }
                                else
                                {
                                    Response.Redirect("fornecedor_Lista.aspx", false);
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('erro no banco - Oficina nao cadastrada');</script>");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void btn_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    produto = context.Produto.Where(p => p.idProduto == produto.idProduto).FirstOrDefault();
                    produto.descricao = txt_Descricao.Text;
                    produto.categoria = txt_Categoria.Text;
                    produto.marca = txt_Marca.Text;
                    produto.precoCompra = double.Parse(txt_PrecoCompra.Text.Replace("R$", "").Replace(" ", ""));
                    if (txt_PrecoVenda.Text.Length > 0)
                        produto.precoVenda = double.Parse(txt_PrecoVenda.Text.Replace("R$", "").Replace(" ", ""));
                    if (txt_Validade.Text.Length > 0)
                        produto.validade = DateTime.Parse(txt_Validade.Text);

                    context.SaveChanges();
                    Session["produto"] = null;
                    Response.Redirect("produto_Lista.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btn_Voltar_Click(object sender, EventArgs e)
        {
            Session["produto"] = null;
            Response.Redirect("produto_Lista.aspx", false);
        }
    }
}