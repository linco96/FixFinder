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
                            if (funcionario.salario == null)
                            {
                                double salario = (Double)funcionario.salario;
                                cell.Text = "R$ " + salario.ToString("0.00");
                            }
                            else
                            {
                                cell.Text = "-";
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            if (funcionario.banco == null)
                            {
                                cell.Text = funcionario.banco.ToString();
                            }
                            else
                            {
                                cell.Text = "-";
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            if (funcionario.agencia == null)
                            {
                                cell.Text = funcionario.agencia;
                            }
                            else
                            {
                                cell.Text = "-";
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            if (funcionario.conta == null)
                            {
                                cell.Text = funcionario.conta;
                            }
                            else
                            {
                                cell.Text = "-";
                            }
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            if (!funcionario.cargo.ToLower().Equals("gerente"))
                            {
                                //Botao Excluir

                                btn = new Button();
                                btn.Click += new EventHandler(btn_Editar_Click);
                                btn.ID = "btn_Editar" + f.cpf;
                                btn.Text = "Editar";
                                btn.CssClass = "btn btn-primary mr-2";
                                btn.CommandArgument = funcionario.cpf;
                                cell.Controls.Add(btn);

                                //Botao Excluir
                                btn2 = new Button();
                                btn2.Click += new EventHandler(btn_Excluir_Click);
                                btn.ID = "btn_Excluir" + f.cpf;
                                btn2.Text = "Excluir";
                                btn2.CssClass = "btn btn-danger ml-2";
                                btn2.CommandArgument = funcionario.cpf;
                                cell.Controls.Add(btn2);
                            }
                            row.Cells.Add(cell);

                            tbl_Funcionarios.Rows.Add(row);
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
                Funcionario func;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    func = context.Funcionario.Where(funcionario => funcionario.cpf.Equals(btn.CommandArgument)).FirstOrDefault();
                }

                Session["funcionarioEditar"] = func;
                Response.Redirect("funcionario_Editar.aspx", false);
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_RegistrarFuncionario_Click1(object sender, EventArgs e)
        {
            Response.Redirect("funcionario_Cadastro.aspx", false);
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}