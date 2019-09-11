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
    public partial class orcamento_ListaOficina : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    if (f == null)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else
                    {
                        preencherTabela();
                        if (Session["orcamento"] != null)
                            Session["orcamento"] = false;
                    }
                    lbl_Nome.Text = c.nome;
                    if (f == null)
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
                        lbl_Nome.Text += " | " + f.Oficina.nome;
                        if (f.cargo.ToLower().Equals("gerente"))
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
                    headerCell.Text = "Cliente";
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

                    List<Orcamento> orcamentos = context.Orcamento.Where(o => o.cnpjOficina.Equals(f.cnpjOficina)).ToList();
                    if (orcamentos.Count > 0)
                    {
                        foreach (Orcamento o in orcamentos)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = o.Cliente.nome;
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
                            if (o.status.Equals("Cancelado") || o.status.Equals("Rejeitado pelo cliente") || o.status.Equals("Rejeitado pela gerencia"))
                                cell.CssClass = "text-center text-danger align-middle";
                            else if (o.status.Equals("Concluído"))
                                cell.CssClass = "text-center text-success align-middle";
                            else
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

                            if (o.status.Equals("Aprovado"))
                            {
                                btn = new Button();
                                btn.Click += new EventHandler(btn_AlterarOrcamento_Click);
                                btn.ID = "btn_Alterar" + o.idOrcamento.ToString();
                                btn.Text = "Atualizar";
                                btn.CssClass = "btn btn-primary ml-1";
                                btn.CommandArgument = o.idOrcamento.ToString();
                                cell.Controls.Add(btn);
                            }

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
                    Button btn;
                    HtmlGenericControl h;
                    bool completo = true;

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

                        if (so.status.Equals("Pendente") || so.status.Equals("Em execução"))
                        {
                            completo = false;
                        }

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

                    if (!o.status.Equals("Concluído"))
                    {
                        if (o.status.Equals("Aprovação da gerencia pendente"))
                        {
                            if (f.cargo.ToLower().Equals("gerente"))
                            {
                                btn_Aceitar = new Button();
                                btn_Aceitar.Click += new EventHandler(btn_Aprovar_Click);
                                btn_Aceitar.ID = "btn_Aprovar" + o.idOrcamento.ToString();
                                btn_Aceitar.Text = "Aprovar";
                                btn_Aceitar.CssClass = "btn btn-success mr-2 mt-3";
                                btn_Aceitar.CommandArgument = o.idOrcamento.ToString();

                                btn_Rejeitar = new Button();
                                btn_Rejeitar.Click += new EventHandler(btn_Rejeitar_Click);
                                btn_Rejeitar.ID = "btn_Rejeitar" + o.idOrcamento.ToString();
                                btn_Rejeitar.Text = "Rejeitar";
                                btn_Rejeitar.CssClass = "btn btn-danger ml-2 mt-3";
                                btn_Rejeitar.CommandArgument = o.idOrcamento.ToString();

                                body.Controls.Add(btn_Aceitar);
                                body.Controls.Add(btn_Rejeitar);
                            }
                        }
                        else
                        {
                            if (o.status.Equals("Aprovado"))
                            {
                                btn = new Button();
                                btn.Click += new EventHandler(btn_AlterarOrcamento_Click);
                                btn.ID = "btn_AlterarCollapse" + o.idOrcamento.ToString();
                                btn.Text = "Atualizar";
                                btn.CssClass = "btn btn-primary mt-3";
                                btn.CommandArgument = o.idOrcamento.ToString();
                                body.Controls.Add(btn);

                                if (completo)
                                {
                                    btn = new Button();
                                    btn.Click += new EventHandler(btn_FinalizarOrcamento_Click);
                                    btn.ID = "btn_Finalizar" + o.idOrcamento.ToString();
                                    btn.Text = "Enviar para o pagamento";
                                    btn.CssClass = "btn btn-success ml-2 mt-3";
                                    btn.CommandArgument = o.idOrcamento.ToString();
                                    body.Controls.Add(btn);
                                }

                                btn = new Button();
                                btn.Click += new EventHandler(btn_CancelarOrcamento_Click);
                                btn.ID = "btn_Cancelar" + o.idOrcamento.ToString();
                                btn.Text = "Cancelar";
                                btn.CssClass = "btn btn-danger ml-2 mt-3";
                                btn.CommandArgument = o.idOrcamento.ToString();
                                body.Controls.Add(btn);
                            }
                            else if (o.status.Equals("Pagamento pendente") || o.status.Equals("Cancelado"))
                            {
                                btn = new Button();
                                btn.Click += new EventHandler(btn_ReabrirOrcamento_Click);
                                btn.ID = "btn_Reabrir" + o.idOrcamento.ToString();
                                btn.Text = "Reabrir";
                                btn.CssClass = "btn btn-warning mt-3";
                                btn.CommandArgument = o.idOrcamento.ToString();
                                body.Controls.Add(btn);
                            }
                        }
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

        protected void btn_Aprovar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Orcamento orcamento = context.Orcamento.Where(o => o.idOrcamento == id).FirstOrDefault();
                    orcamento.status = "Aprovação do cliente pendente";
                    context.SaveChanges();
                    preencherTabela();
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "O orçamento foi aprovado com sucesso";
                    pnl_Alert.Visible = true;
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
                    orcamento.status = "Rejeitado pela gerencia";
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
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "O orçamento foi rejeitado com sucesso";
                    pnl_Alert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_AlterarOrcamento_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Orcamento orcamento = context.Orcamento.Where(o => o.idOrcamento == id).FirstOrDefault();
                    Session["orcamento"] = orcamento;
                    Response.Redirect("orcamento_Editar.aspx", false);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_CancelarOrcamento_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Orcamento orcamento = context.Orcamento.Where(o => o.idOrcamento == id).FirstOrDefault();
                    orcamento.status = "Cancelado";
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
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "O orçamento foi cancelado com sucesso";
                    pnl_Alert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_ReabrirOrcamento_Click(object sender, EventArgs e)
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

                    List<ProdutosOrcamento> produtos = context.ProdutosOrcamento.Where(prod => prod.idOrcamento == id).ToList();
                    Produto p;
                    foreach (ProdutosOrcamento produto in produtos)
                    {
                        p = context.Produto.Where(prod => prod.idProduto == produto.idProduto).FirstOrDefault();
                        p.quantidade -= produto.quantidade;
                        context.SaveChanges();
                    }
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "O orçamento foi reaberto com sucesso";
                    pnl_Alert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_FinalizarOrcamento_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Orcamento orcamento = context.Orcamento.Where(o => o.idOrcamento == id).FirstOrDefault();
                    orcamento.status = "Pagamento pendente";
                    context.SaveChanges();
                    preencherTabela();
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "O orçamento foi enviado para o pagamento com sucesso";
                    pnl_Alert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_CadastrarOrcamento_Click(object sender, EventArgs e)
        {
            Response.Redirect("orcamento_cadastro.aspx", false);
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}