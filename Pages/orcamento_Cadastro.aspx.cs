using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class orcamento_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private static List<Servico> servicosSelecionados;
        private static Dictionary<Produto, int> produtosSelecionados;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["Usuario"];
            if (c != null)
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    c = context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).FirstOrDefault();
                    Funcionario f = c.Funcionario;
                    if (f == null)
                        Response.Redirect("home.aspx", false);
                    else if (f.Oficina == null)
                        Response.Redirect("home.aspx", false);
                    else
                    {
                        if (produtosSelecionados == null)
                            produtosSelecionados = new Dictionary<Produto, int>();
                        if (servicosSelecionados == null)
                            servicosSelecionados = new List<Servico>();
                        carregarTabelaServicos();
                        carregarTabelaProdutos();
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
            else
            {
                Response.Redirect("login.aspx", false);
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            carregarSelectServicos();
            carregarSelectProdutos();
        }

        protected void carregarSelectServicos()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<Servico> servicos = context.Servico.Where(servico => servico.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();
                    ListItem item;

                    if (servicos.Count > 0)
                    {
                        if (txt_ServicoSelecionado.Items.Count > 0)
                            txt_ServicoSelecionado.Items.Clear();
                        foreach (Servico s in servicos)
                        {
                            item = new ListItem();
                            item.Text = s.descricao + " - R$ " + s.valor.ToString("0.00");
                            item.Value = s.idServico.ToString();
                            txt_ServicoSelecionado.Items.Add(item);
                            txt_ServicoSelecionado.Enabled = true;
                        }
                        foreach (Servico s in servicosSelecionados)
                        {
                            item = txt_ServicoSelecionado.Items.FindByValue(s.idServico.ToString());
                            if (item != null)
                            {
                                txt_ServicoSelecionado.Items.Remove(item);
                            }
                        }
                        if (txt_ServicoSelecionado.Items.Count == 0)
                        {
                            item = new ListItem();
                            item.Text = "Todos os serviços cadastrados já foram adicionados";
                            item.Value = "-";
                            txt_ServicoSelecionado.Items.Add(item);
                            txt_ServicoSelecionado.Enabled = false;
                        }
                    }
                    else
                    {
                        item = new ListItem();
                        item.Text = "Nenhum serviço encontrado";
                        item.Value = "-";
                        txt_ServicoSelecionado.Items.Add(item);
                        txt_ServicoSelecionado.Enabled = false;
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

        protected void carregarSelectProdutos()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<Produto> produtos = context.Produto.Where(produto => produto.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();
                    ListItem item;

                    if (produtos.Count > 0)
                    {
                        if (txt_ProdutoSelecionado.Items.Count > 0)
                            txt_ProdutoSelecionado.Items.Clear();
                        double precoVenda;
                        DateTime validade;
                        foreach (Produto p in produtos)
                        {
                            item = new ListItem();
                            if (p.precoVenda != null)
                            {
                                precoVenda = (Double)p.precoVenda;
                            }
                            else
                            {
                                precoVenda = 0;
                            }
                            if (p.validade == null)
                            {
                                item.Text = p.descricao + " " + p.marca + " - R$ " + precoVenda.ToString("0.00") + " - Estoque: " + p.quantidade;
                            }
                            else
                            {
                                validade = (DateTime)p.validade;
                                item.Text = p.descricao + " " + p.marca + " - R$ " + precoVenda.ToString("0.00") + " - Estoque: " + p.quantidade + " (Vence em " + validade.ToString("dd/MM/yyyy") + ")";
                            }
                            item.Value = p.idProduto.ToString();
                            txt_ProdutoSelecionado.Items.Add(item);
                            txt_ProdutoSelecionado.Enabled = true;
                        }
                        foreach (Produto p in produtosSelecionados.Keys)
                        {
                            item = txt_ProdutoSelecionado.Items.FindByValue(p.idProduto.ToString());
                            if (item != null)
                            {
                                txt_ProdutoSelecionado.Items.Remove(item);
                            }
                        }
                        if (txt_ProdutoSelecionado.Items.Count == 0)
                        {
                            item = new ListItem();
                            item.Text = "Todos os produtos cadastrados já foram adicionados";
                            item.Value = "-";
                            txt_ProdutoSelecionado.Items.Add(item);
                            txt_ProdutoSelecionado.Enabled = false;
                        }
                    }
                    else
                    {
                        item = new ListItem();
                        item.Text = "Nenhum produto encontrado";
                        item.Value = "-";
                        txt_ProdutoSelecionado.Items.Add(item);
                        txt_ProdutoSelecionado.Enabled = false;
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

        protected void carregarTabelaServicos()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    TableRow row;
                    TableHeaderRow headerRow;
                    TableCell cell;
                    TableHeaderCell headerCell;
                    Button btn;

                    if (servicosSelecionados.Count > 0)
                    {
                        headerRow = new TableHeaderRow();
                        headerRow.CssClass = "thead-light";

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Descrição";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Valor";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Ações";
                        headerRow.Cells.Add(headerCell);

                        tbl_Servicos.Rows.Add(headerRow);

                        foreach (Servico s in servicosSelecionados)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = s.descricao;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = "R$ " + s.valor.ToString("0.00");
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Text = "Remover";
                            btn.Click += new EventHandler(btn_RemoverServico_Click);
                            btn.ID = "btn_Servico" + s.idServico.ToString();
                            btn.CommandArgument = s.idServico.ToString();
                            btn.CssClass = "btn btn-danger";
                            cell.Controls.Add(btn);
                            row.Cells.Add(cell);

                            tbl_Servicos.Rows.Add(row);
                        }
                        tbl_Servicos.Visible = true;
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

        protected void carregarTabelaProdutos()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    TableRow row;
                    TableHeaderRow headerRow;
                    TableCell cell;
                    TableHeaderCell headerCell;
                    Button btn;

                    if (produtosSelecionados.Count > 0)
                    {
                        headerRow = new TableHeaderRow();
                        headerRow.CssClass = "thead-light";

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Descrição";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Marca";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Preço";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Validade";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Quantidade em<br />estoque";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Quantidade<br />selecionada";
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();
                        headerCell.CssClass = "text-center";
                        headerCell.Text = "Ações";
                        headerRow.Cells.Add(headerCell);

                        tbl_Produtos.Rows.Add(headerRow);

                        Double precoVenda;
                        DateTime validade;

                        foreach (Produto p in produtosSelecionados.Keys)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = p.descricao;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = p.marca;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            if (p.precoVenda == null)
                            {
                                cell.Text = "-";
                            }
                            else
                            {
                                precoVenda = (Double)p.precoVenda;
                                cell.Text = "R$ " + precoVenda.ToString("0.00");
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            if (p.validade == null)
                            {
                                cell.Text = "-";
                            }
                            else
                            {
                                validade = (DateTime)p.validade;
                                cell.Text = validade.ToString("dd/MM/yyyy");
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = p.quantidade.ToString();
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = produtosSelecionados[p].ToString();
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Text = "Remover";
                            btn.Click += new EventHandler(btn_RemoverProduto_Click);
                            btn.ID = "btn_Produto" + p.idProduto.ToString();
                            btn.CommandArgument = p.idProduto.ToString();
                            btn.CssClass = "btn btn-danger";
                            cell.Controls.Add(btn);
                            row.Cells.Add(cell);

                            tbl_Produtos.Rows.Add(row);
                        }
                        tbl_Produtos.Visible = true;
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

        protected void adicionarTabelaServicos(Servico s)
        {
            try
            {
                if (tbl_Servicos.Rows.Count == 0)
                {
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-light";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Descrição";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Valor";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Ações";
                    headerRow.Cells.Add(headerCell);

                    tbl_Servicos.Rows.Add(headerRow);
                }

                TableRow row;
                TableCell cell;
                Button btn;

                row = new TableRow();

                cell = new TableCell();
                cell.Text = s.descricao;
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "R$ " + s.valor.ToString("0.00");
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "text-center align-middle";
                btn = new Button();
                btn.Text = "Remover";
                btn.Click += new EventHandler(btn_RemoverServico_Click);
                btn.ID = "btn_Servico" + s.idServico.ToString();
                btn.CommandArgument = s.idServico.ToString();
                btn.CssClass = "btn btn-danger";
                cell.Controls.Add(btn);
                row.Cells.Add(cell);

                tbl_Servicos.Rows.Add(row);
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void removerTabelaServicos(Servico s)
        {
            try
            {
                if (servicosSelecionados.Count == 0)
                {
                    tbl_Servicos.Rows.Clear();
                }
                else
                {
                    Button btn;
                    foreach (TableRow row in tbl_Servicos.Rows)
                    {
                        if (row.Cells[2].Controls.Count > 0)
                        {
                            btn = (Button)row.Cells[2].Controls[0];
                            if (int.Parse(btn.CommandArgument) == s.idServico)
                                tbl_Servicos.Rows.Remove(row);
                        }
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

        protected void adicionarTabelaProdutos(Produto p)
        {
            try
            {
                if (tbl_Produtos.Rows.Count == 0)
                {
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-light";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Descrição";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Marca";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Preço";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Validade";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Quantidade em<br />estoque";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Quantidade<br />selecionada";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Ações";
                    headerRow.Cells.Add(headerCell);

                    tbl_Produtos.Rows.Add(headerRow);
                }

                TableRow row;
                TableCell cell;
                Button btn;

                Double precoVenda;
                DateTime validade;

                row = new TableRow();

                cell = new TableCell();
                cell.Text = p.descricao;
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = p.marca;
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                if (p.precoVenda == null)
                {
                    cell.Text = "-";
                }
                else
                {
                    precoVenda = (Double)p.precoVenda;
                    cell.Text = "R$ " + precoVenda.ToString("0.00");
                }
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                if (p.validade == null)
                {
                    cell.Text = "-";
                }
                else
                {
                    validade = (DateTime)p.validade;
                    cell.Text = validade.ToString("dd/MM/yyyy");
                }
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = p.quantidade.ToString();
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = produtosSelecionados[p].ToString();
                cell.CssClass = "text-center align-middle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "text-center align-middle";
                btn = new Button();
                btn.Text = "Remover";
                btn.Click += new EventHandler(btn_RemoverProduto_Click);
                btn.ID = "btn_Produto" + p.idProduto.ToString();
                btn.CommandArgument = p.idProduto.ToString();
                btn.CssClass = "btn btn-danger";
                cell.Controls.Add(btn);
                row.Cells.Add(cell);

                tbl_Produtos.Rows.Add(row);
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void removerTabelaProdutos(Produto p)
        {
            try
            {
                if (produtosSelecionados.Count == 0)
                {
                    tbl_Produtos.Rows.Clear();
                }
                else
                {
                    Button btn;
                    foreach (TableRow row in tbl_Produtos.Rows)
                    {
                        if (row.Cells[6].Controls.Count > 0)
                        {
                            btn = (Button)row.Cells[6].Controls[0];
                            if (int.Parse(btn.CommandArgument) == p.idProduto)
                                tbl_Produtos.Rows.Remove(row);
                        }
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

        protected void btn_AdicionarServico_Click(object sender, EventArgs e)
        {
            if (!txt_ServicoSelecionado.SelectedValue.Equals("-"))
                try
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        int id = int.Parse(txt_ServicoSelecionado.SelectedValue);
                        Servico s = context.Servico.Where(servico => servico.idServico == id).FirstOrDefault();
                        servicosSelecionados.Add(s);
                        pnl_Alert.Visible = false;
                        adicionarTabelaServicos(s);
                        atualizarTotal();
                    }
                }
                catch (Exception ex)
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                    pnl_Alert.Visible = true;
                }
        }

        protected void btn_AdicionarProduto_Click(object sender, EventArgs e)
        {
            if (!txt_ProdutoSelecionado.SelectedValue.Equals("-"))
            {
                try
                {
                    if (txt_ProdutoQuantidade.Text.Length > 0)
                    {
                        int quantidade = int.Parse(txt_ProdutoQuantidade.Text);
                        if (quantidade > 0)
                        {
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                int id = int.Parse(txt_ProdutoSelecionado.SelectedValue);
                                Produto p = context.Produto.Where(produto => produto.idProduto == id).FirstOrDefault();
                                if (quantidade > p.quantidade)
                                {
                                    pnl_Alert.CssClass = "alert alert-danger";
                                    lbl_Alert.Text = "Informe uma quantidade válida";
                                    pnl_Alert.Visible = true;
                                }
                                else
                                {
                                    produtosSelecionados.Add(p, quantidade);
                                    pnl_Alert.Visible = false;
                                    adicionarTabelaProdutos(p);
                                    atualizarTotal();
                                }
                            }
                        }
                        else
                        {
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Informe uma quantidade válida";
                            pnl_Alert.Visible = true;
                        }
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Informe uma quantidade válida";
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
        }

        protected void btn_RemoverServico_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int id;
            try
            {
                foreach (Servico s in servicosSelecionados)
                {
                    id = int.Parse(btn.CommandArgument);
                    if (s.idServico == id)
                    {
                        servicosSelecionados.Remove(s);
                        removerTabelaServicos(s);
                        break;
                    }
                }
                pnl_Alert.Visible = false;
                atualizarTotal();
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_RemoverProduto_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int id;
            try
            {
                foreach (Produto p in produtosSelecionados.Keys)
                {
                    id = int.Parse(btn.CommandArgument);
                    if (p.idProduto == id)
                    {
                        produtosSelecionados.Remove(p);
                        removerTabelaProdutos(p);
                        break;
                    }
                }
                pnl_Alert.Visible = false;
                atualizarTotal();
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_NovoServico_Click(object sender, EventArgs e)
        {
            form_CadastroServico.Visible = true;
            btn_NovoServico.Visible = false;
        }

        protected void btn_CancelarCadastroServico_Click(object sender, EventArgs e)
        {
            form_CadastroServico.Visible = false;
            btn_NovoServico.Visible = true;
            txt_DescricaoServico.Text = "";
            txt_ValorServico.Text = "";
        }

        protected void btn_CadastrarServico_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Funcionario f = context.Funcionario.Where(funcionario => funcionario.cpf.Equals(c.cpf)).FirstOrDefault();
                    Servico s = new Servico
                    {
                        descricao = txt_DescricaoServico.Text,
                        valor = Double.Parse(txt_ValorServico.Text),
                        cnpjOficina = f.cnpjOficina
                    };
                    context.Servico.Add(s);
                    context.SaveChanges();
                    servicosSelecionados.Add(s);
                    txt_DescricaoServico.Text = "";
                    txt_ValorServico.Text = "";
                    form_CadastroServico.Visible = false;
                    btn_NovoServico.Visible = true;
                    pnl_Alert.Visible = false;
                    adicionarTabelaServicos(s);
                    atualizarTotal();
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_NovoProduto_Click(object sender, EventArgs e)
        {
            form_CadastroProduto.Visible = true;
            btn_NovoProduto.Visible = false;
        }

        protected void btn_CadastrarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txt_QuantidadeAtualProduto.Text) > 0 && int.Parse(txt_QuantidadeUtilizadaProduto.Text) > 0)
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        Funcionario f = context.Funcionario.Where(funcionario => funcionario.cpf.Equals(c.cpf)).FirstOrDefault();
                        DateTime? validade = null;

                        if (txt_ValidadeProduto.Text.Length > 0)
                            validade = DateTime.Parse(txt_ValidadeProduto.Text);

                        Produto p = new Produto()
                        {
                            descricao = txt_DescricaoProduto.Text,
                            quantidade = int.Parse(txt_QuantidadeAtualProduto.Text),
                            marca = txt_MarcaProduto.Text,
                            categoria = txt_CategoriaProduto.Text,
                            precoCompra = Double.Parse(txt_PrecoCompraProduto.Text),
                            precoVenda = Double.Parse(txt_PrecoVendaProduto.Text),
                            validade = validade,
                            ativo = 1,
                            cnpjOficina = f.cnpjOficina
                        };

                        context.Produto.Add(p);
                        context.SaveChanges();

                        produtosSelecionados.Add(p, int.Parse(txt_QuantidadeUtilizadaProduto.Text));
                        txt_DescricaoProduto.Text = "";
                        txt_QuantidadeAtualProduto.Text = "";
                        txt_MarcaProduto.Text = "";
                        txt_CategoriaProduto.Text = "";
                        txt_PrecoCompraProduto.Text = "";
                        txt_PrecoVendaProduto.Text = "";
                        txt_ValidadeProduto.Text = "";
                        txt_QuantidadeUtilizadaProduto.Text = "";
                        form_CadastroProduto.Visible = false;
                        btn_NovoProduto.Visible = true;

                        pnl_Alert.Visible = false;

                        adicionarTabelaProdutos(p);
                        atualizarTotal();
                    }
                }
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Insira quantidades válidas";
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

        protected void btn_CancelarCadastroProduto_Click(object sender, EventArgs e)
        {
            form_CadastroProduto.Visible = false;
            btn_NovoProduto.Visible = true;
            txt_DescricaoProduto.Text = "";
            txt_QuantidadeAtualProduto.Text = "";
            txt_MarcaProduto.Text = "";
            txt_CategoriaProduto.Text = "";
            txt_PrecoCompraProduto.Text = "";
            txt_PrecoVendaProduto.Text = "";
            txt_ValidadeProduto.Text = "";
            txt_QuantidadeUtilizadaProduto.Text = "";
        }

        protected void btn_CarregarCliente_Click(object sender, EventArgs e)
        {
            txt_Nome.Text = "";
            if (txt_Veiculo.Items.Count > 0)
            {
                txt_Veiculo.Items.Clear();
                txt_Veiculo.Enabled = false;
            }
            if (txt_CPF.Text.Length < 14)
            {
                alert_CPF.InnerText = "CPF inválido";
                alert_CPF.Visible = true;
            }
            else
            {
                try
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        Cliente cliente = context.Cliente.Where(cl => cl.cpf.Equals(txt_CPF.Text.Replace(".", "").Replace("-", ""))).FirstOrDefault();
                        if (cliente == null)
                        {
                            alert_CPF.InnerText = "CPF inválido";
                            alert_CPF.Visible = true;
                            txt_CPF.Text = "";
                        }
                        else if (cliente.cpf.Equals(c.cpf))
                        {
                            alert_CPF.InnerText = "CPF inválido";
                            alert_CPF.Visible = true;
                            txt_CPF.Text = "";
                        }
                        else
                        {
                            List<Veiculo> veiculos = context.Veiculo.Where(v => v.cpfCliente.Equals(cliente.cpf)).ToList();
                            ListItem item;
                            if (veiculos.Count == 0)
                            {
                                alert_CPF.InnerText = "Este usuário não possui veículos cadastrados";
                                alert_CPF.Visible = true;
                            }
                            else
                            {
                                if (txt_Veiculo.Items.Count > 0)
                                    txt_Veiculo.Items.Clear();

                                foreach (Veiculo v in veiculos)
                                {
                                    item = new ListItem();
                                    item.Text = v.marca + " - " + v.placa;
                                    item.Value = v.idVeiculo.ToString();
                                    txt_Veiculo.Items.Add(item);
                                }
                                txt_Veiculo.Enabled = true;
                                txt_Nome.Text = cliente.nome;
                                txt_CPF.Text = cliente.cpf;
                                alert_CPF.Visible = false;
                            }
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

        protected void btn_AtualizarTotal_Click(object sender, EventArgs e)
        {
            atualizarTotal();
        }

        protected void txt_DescontoAcrescimo_SelectedIndexChanged(object sender, EventArgs e)
        {
            atualizarTotal();
        }

        protected double atualizarTotal()
        {
            double total = 0;
            foreach (Servico s in servicosSelecionados)
            {
                total += s.valor;
            }
            foreach (Produto p in produtosSelecionados.Keys)
            {
                if (p.precoVenda != null)
                    total += ((Double)p.precoVenda) * produtosSelecionados[p];
            }
            if (txt_Desconto.Text.Length > 0)
            {
                if (txt_DescontoAcrescimo.SelectedValue == "d")
                {
                    total -= Double.Parse(txt_Desconto.Text);
                }
                else
                {
                    total += Double.Parse(txt_Desconto.Text);
                }
            }
            if (total < 0)
                lbl_ValorTotal.Attributes.Add("class", "mt-2 text-center text-danger");
            else
                lbl_ValorTotal.Attributes.Add("class", "mt-2 text-center text-primary");
            lbl_ValorTotal.InnerText = "Valor total: R$ " + total.ToString("0.00");
            return total;
        }

        protected void clearForm()
        {
            txt_CPF.Text = "";
            txt_Nome.Text = "";
            txt_Veiculo.Items.Clear();
            txt_Veiculo.Enabled = false;
            servicosSelecionados.Clear();
            tbl_Servicos.Rows.Clear();
            produtosSelecionados.Clear();
            tbl_Produtos.Rows.Clear();
            txt_Desconto.Text = "";
            lbl_ValorTotal.InnerText = "Valor total: R$ 0,00";
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
            if (servicosSelecionados.Count == 0)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Insira no mínimo 1 serviço";
                pnl_Alert.Visible = true;
            }
            else
            {
                if (txt_Veiculo.Enabled == false)
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Informe um cliente com no mínimo 1 veículo cadastrado";
                    pnl_Alert.Visible = true;
                }
                else
                {
                    try
                    {
                        using (DatabaseEntities context = new DatabaseEntities())
                        {
                            int id = int.Parse(txt_Veiculo.SelectedValue);
                            Veiculo veiculo = context.Veiculo.Where(v => v.idVeiculo == id).FirstOrDefault();
                            Cliente cliente = veiculo.Cliente;
                            Funcionario funcionario = context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();
                            Oficina oficina = funcionario.Oficina;
                            DateTime data = DateTime.Now;
                            Double total = atualizarTotal();
                            String status = "Aprovação da gerencia pendente";
                            Produto prod;

                            Orcamento orcamento = new Orcamento()
                            {
                                valor = total,
                                data = data,
                                status = status,
                                cpfFuncionario = funcionario.cpf,
                                cnpjOficina = oficina.cnpj,
                                idVeiculo = veiculo.idVeiculo,
                                cpfCliente = cliente.cpf,
                            };

                            context.Orcamento.Add(orcamento);
                            context.SaveChanges();

                            ServicosOrcamento so;
                            foreach (Servico s in servicosSelecionados)
                            {
                                so = new ServicosOrcamento()
                                {
                                    idOrcamento = orcamento.idOrcamento,
                                    idServico = s.idServico,
                                    status = "Pendente"
                                };
                                context.ServicosOrcamento.Add(so);
                                context.SaveChanges();
                            }
                            if (produtosSelecionados.Count > 0)
                            {
                                ProdutosOrcamento po;
                                foreach (Produto p in produtosSelecionados.Keys)
                                {
                                    po = new ProdutosOrcamento()
                                    {
                                        idOrcamento = orcamento.idOrcamento,
                                        idProduto = p.idProduto,
                                        quantidade = produtosSelecionados[p]
                                    };
                                    context.ProdutosOrcamento.Add(po);
                                    context.SaveChanges();

                                    prod = context.Produto.Where(produto => produto.idProduto == p.idProduto).FirstOrDefault();
                                    prod.quantidade -= po.quantidade;
                                    context.SaveChanges();
                                }
                            }
                            clearForm();
                            pnl_Alert.CssClass = "alert alert-success";
                            lbl_Alert.Text = "Orçamento criado com sucesso";
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
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }
    }
}