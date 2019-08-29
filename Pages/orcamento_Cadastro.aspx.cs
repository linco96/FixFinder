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
                        preencherCampos();
                }
            }
            else
            {
                Response.Redirect("home.aspx", false);
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

                    if (servicos.Count > 0)
                    {
                        foreach (Servico s in servicos)
                        {
                            item = new ListItem();
                            item.Text = s.descricao + " - R$ " + s.valor.ToString("0.00");
                            item.Value = s.idServico.ToString();
                            txt_ServicoSelecionado.Items.Add(item);
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

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
        }

        protected void btn_AdicionarServico_Click(object sender, EventArgs e)
        {
        }

        protected void btn_AdicionarProduto_Click(object sender, EventArgs e)
        {
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

        protected void btn_RemoverServico_Click(object sender, EventArgs e)
        {
        }
    }
}