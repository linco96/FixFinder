using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class fornecedor_Lista : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;
        private Oficina oficina;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
                Response.Redirect("login.aspx", false);
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        c = (Cliente)context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).FirstOrDefault();
                        funcionario = (Funcionario)context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();
                        if (funcionario == null)
                        {
                            Response.Redirect("home.aspx", false);
                        }
                        else
                        {
                            oficina = (Oficina)context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault();
                            if (oficina != null)
                            {
                                if (funcionario.cargo.ToUpper().Equals("GERENTE"))
                                    btn_CadastrarFornecedor.Visible = true;
                                else
                                    btn_CadastrarFornecedor.Visible = false;
                                preencher_Tabela();
                            }
                            else
                            {
                                Response.Write("<script>alert('erro no banco - Oficina nao cadastrada');</script>");
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
            try
            {
                using (var context = new DatabaseEntities())
                {
                    var query = context.Fornecedor.Where(f => f.cnpjOficina.Equals(oficina.cnpj)).ToList();
                    TableRow row;
                    TableCell cell;
                    Button btn;
                    if (query.Count > 0)
                    {
                        foreach (var fornecedor in query)
                        {
                            row = new TableRow();
                            //CNPJ
                            cell = new TableCell();
                            cell.Text = fornecedor.cnpjFornecedor;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //FORNECEDOR
                            cell = new TableCell();
                            cell.Text = fornecedor.razaoSocial;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //TELEFONE
                            cell = new TableCell();
                            cell.Text = fornecedor.telefone;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //EMAIL
                            cell = new TableCell();
                            cell.Text = fornecedor.email;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);
                            //ACOES
                            //EXCLUIR
                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Click += new EventHandler(btn_Acao_Click);
                            btn.Text = "Excluir";
                            btn.CssClass = "btn btn-danger ml-2";
                            btn.CommandName = "excluirVeiculo";
                            btn.CommandArgument = veiculo.idVeiculo.ToString();
                            cell.Controls.Add(btn);
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Você não tem nenhum fornecedor cadastrado";
                        cell.ColumnSpan = 6;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        row.Cells.Add(cell);
                        tbl_Fornecedores.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btn_CadastrarFornecedor_Click(object sender, EventArgs e)
        {
            Response.Redirect("fornecedor_Cadastro.aspx", false);
        }
    }
}