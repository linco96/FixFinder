using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class cartao_Lista : System.Web.UI.Page
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
                preencherTabela();
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Funcionario f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
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

        private void preencherTabela()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    List<Cartao> cartoes = context.Cartao.Where(card => card.cpfCliente.Equals(c.cpf)).ToList();
                    TableRow row;
                    TableCell cell;
                    Button btn;
                    if (cartoes.Count > 0)
                    {
                        foreach (Cartao card in cartoes)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = card.bandeira.ToUpper();
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = card.numero.Substring(card.numero.Length - 4);
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = card.titular;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            if (card.mesVencimento < 10)
                                cell.Text = "0" + card.mesVencimento + "/" + card.anoVencimento;
                            else
                                cell.Text = card.mesVencimento + "/" + card.anoVencimento;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Click += new EventHandler(btn_DeletarCartao_Click);
                            btn.ID = "btn_Remover" + card.idCartao.ToString();
                            btn.Text = "Remover";
                            btn.CssClass = "btn btn-danger";
                            btn.CommandArgument = card.idCartao.ToString();
                            cell.Controls.Add(btn);
                            row.Cells.Add(cell);

                            tbl_Cartao.Rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Você não possui nenhum cartão cadastrado";
                        cell.ColumnSpan = 5;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        row.Cells.Add(cell);
                        tbl_Cartao.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + ". Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        protected void btn_DeletarCartao_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Cartao card = context.Cartao.Where(cartao => cartao.idCartao == id).FirstOrDefault();
                    context.Cartao.Remove(card);
                    context.SaveChanges();
                    Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + ". Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Retorno_Click(object sender, EventArgs e)
        {
            Response.Redirect("pagamento_ListaCliente.aspx", false);
        }
    }
}