using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class compra_Lista : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];

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
                            if (context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault() == null)
                            {
                                Response.Redirect("home.aspx", false);
                            }
                            else
                            {
                                preencher_Compras();
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

        private void preencher_Compras()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    Fornecedor fornecedor;
                    List<ProdutosCompra> lista_pCompra;
                    int adicionados = 0;

                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;
                    TableRow row;
                    TableCell cell;
                    HtmlGenericControl btnExpandir;
                    HtmlGenericControl divProdutos;

                    tbl_Compras.Rows.Clear();

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-dark";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Fornecedor";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Data";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Ações";
                    headerRow.Cells.Add(headerCell);

                    tbl_Compras.Rows.Add(headerRow);

                    var query = context.Compra.Where(comp => comp.cnpjOficina.Equals(funcionario.cnpjOficina)).ToList();

                    foreach (Compra compra in query)
                    {
                        fornecedor = context.Fornecedor.Where(f => f.idFornecedor == compra.idFornecedor).FirstOrDefault();
                        lista_pCompra = context.ProdutosCompra.Where(p => p.idCompra == compra.idCompra).ToList();
                        if (fornecedor != null)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = fornecedor.razaoSocial;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = compra.data.ToString("yyyy-MM-dd");
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btnExpandir = new HtmlGenericControl("a");
                            btnExpandir.Attributes.Add("class", "btn btn-primary");
                            btnExpandir.Attributes.Add("data-toggle", "collapse");
                            btnExpandir.Attributes.Add("href", "#collapse_" + compra.idCompra);
                            btnExpandir.Attributes.Add("aria-expanded", "false");
                            btnExpandir.InnerText = "Expandir/Recolher";
                            cell.Controls.Add(btnExpandir);
                            row.Cells.Add(cell);

                            row = new TableRow();
                            row.BorderStyle = BorderStyle.None;

                            cell = new TableCell();
                            cell.ColumnSpan = 3;
                            cell.CssClass = "p-0";
                            divProdutos = new HtmlGenericControl("div");
                            divProdutos.Attributes.Add("class", "collapse");
                            divProdutos.Attributes.Add("id", "collapse_" + compra.idCompra);
                            divProdutos.Controls.Add(addCard(lista_pCompra));
                            cell.Controls.Add(divProdutos);
                            row.Cells.Add(cell);

                            tbl_Compras.Rows.Add(row);
                            adicionados += 1;
                        }
                    }
                    if (adicionados == 0)
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.ColumnSpan = 3;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        cell.Text = "Nenhuma compra cadastrada";
                        row.Cells.Add(cell);
                        tbl_Compras.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private Panel addCard(List<ProdutosCompra> lista_pCompra)
        {
            try
            {
                Panel card;
                Panel body;
                using (var context = new DatabaseEntities())
                {
                    Table tblProdutos;
                    HtmlGenericControl title;
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;
                    Produto produto;
                    TableRow row;
                    TableCell cell;

                    card = new Panel();
                    card.CssClass = "card text-center bg-light p-3";
                    card.BorderStyle = BorderStyle.None;

                    body = new Panel();
                    body.CssClass = "card-body";

                    title = new HtmlGenericControl("h3");
                    title.Attributes.Add("class", "card-title text-primary");
                    title.InnerText = "Produtos";
                    body.Controls.Add(title);

                    tblProdutos = new Table();
                    tblProdutos.CssClass = "table table-bordered rounded-lg bg-white";

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-white";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Produto";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Quantidade";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Preço Compra";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Marca";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Validade";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Categoria";
                    headerRow.Cells.Add(headerCell);

                    tblProdutos.Rows.Add(headerRow);

                    foreach (ProdutosCompra pCompra in lista_pCompra)
                    {
                        produto = context.Produto.Where(p => p.idProduto == pCompra.idProduto).FirstOrDefault();
                        if (produto != null)
                        {
                            row = new TableRow();
                            //Produto
                            cell = new TableCell();
                            cell.Text = produto.descricao;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //Quantidade
                            cell = new TableCell();
                            cell.Text = pCompra.quantidade.ToString();
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //Preco compra
                            cell = new TableCell();
                            cell.Text = "R$ " + produto.precoCompra.ToString("0.00");
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //Marca
                            cell = new TableCell();
                            cell.Text = produto.marca;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //Validade
                            cell = new TableCell();
                            if (produto.validade != null)
                            {
                                DateTime dt = (DateTime)produto.validade;
                                cell.Text = dt.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                cell.Text = "-";
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //Categoria
                            cell = new TableCell();
                            cell.Text = produto.categoria;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            tblProdutos.Rows.Add(row);
                        }
                    }
                    body.Controls.Add(tblProdutos);
                }
                card.Controls.Add(body);
                return card;
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return null;
            }
        }
    }
}