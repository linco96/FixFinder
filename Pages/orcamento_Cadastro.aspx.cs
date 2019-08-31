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
        private static Cliente fregues;

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
                    ListItem item;
                    if (fregues != null)
                    {
                        List<Veiculo> veiculos = context.Veiculo.Where(v => v.cpfCliente.Equals(fregues.cpf)).ToList();
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
                            txt_Veiculo.Attributes.Remove("disabled");
                            txt_Nome.Text = fregues.nome;
                            txt_CPF.Text = fregues.cpf;
                            alert_CPF.Visible = false;
                        }
                    }

                    List<Servico> servicos = context.Servico.Where(servico => servico.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();
                    List<Produto> produtos = context.Produto.Where(produto => produto.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();

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
                                produtosSelecionados.Add(p, quantidade);
                                pnl_Alert.Visible = false;
                                Response.Redirect(Request.RawUrl);
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
                foreach (Produto p in produtosSelecionados.Keys)
                {
                    id = int.Parse(btn.CommandArgument);
                    if (p.idProduto == id)
                    {
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

                        pnl_Alert.Visible = false;
                        Response.Redirect(Request.RawUrl);
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

        [Obsolete]
        protected void btn_CarregarCliente_Click(object sender, EventArgs e)
        {
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
                            fregues = cliente;
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
            txt_Desconto.Text = "123456";
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
        }

        protected void do_Postback()
        {
        }
    }
}