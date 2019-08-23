using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class funcionario_Lista : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                c = (Cliente)Session["usuario"];
                if (c == null)
                    Response.Redirect("login.aspx", false);
                else
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    if (f == null || f.cargo.ToLower() != "gerente")
                        Response.Redirect("home.aspx", false);
                    else
                    {
                        o = context.Oficina.Where(of => of.cnpj.Equals(f.cnpjOficina)).FirstOrDefault();
                        preencher_Tabela();
                    }
                }
            }
        }

        protected void btn_RegistrarFuncionario_Click(object sender, EventArgs e)
        {
            Response.Redirect("funcionario_Cadastro.aspx", false);
        }

        private void preencher_Tabela()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    List<Funcionario> funcionarios = context.Funcionario.Where(func => func.cnpjOficina.Equals(o.cnpj)).ToList();
                    TableRow row;
                    TableCell cell;
                    Button btn, btn2;
                    if (funcionarios.Count > 0)
                    {
                        foreach (var funcionario in funcionarios)
                        {
                            if (!funcionario.cargo.ToLower().Equals("gerente"))
                            {
                                row = new TableRow();

                                cell = new TableCell();
                                cell.Text = funcionario.Cliente.nome;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = funcionario.Cliente.telefone;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = funcionario.Cliente.email;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = funcionario.cargo;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = "R$ " + funcionario.salario.ToString("0.00");
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = funcionario.banco.ToString();
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = funcionario.agencia;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = funcionario.conta;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                //Botao Excluir
                                cell = new TableCell();
                                cell.CssClass = "text-center align-middle";
                                btn = new Button();
                                btn.Click += new EventHandler(btn_Editar_Click);
                                btn.Text = "Editar";
                                btn.CssClass = "btn btn-primary mr-2";
                                btn.CommandArgument = funcionario.cpf;
                                cell.Controls.Add(btn);

                                //Botao Excluir
                                btn2 = new Button();
                                btn2.Click += new EventHandler(btn_Excluir_Click);
                                btn2.Text = "Excluir";
                                btn2.CssClass = "btn btn-danger ml-2";
                                btn2.CommandArgument = funcionario.cpf;
                                cell.Controls.Add(btn2);
                                row.Cells.Add(cell);

                                tbl_Funcionarios.Rows.Add(row);
                            }
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Você não tem nenhum veículo cadastrado";
                        cell.ColumnSpan = 6;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        row.Cells.Add(cell);
                        tbl_Funcionarios.Rows.Add(row);
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

        private void btn_Excluir_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            try
            {
                using (var context = new DatabaseEntities())
                {
                    context.Funcionario.Remove(context.Funcionario.Where(func => func.cpf.Equals(btn.CommandArgument)).FirstOrDefault());
                    context.SaveChanges();
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

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            try
            {
                Funcionario f;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(btn.CommandArgument)).FirstOrDefault();
                }

                Session["funcionario"] = f;
                Response.Redirect("funcionario_Editar.aspx", false);
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