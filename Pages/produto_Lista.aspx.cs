using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class produto_Lista : System.Web.UI.Page
    {
        private Cliente cliente;
        private Funcionario funcionario;

        protected void Page_Load(object sender, EventArgs e)
        {
            cliente = (Cliente)Session["usuario"];
            if (cliente == null)
                Response.Redirect("login.aspx", false);
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        funcionario = context.Funcionario.Where(f => f.cpf.Equals(cliente.cpf)).FirstOrDefault();
                        if (funcionario != null)
                        {
                            if (context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault() != null)
                                preencher_Tabela();
                            else
                                Response.Write("<script>alert('erro no BD - Oficina nao cadastrada');</script>");
                        }
                        else
                        {
                            Response.Redirect("home.aspx", false);
                        }
                        lbl_Nome.Text = cliente.nome;
                        if (funcionario == null)
                        {
                            pnl_Oficina.Visible = false;
                            btn_CadastroOficina.Visible = true;

                            List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(r => r.cpfCliente.Equals(cliente.cpf)).ToList();
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
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void preencher_Tabela()
        {
            int adicionados = 0;
            TableRow row;
            TableCell cell;
            Button btn;
            try
            {
                using (var context = new DatabaseEntities())
                {
                    var query = context.Produto.Where(p => p.cnpjOficina.Equals(funcionario.cnpjOficina)).ToList();

                    if (query.Count > 0)
                    {
                        foreach (var produto in query)
                        {
                            if (produto.ativo == 1)
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

                                //QTD
                                cell = new TableCell();
                                cell.Text = produto.quantidade.ToString();
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                //VALIDADE
                                cell = new TableCell();
                                if (produto.validade != null)
                                {
                                    cell.Text = DateTime.Parse(produto.validade.ToString()).ToString("dd/MM/yyyy");
                                }
                                else
                                    cell.Text = "";
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
                                    double dbl = (Double)produto.precoVenda;
                                    cell.Text = "R$ " + dbl.ToString("0.00");
                                }
                                else
                                    cell.Text = "";
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                //ACOES
                                //EDITAR
                                cell = new TableCell();
                                cell.CssClass = "text-center align-middle";
                                btn = new Button();
                                btn.Click += new EventHandler(btn_Acao_Click);
                                btn.Text = "Editar";
                                btn.CssClass = "btn btn-primary ml-2";
                                btn.CommandName = "editarProduto";
                                btn.CommandArgument = produto.idProduto.ToString();
                                cell.Controls.Add(btn);

                                //EXCLUIR
                                btn = new Button();
                                btn.Click += new EventHandler(btn_Acao_Click);
                                btn.Text = "Excluir";
                                btn.CssClass = "btn btn-danger ml-2";
                                btn.CommandName = "excluirProduto";
                                btn.CommandArgument = produto.idProduto.ToString();
                                cell.Controls.Add(btn);
                                row.Cells.Add(cell);

                                tbl_Produtos.Rows.Add(row);
                                adicionados += 1;
                            }
                        }
                    }
                }
                if (adicionados == 0)
                {
                    row = new TableRow();
                    cell = new TableCell();
                    cell.Text = "Você não tem nenhum produto cadastrado";
                    cell.ColumnSpan = 8;
                    cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                    row.Cells.Add(cell);
                    tbl_Produtos.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void btn_Acao_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.CommandName)
            {
                case "editarProduto":
                    try
                    {
                        using (var context = new DatabaseEntities())
                        {
                            Produto produto = context.Produto.Where(p => p.idProduto.ToString().Equals(btn.CommandArgument)).FirstOrDefault();
                            if (produto != null)
                            {
                                Session["produto"] = produto;
                                Response.Redirect("produto_Editar.aspx", false);
                            }
                            else
                            {
                                Response.Write("<script>alert('Erro na aplicacao, produto nao existe mais no BD');</script>");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('" + ex.Message + "');</script>");
                    }
                    break;

                case "excluirProduto":
                    try
                    {
                        using (var context = new DatabaseEntities())
                        {
                            Produto produto = context.Produto.Where(p => p.idProduto.ToString().Equals(btn.CommandArgument)).FirstOrDefault();
                            if (produto != null)
                            {
                                produto.ativo = 0;
                                context.SaveChanges();
                                Response.Redirect("produto_Lista.aspx", false);
                            }
                            else
                            {
                                Response.Write("<script>alert('Erro na aplicacao, produto nao existe mais no BD');</script>");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('" + ex.Message + "');</script>");
                    }
                    break;

                default:
                    Response.Write("<script>alert('Erro na opção');</script>");
                    break;
            }
        }

        protected void btn_CadastrarProduto_Click(object sender, EventArgs e)
        {
            Response.Redirect("produto_Cadastro.aspx", false);
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}