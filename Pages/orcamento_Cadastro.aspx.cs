using System;
using System.Collections.Generic;
using System.Linq;
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
        private static List<Produto> produtosSelecionados;
        private static List<int> quantidades;

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
                            produtosSelecionados = new List<Produto>();
                        if (servicosSelecionados == null)
                            servicosSelecionados = new List<Servico>();
                        if (quantidades == null)
                            quantidades = new List<int>();
                        preencherCampos();
                    }
                }
            }
            else
            {
                Response.Redirect("login.aspx", false);
            }
        }

        protected void preencherCampos()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<Servico> servicos = context.Servico.Where(servico => servico.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();
                    List<Produto> produtos = context.Produto.Where(produto => produto.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();
                    ListItem item;

                    if (txt_ServicoSelecionado.Items.Count == 0)
                    {
                        if (servicos.Count > 0)
                        {
                            foreach (Servico s in servicos)
                            {
                                item = new ListItem();
                                item.Text = s.descricao + " - R$ " + s.valor.ToString("0.00");
                                item.Value = s.idServico.ToString();
                                txt_ServicoSelecionado.Items.Add(item);
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

                    if (txt_ProdutoSelecionado.Items.Count == 0)
                    {
                        if (produtos.Count > 0)
                        {
                            double precoVenda;
                            DateTime validade;
                            foreach (Produto p in produtos)
                            {
                                precoVenda = (Double)p.precoVenda;
                                item = new ListItem();
                                if (p.validade == null)
                                {
                                    item.Text = p.descricao + " " + p.marca + " - R$ " + precoVenda.ToString("0.00");
                                }
                                else
                                {
                                    validade = (DateTime)p.validade;
                                    item.Text = p.descricao + " " + p.marca + " - R$ " + precoVenda.ToString("0.00") + " (Vence em " + validade.ToString("dd/MM/yyyy") + ")";
                                }
                                item.Value = p.idProduto.ToString();
                                txt_ProdutoSelecionado.Items.Add(item);
                            }
                            foreach (Produto p in produtosSelecionados)
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

                    TableRow row;
                    TableCell cell;
                    Button btn;

                    if (servicosSelecionados.Count > 0)
                    {
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
                            btn.CommandArgument = s.idServico.ToString();
                            btn.CssClass = "btn btn-danger";
                            cell.Controls.Add(btn);
                            row.Cells.Add(cell);

                            tbl_Servicos.Rows.Add(row);
                        }
                        tbl_Servicos.Visible = true;
                    }

                    if (produtosSelecionados.Count > 0)
                    {
                        Double precoVenda;
                        DateTime validade;

                        foreach (Produto p in produtosSelecionados)
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
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Text = "Remover";
                            btn.Click += new EventHandler(btn_RemoverProduto_Click);
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
                        Response.Redirect(Request.RawUrl);
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
                int quantidade = int.Parse(txt_ProdutoQuantidade.Text);
                if (quantidade > 0)
                {
                    try
                    {
                        using (DatabaseEntities context = new DatabaseEntities())
                        {
                            int id = int.Parse(txt_ProdutoSelecionado.SelectedValue);
                            Produto p = context.Produto.Where(produto => produto.idProduto == id).FirstOrDefault();
                            produtosSelecionados.Add(p);
                            quantidades.Add(quantidade);
                            pnl_Alert.Visible = false;
                            Response.Redirect(Request.RawUrl);
                        }
                    }
                    catch (Exception ex)
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
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
                        break;
                    }
                }
                pnl_Alert.Visible = false;
                Response.Redirect(Request.RawUrl);
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
                foreach (Produto p in produtosSelecionados)
                {
                    id = int.Parse(btn.CommandArgument);
                    if (p.idProduto == id)
                    {
                        quantidades.RemoveAt(produtosSelecionados.FindIndex(produto => produto == p));
                        produtosSelecionados.Remove(p);
                        break;
                    }
                }
                pnl_Alert.Visible = false;
                Response.Redirect(Request.RawUrl);
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
            txt_Descricao.Text = "";
            txt_Valor.Text = "";
        }

        protected void btn_CadastrarServico_Click(object sender, EventArgs e)
        {
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
        }
    }
}