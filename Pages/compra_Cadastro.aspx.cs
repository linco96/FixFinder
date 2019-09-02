using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class compra_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;
        private Compra compra;
        private static List<Produto> listaProdutos;
        private static int idProduto;

        //A FAZER
        //VALIDADE NAO ESTA INDO PARA O FORMULARIO
        //AO ADICIONAR PRODUTO, SE A VALIDADE FOR DIFERENTE É NECESSARIO CADASTRAR UM NOVO PRODUTO (PENSADO SOBRE ISSO AINDA)
        //FAZER BOTAO DE REMOVER E FAZER O BOTAO FUNCIONAR
        //FAZER A COLLECTION VIRAR LIST PARA ASSIM SE TIVER UMA COMPRA CADASTRADA ELE JA CARREGAR OS PRODUTOS NA LISTA
        //SEI LA FALTA BASTANTE

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            compra = (Compra)Session["compra"];
            if (c == null)
            {
                Session["compra"] = null;
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
                                Session["compra"] = null;
                                Response.Redirect("home.aspx", false);
                            }
                            else
                            {
                                compra = (Compra)Session["compra"];
                                if (compra != null)
                                {
                                    //transformar a collection da compra em list de produto
                                    //listaProdutos = compra.ProdutosCompra.ToList<Produto>;
                                }

                                if (listaProdutos == null)
                                    listaProdutos = new List<Produto>();
                                preencher_Tabela();
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
                            Session["compra"] = null;
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
                            if (produto.validade != null)
                            {
                                DateTime validade = (DateTime)produto.validade;
                                select_Produto.Items.Add(new ListItem(produto.descricao + " - Preço Compra: R$ " + produto.precoCompra.ToString("0.00") + " - Validade: " + validade.ToString("dd/MM/yyyy"), produto.idProduto.ToString()));
                            }
                            else
                            {
                                select_Produto.Items.Add(new ListItem(produto.descricao + " - Preço Compra: R$ " + produto.precoCompra.ToString("0.00"), produto.idProduto.ToString()));
                            }
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
                        txt_ProdutoPrecoCompra.ReadOnly = true;
                        txt_ProdutoPrecoVenda.ReadOnly = true;
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
                    if (select_Produto.SelectedValue != "")
                        idProduto = int.Parse(select_Produto.SelectedValue);
                    else
                        idProduto = 0;
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

        protected void btn_CadastrarProduto_Click(object sender, EventArgs e)
        {
            compra = null;
            Session["compra"] = null;
            compra.cnpjOficina = funcionario.cnpjOficina;
            compra.cpfFuncionario = funcionario.cpf;
            compra.idFornecedor = int.Parse(select_Fornecedores.SelectedValue);
        }

        private void preencher_Tabela()
        {
            tbl_Produtos.Rows.Clear();
            TableRow row;
            TableCell cell;
            Button btn;

            TableHeaderCell headerCell;
            TableHeaderRow header = new TableHeaderRow();

            headerCell = new TableHeaderCell();
            headerCell.Text = "Produto";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Marca";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Categoria";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Quantidade";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Preço Compra";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Preço Venda";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            headerCell = new TableHeaderCell();
            headerCell.Text = "Ações";
            headerCell.CssClass = "text-center";
            header.Cells.Add(headerCell);

            header.CssClass = "thead-light";

            tbl_Produtos.Rows.Add(header);

            if (listaProdutos != null)
            {
                foreach (Produto produto in listaProdutos)
                {
                    row = new TableRow();
                    //PRODUTO
                    cell = new TableCell();
                    cell.Text = produto.descricao;
                    cell.CssClass = "text-center align-middle";
                    row.Cells.Add(cell);
                    //MARCA
                    cell = new TableCell();
                    cell.Text = produto.marca;
                    cell.CssClass = "text-center align-middle";
                    row.Cells.Add(cell);
                    //CATEGORIA
                    cell = new TableCell();
                    cell.Text = produto.categoria;
                    cell.CssClass = "text-center align-middle";
                    row.Cells.Add(cell);
                    //QUANTIDADE
                    cell = new TableCell();
                    cell.Text = produto.quantidade.ToString();
                    cell.CssClass = "text-center align-middle";
                    row.Cells.Add(cell);
                    //PRECO COMPRA
                    cell = new TableCell();
                    cell.Text = "R$ " + produto.precoCompra.ToString("0.00");
                    cell.CssClass = "text-center align-middle";
                    row.Cells.Add(cell);
                    //PRECO VENDA
                    cell = new TableCell();
                    if (produto.precoVenda != null)
                    {
                        double dbl = (double)produto.precoVenda;
                        cell.Text = dbl.ToString("0.00");
                    }
                    else
                    {
                        cell.Text = "N/A";
                    }

                    cell.CssClass = "text-center align-middle";
                    row.Cells.Add(cell);
                    tbl_Produtos.Rows.Add(row);
                }
            }
        }

        protected void btn_AdicionarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_ProdutoQuantidade.Text != "")
                    if (int.Parse(txt_ProdutoQuantidade.Text) > 0)
                        using (var context = new DatabaseEntities())
                        {
                            Produto produto = context.Produto.Where(p => p.idProduto == idProduto).FirstOrDefault();
                            if (produto != null)
                            {
                                produto.quantidade = int.Parse(txt_ProdutoQuantidade.Text.Replace(".", ""));
                                if (txt_ProdutoPrecoCompra.Text != produto.precoCompra.ToString())
                                    produto.precoCompra = double.Parse(txt_ProdutoPrecoCompra.Text.Replace("R$", ""));
                                listaProdutos.Add(produto);
                                preencher_Tabela();
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