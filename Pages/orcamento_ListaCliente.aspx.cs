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
    public partial class orcamento_ListaCliente : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                if (Session["orcamento"] != null)
                    Session["orcamento"] = null;
                preencherTabela();
            }
        }

        protected void preencherTabela()
        {
            try
            {
                tbl_Orcamentos.Rows.Clear();

                using (DatabaseEntities context = new DatabaseEntities())
                {
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;
                    TableRow row;
                    TableCell cell;
                    Button btn;
                    HtmlGenericControl btnExpandir;
                    HtmlGenericControl divOrcamento;

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-dark";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Oficina";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Veículo";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Valor";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Data";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Status";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Ações";
                    headerRow.Cells.Add(headerCell);

                    tbl_Orcamentos.Rows.Add(headerRow);

                    List<Orcamento> orcamentos = context.Orcamento.Where(o => o.cpfCliente.Equals(c.cpf)).ToList();
                    if (orcamentos.Count > 0)
                    {
                        foreach (Orcamento o in orcamentos)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = o.Oficina.nome;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = o.Veiculo.modelo + " - " + o.Veiculo.placa;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = "R$ " + o.valor.ToString("0.00");
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = o.data.ToString("dd/MM/yyyy");
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = o.status;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btnExpandir = new HtmlGenericControl("a");
                            btnExpandir.Attributes.Add("class", "btn btn-info");
                            btnExpandir.Attributes.Add("data-toggle", "collapse");
                            btnExpandir.Attributes.Add("href", "#collapse_" + o.idOrcamento);
                            btnExpandir.Attributes.Add("aria-expanded", "false");
                            btnExpandir.InnerText = "Expandir/Retrair";
                            cell.Controls.Add(btnExpandir);
                            row.Cells.Add(cell);

                            tbl_Orcamentos.Rows.Add(row);

                            row = new TableRow();
                            row.BorderStyle = BorderStyle.None;
                            cell = new TableCell();
                            cell.ColumnSpan = 6;
                            cell.CssClass = "p-0";
                            divOrcamento = new HtmlGenericControl("div");
                            divOrcamento.Attributes.Add("class", "collapse");
                            divOrcamento.Attributes.Add("id", "collapse_" + o.idOrcamento);
                            divOrcamento.Controls.Add(criarCard(o));
                            cell.Controls.Add(divOrcamento);
                            row.Cells.Add(cell);

                            tbl_Orcamentos.Rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Você não tem nenhum orçamento";
                        cell.ColumnSpan = 6;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        row.Cells.Add(cell);

                        tbl_Orcamentos.Rows.Add(row);
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

        protected Panel criarCard(Orcamento o)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Panel card;
                    Panel body;
                    Table tblServicos;
                    Table tblProdutos;
                    TableRow row;
                    TableCell cell;
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;
                    Button btn_Aceitar;
                    Button btn_Rejeitar;
                    HtmlGenericControl h;

                    card = new Panel();
                    card.CssClass = "card text-center bg-light p-3";
                    card.BorderStyle = BorderStyle.None;

                    body = new Panel();
                    body.CssClass = "card-body";

                    //ADICIONAR TABELA DE SERVIÇOS
                    h = new HtmlGenericControl("h3");
                    h.Attributes.Add("class", "card-title text-primary");
                    h.InnerText = "Serviços";
                    body.Controls.Add(h);

                    tblServicos = new Table();
                    tblServicos.CssClass = "table table-bordered rounded-lg bg-white";

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-white";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Descrição";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center w-25";
                    headerCell.Text = "Preço";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center w-25";
                    headerCell.Text = "Status";
                    headerRow.Cells.Add(headerCell);

                    tblServicos.Rows.Add(headerRow);

                    foreach (ServicosOrcamento so in context.ServicosOrcamento.Where(servico => servico.idOrcamento == o.idOrcamento).ToList())
                    {
                        row = new TableRow();

                        cell = new TableCell();
                        cell.Text = so.Servico.descricao;
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.Text = "R$ " + so.Servico.valor.ToString("0.00");
                        cell.CssClass = "text-center align-middle w-25";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.Text = so.status;
                        cell.CssClass = "text-center align-middle w-25";
                        row.Cells.Add(cell);

                        tblServicos.Rows.Add(row);
                    }
                    body.Controls.Add(tblServicos);

                    //ADICIONAR TABELA DE PRODUTOS
                    List<ProdutosOrcamento> produtosOrcamento = context.ProdutosOrcamento.Where(po => po.idOrcamento == o.idOrcamento).ToList();
                    if (produtosOrcamento.Count > 0)
                    {
                        h = new HtmlGenericControl("h3");
                        h.Attributes.Add("class", "card-title text-primary pt-3");
                        h.InnerText = "Produtos";
                        body.Controls.Add(h);

                        tblProdutos = new Table();
                        tblProdutos.CssClass = "table table-bordered rounded-lg bg-white";

                        headerRow = new TableHeaderRow();
                        headerRow.CssClass = "thead-white";

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center w-75";
                        headerCell.Text = "Descrição";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center w-25";
                        headerCell.Text = "Preço";
                        headerRow.Cells.Add(headerCell);

                        tblProdutos.Rows.Add(headerRow);

                        Double precoVenda;
                        foreach (ProdutosOrcamento po in produtosOrcamento)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = po.Produto.descricao + " " + po.Produto.marca;
                            cell.CssClass = "text-center align-middle w-75";
                            row.Cells.Add(cell);

                            if (po.Produto.precoVenda == null)
                            {
                                precoVenda = 0;
                            }
                            else
                            {
                                precoVenda = (Double)po.Produto.precoVenda;
                            }
                            cell = new TableCell();
                            cell.Text = "R$ " + precoVenda.ToString("0.00");
                            cell.CssClass = "text-center align-middle w-25";
                            row.Cells.Add(cell);

                            tblProdutos.Rows.Add(row);
                        }
                        body.Controls.Add(tblProdutos);
                    }

                    if (o.status.Equals("Aprovação do cliente pendente"))
                    {
                        btn_Aceitar = new Button();
                        btn_Aceitar.Click += new EventHandler(btn_Aprovar_Click);
                        btn_Aceitar.ID = "btn_Aprovar" + o.idOrcamento.ToString();
                        btn_Aceitar.Text = "Aprovar";
                        btn_Aceitar.CssClass = "btn btn-success mr-1 mt-3";
                        btn_Aceitar.CommandArgument = o.idOrcamento.ToString();

                        btn_Rejeitar = new Button();
                        btn_Rejeitar.Click += new EventHandler(btn_Rejeitar_Click);
                        btn_Rejeitar.ID = "btn_Rejeitar" + o.idOrcamento.ToString();
                        btn_Rejeitar.Text = "Rejeitar";
                        btn_Rejeitar.CssClass = "btn btn-danger ml-1 mt-3";
                        btn_Rejeitar.CommandArgument = o.idOrcamento.ToString();

                        body.Controls.Add(btn_Aceitar);
                        body.Controls.Add(btn_Rejeitar);
                    }
                    else if (o.status.Equals("Concluído"))
                    {
                        Button btn = new Button();
                        btn.Click += new EventHandler(btn_Avaliar_Click);
                        btn.ID = "btn_Rejeitar" + o.idOrcamento.ToString();
                        btn.Text = "Rejeitar";
                        btn.CssClass = "btn btn-danger ml-1 mt-3";
                        btn.CommandArgument = o.idOrcamento.ToString();
                    }

                    card.Controls.Add(body);

                    return card;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
                return null;
            }
        }

        protected void btn_Avaliar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                Response.Redirect("orcamento_Avaliar.aspx", false);
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Aprovar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Orcamento orcamento = context.Orcamento.Where(o => o.idOrcamento == id).FirstOrDefault();
                    orcamento.status = "Aprovado";
                    context.SaveChanges();
                    preencherTabela();
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Rejeitar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Orcamento orcamento = context.Orcamento.Where(o => o.idOrcamento == id).FirstOrDefault();
                    orcamento.status = "Rejeitado pelo cliente";
                    context.SaveChanges();
                    preencherTabela();

                    List<ProdutosOrcamento> produtos = context.ProdutosOrcamento.Where(prod => prod.idOrcamento == id).ToList();
                    Produto p;
                    foreach (ProdutosOrcamento produto in produtos)
                    {
                        p = context.Produto.Where(prod => prod.idProduto == produto.idProduto).FirstOrDefault();
                        p.quantidade += produto.quantidade;
                        context.SaveChanges();
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
    }
}