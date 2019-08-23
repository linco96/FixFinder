using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class servico_Lista : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("home.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    c = context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).FirstOrDefault();
                    if (c.Funcionario == null)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else
                    {
                        preencher_Tabela();
                    }
                }
            }
        }

        protected void btn_Cadastro_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Servico s = new Servico
                    {
                        descricao = txt_Descricao.Text,
                        valor = double.Parse(txt_Valor.Text),
                        cnpjOficina = c.Funcionario.cnpjOficina
                    };
                    context.Servico.Add(s);
                    context.SaveChanges();

                    form_Cadastro.Visible = false;
                    btn_CadastrarServico.Visible = true;

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

        protected void btn_CadastrarServico_Click(object sender, EventArgs e)
        {
            form_Cadastro.Visible = true;
            btn_CadastrarServico.Visible = false;
        }

        protected void btn_CancelarCadastro_Click(object sender, EventArgs e)
        {
            form_Cadastro.Visible = false;
            btn_CadastrarServico.Visible = true;
            txt_Descricao.Text = "";
            txt_Valor.Text = "";
        }

        protected void preencher_Tabela()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<Servico> servicos = context.Servico.Where(s => s.cnpjOficina.Equals(c.Funcionario.cnpjOficina)).ToList();
                    TableRow row;
                    TableCell cell;
                    Button btn;
                    if (servicos.Count > 0)
                    {
                        foreach (Servico s in servicos)
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

                            //Botao Editar
                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            btn = new Button();
                            btn.Click += new EventHandler(btn_EditarServico_Click);
                            btn.Text = "Editar";
                            btn.CssClass = "btn btn-primary";
                            btn.CommandArgument = s.idServico.ToString();
                            btn.Attributes.Add("formnovalidate", "true");
                            cell.Controls.Add(btn);

                            row.Cells.Add(cell);

                            tbl_Servicos.Rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Nenhum serviço encontrado";
                        cell.ColumnSpan = 6;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        row.Cells.Add(cell);
                        tbl_Servicos.Rows.Add(row);
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

        protected void btn_EditarServico_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Servico servico = context.Servico.Where(s => s.idServico == id).FirstOrDefault();
                    head_Edicao.InnerText = "Editando o serviço:    " + servico.descricao + " - R$ " + servico.valor.ToString("0.00");
                    txt_DescricaoEdicao.Text = servico.descricao;
                    txt_ValorEdicao.Text = servico.valor.ToString("0.00");
                    form_Edicao.Visible = true;
                    form_Cadastro.Visible = false;
                    btn_CadastrarServico.Visible = false;
                    btn_SalvarEdicao.CommandArgument = servico.idServico.ToString();
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_SalvarEdicao_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Servico servico = context.Servico.Where(s => s.idServico == id).FirstOrDefault();
                    servico.descricao = txt_DescricaoEdicao.Text;
                    servico.valor = double.Parse(txt_ValorEdicao.Text);
                    context.SaveChanges();

                    form_Edicao.Visible = false;
                    btn_CadastrarServico.Visible = true;

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

        protected void btn_CancelarEdicao_Click(object sender, EventArgs e)
        {
            form_Edicao.Visible = false;
            btn_CadastrarServico.Visible = true;
        }
    }
}