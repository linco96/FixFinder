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
        private static List<Produto> listaProdutos;
        private static int idProduto;
        private static Fornecedor fornecedor;
        private bool isRefresh;

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
                //verifica se e refresh
                if (!IsPostBack)
                {
                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();
                }
                else
                {
                    if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
                    {
                        isRefresh = true;
                    }
                    Session["SessionId"] = System.Guid.NewGuid().ToString();
                    ViewState["ViewStateId"] = Session["SessionId"].ToString();
                }

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
                                    preencher_CamposCompra(compra);
                                }

                                if (listaProdutos == null)
                                    listaProdutos = new List<Produto>();

                                pnl_Alert.Visible = false;
                                pnl_AlertProdutoDuplicado.Visible = false;

                                preencher_Tabela();

                                if (!IsPostBack)
                                {
                                    preencher_Fornecedores();
                                    preencher_Produto();
                                }

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
                                    txt_ProdutoQuantidade.ReadOnly = false;
                                }
                                lbl_Nome.Text = c.nome;
                                if (funcionario == null)
                                {
                                    pnl_Oficina.Visible = false;
                                    btn_CadastroOficina.Visible = true;

                                    List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(r => r.cpfCliente.Equals(c.cpf)).ToList();
                                    if (requisicoes.Count > 0)
                                    {
                                        pnl_Funcionario.Visible = true;
                                        badge_Requisicoes.InnerHtml = requisicoes.Count.ToString();
                                    }
                                    else
                                    {
                                        pnl_Funcionario.Visible = false;
                                    }
                                }
                                else
                                {
                                    pnl_Oficina.Visible = true;
                                    pnl_Funcionario.Visible = false;
                                    btn_CadastroOficina.Visible = false;
                                    lbl_Nome.Text += " | " + funcionario.Oficina.nome;
                                    if (funcionario.cargo.ToLower().Equals("gerente"))
                                    {
                                        btn_Configuracoes.Visible = true;
                                        btn_Funcionarios.Visible = true;
                                    }
                                    else
                                    {
                                        btn_Configuracoes.Visible = false;
                                        btn_Funcionarios.Visible = false;
                                    }
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

        private void preencher_CamposCompra(Compra compra)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    if (compra.cpfFuncionario != funcionario.cpf)
                    {
                        Session["compra"] = null;
                        Response.Redirect("home.aspx");
                    }

                    fornecedor = context.Fornecedor.Where(f => f.idFornecedor == compra.idFornecedor).FirstOrDefault();
                    if (fornecedor != null)
                    {
                        txt_FornecedorCNPJ.Text = fornecedor.cnpjFornecedor;
                        txt_FornecedorNome.Text = fornecedor.razaoSocial;
                        txt_FornecedorTelefone.Text = fornecedor.telefone;
                        txt_FornecedorEmail.Text = fornecedor.email;
                    }

                    listaProdutos = new List<Produto>();
                    Produto produto;

                    foreach (ProdutosCompra pCompra in compra.ProdutosCompra)
                    {
                        produto = context.Produto.Where(p => p.idProduto == pCompra.idProduto).FirstOrDefault();
                        if (produto != null)
                        {
                            produto.quantidade = pCompra.quantidade;
                            listaProdutos.Add(produto);
                        }
                    }
                    Session["compra"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
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
                        txt_ProdutoPrecoCompra.Text = produto.precoCompra.ToString("0.00");

                        if (produto.validade != null)
                        {
                            DateTime dt = (DateTime)produto.validade;
                            txt_ProdutoValidade.Text = dt.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txt_ProdutoValidade.Text = "";
                        }

                        if (produto.precoVenda != null)
                        {
                            double dbl = (double)produto.precoVenda;
                            txt_ProdutoPrecoVenda.Text = dbl.ToString("0.00");
                        }
                        else
                        {
                            txt_ProdutoPrecoVenda.Text = "";
                        }
                        btn_AdicionarProduto.Enabled = true;
                    }
                    else
                    {
                        txt_Produto.Text = "";
                        txt_ProdutoCategoria.Text = "";
                        txt_ProdutoMarca.Text = "";
                        txt_ProdutoQuantidadeAtual.Text = "";
                        txt_ProdutoPrecoCompra.Text = "";
                        txt_ProdutoPrecoVenda.Text = "";
                        txt_ProdutoValidade.Text = "";
                        btn_AdicionarProduto.Enabled = false;
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
            ProdutosCompra pCompra;

            compra = new Compra();
            compra.cnpjOficina = funcionario.cnpjOficina;
            compra.cpfFuncionario = funcionario.cpf;
            if (fornecedor != null)
                compra.idFornecedor = fornecedor.idFornecedor;

            if (listaProdutos != null && listaProdutos.Count > 0)
            {
                foreach (Produto p in listaProdutos)
                {
                    pCompra = new ProdutosCompra()
                    {
                        idProduto = p.idProduto,
                        idCompra = 0,
                        quantidade = p.quantidade
                    };
                    compra.ProdutosCompra.Add(pCompra);
                }
            }
            Session["compra"] = compra;
            Response.Redirect("produto_Cadastro.aspx", false);
        }

        private void preencher_Tabela()
        {
            TableRow row;
            TableCell cell;
            Button btn;
            double total = 0;

            tbl_Produtos.Rows.Clear();

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
            headerCell.Text = "Data de validade";
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
                    cell.CssClass = "text-center align-middle";
                    if (produto.precoVenda != null)
                    {
                        double dbl = (double)produto.precoVenda;
                        cell.Text = "R$ " + dbl.ToString("0.00");
                    }
                    else
                    {
                        cell.Text = "N/A";
                    }
                    row.Cells.Add(cell);

                    //VALIDADE
                    cell = new TableCell();
                    cell.CssClass = "text-center align-middle";
                    if (produto.validade != null)
                    {
                        DateTime dt = (DateTime)produto.validade;
                        cell.Text = dt.ToString("dd/MM/yyyy");
                    }
                    row.Cells.Add(cell);

                    //BOTAO REMOVER
                    cell = new TableCell();
                    cell.CssClass = "text-center align-middle";
                    btn = new Button();
                    btn.Text = "Remover";
                    btn.CssClass = "btn btn-danger";
                    btn.Attributes.Add("formnovalidate", "formnovalidate");
                    btn.Click += new EventHandler(btn_RemoverProduto_Click);
                    btn.CommandArgument = produto.idProduto.ToString();
                    btn.ID = "btnExcluir_" + produto.idProduto.ToString();
                    cell.Controls.Add(btn);
                    row.Cells.Add(cell);

                    tbl_Produtos.Rows.Add(row);
                }
            }

            foreach (Produto produto in listaProdutos)
            {
                total += produto.quantidade * produto.precoCompra;
            }
            lbl_TotalCompra.Text = "Total compra: R$ " + total.ToString("0.00");
        }

        protected void btn_RemoverProduto_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                foreach (Produto p in listaProdutos)
                {
                    if (p.idProduto == int.Parse(btn.CommandArgument))
                    {
                        listaProdutos.Remove(p);
                        ///have a
                        break;
                        //have a kitkat
                    }
                }
                preencher_Tabela();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btn_AdicionarProduto_Click(object sender, EventArgs e)
        {
            if (!isRefresh)
                try
                {
                    if (txt_ProdutoQuantidade.Text != "")
                    {
                        int qtd = int.Parse(txt_ProdutoQuantidade.Text.Replace(".", ""));
                        if (qtd > 0)
                        {
                            Produto produto;
                            using (var context = new DatabaseEntities())
                            {
                                produto = context.Produto.Where(p => p.idProduto == idProduto).FirstOrDefault();
                                if (listaProdutos.Any(p => p.idProduto == produto.idProduto))
                                {
                                    pnl_AlertProdutoDuplicado.Visible = true;
                                }
                                else
                                {
                                    pnl_AlertProdutoDuplicado.Visible = false;
                                    lbl_AlertProduto.Text = "";
                                    if (produto != null)
                                    {
                                        produto.quantidade = qtd;
                                        if (txt_ProdutoPrecoCompra.Text.Replace(".", "").Replace("R$", "") != produto.precoCompra.ToString())
                                            produto.precoCompra = double.Parse(txt_ProdutoPrecoCompra.Text.Replace("R$", ""));
                                        if (txt_ProdutoPrecoVenda.Text.Replace(".", "").Replace("R$", "") != produto.precoVenda.ToString())
                                            produto.precoVenda = double.Parse(txt_ProdutoPrecoVenda.Text.Replace("R$", ""));
                                        if (txt_ProdutoValidade.Text.ToUpper() != "")
                                        {
                                            produto.validade = DateTime.Parse(txt_ProdutoValidade.Text);
                                        }
                                        else
                                        {
                                            produto.validade = null;
                                        }
                                        //Response.Redirect(Request.RawUrl);
                                    }
                                    listaProdutos.Add(produto);
                                    preencher_Tabela();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
        }

        protected void btn_ConcluirCompra_Click(object sender, EventArgs e)
        {
            if (fornecedor == null)
            {
                pnl_Concluir.Visible = true;
                lbl_AlertConcluir.Text = "Selecione um fornecedor para concluir a compra";
            }
            else if (listaProdutos == null || listaProdutos.Count <= 0)
            {
                pnl_Concluir.Visible = true;
                lbl_AlertConcluir.Text = "Selecione ao menos 1 roduto para concluir a compra";
            }
            else if (funcionario == null)
            {
                Session["compra"] = null;
                Response.Redirect("home.aspx", false);
            }
            else if (fornecedor == null)
            {
                pnl_Concluir.Visible = true;
                lbl_AlertConcluir.Text = "Erro com fornecedor selecionado";
            }
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        compra = new Compra()
                        {
                            cnpjOficina = funcionario.cnpjOficina,
                            data = DateTime.Now,
                            idFornecedor = fornecedor.idFornecedor,
                            cpfFuncionario = funcionario.cpf
                        };
                        context.Compra.Add(compra);
                        context.SaveChanges();

                        ProdutosCompra pCompra;
                        //pnl_Concluir.Visible = false;
                        foreach (Produto produto in listaProdutos)
                        {
                            pCompra = new ProdutosCompra()
                            {
                                idCompra = compra.idCompra,
                                idProduto = produto.idProduto,
                                quantidade = produto.quantidade
                            };

                            context.ProdutosCompra.Add(pCompra);
                            context.SaveChanges();

                            //Se o produto ja tiver quantidade, tem que adicionar a quantidade do produto
                            context.Produto.Where(p => p.idProduto == produto.idProduto).FirstOrDefault().quantidade += produto.quantidade;
                            context.SaveChanges();
                        }

                        Session["compra"] = null;
                        listaProdutos.Clear();
                        fornecedor = null;
                        compra = null;
                        Response.Redirect("compra_Lista.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void btn_Voltar_Click(object sender, EventArgs e)
        {
            Session["compra"] = null;
            listaProdutos.Clear();
            fornecedor = null;
            Response.Redirect("compra_Lista.aspx", false);
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }
    }
}