using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class compra_Cadastrar : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;
        private Compra compra;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            compra = (Compra)Session["compra"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        funcionario = context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();

                        if (funcionario != null)
                        {
                            if (context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault() == null || funcionario.cargo.ToUpper() != "GERENTE")
                            {
                                Response.Redirect("home.aspx", false);
                            }
                            else
                            {
                                pnl_Alert.Visible = false;
                                if (!IsPostBack)
                                {
                                    preencher_Fornecedores();
                                    preencher_Produto();
                                }
                            }
                        }
                        else
                        {
                            Response.Redirect("home.aspx", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void preencher_Fornecedores()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    var query = context.Fornecedor.Where(f => f.cnpjOficina.Equals(funcionario.cnpjOficina) && f.status == 1).ToList();

                    if (query.Count > 0)
                    {
                        select_Fornecedores.Items.Add(new ListItem("Selecione um fornecedor", ""));
                        foreach (var fornecedor in query)
                        {
                            select_Fornecedores.Items.Add(new ListItem(fornecedor.cnpjFornecedor + " - " + fornecedor.razaoSocial, fornecedor.idFornecedor.ToString()));
                        }
                    }
                    else
                    {
                        select_Fornecedores.Items.Add(new ListItem("", ""));
                        pnl_Alert.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void preencher_Produto()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    var query = context.Produto.Where(p => p.cnpjOficina.Equals(funcionario.cnpjOficina) && p.ativo == 1).ToList();

                    if (query.Count > 0)
                    {
                        select_Produto.Items.Add(new ListItem("Selecione um produto", ""));
                        foreach (var produto in query)
                        {
                            select_Produto.Items.Add(new ListItem(produto.descricao, produto.idProduto.ToString()));
                        }
                    }
                    else
                    {
                        select_Produto.Items.Add(new ListItem("", ""));
                        pnl_AlertProduto.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void select_Fornecedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fornecedor fornecedor = null;

            try
            {
                using (var context = new DatabaseEntities())
                {
                    fornecedor = context.Fornecedor.Where(f => f.idFornecedor.ToString().Equals(select_Fornecedores.SelectedValue) && f.status == 1).FirstOrDefault();

                    if (fornecedor != null)
                    {
                        txt_FornecedorCNPJ.Text = fornecedor.cnpjFornecedor;
                        txt_FornecedorNome.Text = fornecedor.razaoSocial;
                        txt_FornecedorTelefone.Text = fornecedor.telefone;
                        txt_FornecedorEmail.Text = fornecedor.email;
                    }
                    else
                    {
                        txt_FornecedorCNPJ.Text = "";
                        txt_FornecedorNome.Text = "";
                        txt_FornecedorTelefone.Text = "";
                        txt_FornecedorEmail.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void select_Produto_SelectedIndexChanged(object sender, EventArgs e)
        {
            Produto produto;

            try
            {
                using (var context = new DatabaseEntities())
                {
                    produto = context.Produto.Where(p => p.idProduto.ToString().Equals(select_Produto.SelectedValue) && p.ativo == 1).FirstOrDefault();

                    if (produto != null)
                    {
                        txt_Produto.Text = produto.descricao;
                        txt_ProdutoCategoria.Text = produto.categoria;
                        txt_ProdutoMarca.Text = produto.marca;
                        txt_ProdutoQuantidadeAtual.Text = produto.quantidade.ToString();
                        txt_ProdutoPrecoCompra.Text = "R$ " + produto.precoCompra.ToString("0.00");
                        txt_ProdutoPrecoCompra.ReadOnly = false;
                        if (produto.precoVenda != null)
                        {
                            double dbl = (double)produto.precoVenda;
                            txt_ProdutoPrecoVenda.Text = "R$ " + dbl.ToString("0.00");
                            txt_ProdutoPrecoVenda.ReadOnly = false;
                        }
                        else
                            txt_ProdutoPrecoVenda.Text = "Não foi deifinido preço venda";
                    }
                    else
                    {
                        txt_Produto.Text = "";
                        txt_ProdutoCategoria.Text = "";
                        txt_ProdutoMarca.Text = "";
                        txt_ProdutoQuantidadeAtual.Text = "";
                        txt_ProdutoPrecoCompra.Text = "";
                        txt_ProdutoPrecoVenda.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}