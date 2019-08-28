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
        private Oficina o;

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
                    List<Servico> servicos = context.Servico.Where(servico => servico.cnpjOficina.Equals(o.cnpj)).ToList();
                    List<Produto> produtos = context.Produto.Where(produto => produto.cnpjOficina.Equals(o.cnpj)).ToList();

                    if (servicos.Count > 0)
                    {
                        TableRow row;
                        TableCell cell;
                        Button btn;
                        foreach (Servico s in servicos)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = s.descricao;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = s.valor.ToString("0.00");
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Click += new EventHandler(btn_RemoverServico_Click);
                            btn.Text = "Remover";
                            btn.CssClass = "btn btn-danger";
                            btn.CommandName = "excluirVeiculo";
                            btn.CommandArgument = veiculo.idVeiculo.ToString();
                            cell.Controls.Add(btn);

                            row.Cells.Add(cell);

                            tbl_Veiculos.Rows.Add(row);
                        }
                    }
                    else
                    {
                        ListItem item = new ListItem();
                        item.Text = "Nenhum serviço encontrado";
                        item.Enabled = false;
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