using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class orcamento_Editar : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private Orcamento o;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    if (f == null)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else
                    {
                        o = (Orcamento)Session["orcamento"];
                        if (o == null)
                        {
                            Response.Redirect("orcamento_ListaOficina");
                        }
                        else
                        {
                            preencherTabela();
                        }
                    }
                }
            }
        }

        private void preencherTabela()
        {
            try
            {
                tbl_Servicos.Rows.Clear();

                using (DatabaseEntities context = new DatabaseEntities())
                {
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;
                    TableRow row;
                    TableCell cell;
                    Button btn;

                    headerRow = new TableHeaderRow();
                    headerRow.CssClass = "thead-dark";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Descrição";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Status";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Ações";
                    headerRow.Cells.Add(headerCell);

                    tbl_Servicos.Rows.Add(headerRow);

                    List<ServicosOrcamento> servicos = context.ServicosOrcamento.Where(so => so.idOrcamento == o.idOrcamento).ToList();

                    foreach (ServicosOrcamento s in servicos)
                    {
                        row = new TableRow();

                        cell = new TableCell();
                        cell.Text = s.Servico.descricao;
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.Text = s.status;
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.CssClass = "text-center align-middle";

                        btn = new Button();
                        btn.Click += new EventHandler(btn_AlterarStatus_Click);
                        btn.ID = "btn_Alterar" + s.Servico.idServico;
                        if (s.status.Equals("Pendente"))
                        {
                            btn.Text = "Iniciar";
                            btn.CssClass = "btn btn-primary";
                        }
                        if (s.status.Equals("Em execução"))
                        {
                            btn.Text = "Concluir";
                            btn.CssClass = "btn btn-success";
                        }
                        if (s.status.Equals("Concluído"))
                        {
                            btn.Text = "Reabrir";
                            btn.CssClass = "btn btn-info";
                        }
                        btn.CommandArgument = s.Servico.idServico.ToString();
                        cell.Controls.Add(btn);

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

        protected void btn_AlterarStatus_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    ServicosOrcamento servico = context.ServicosOrcamento.Where(so => so.idOrcamento == o.idOrcamento && so.idServico == id).FirstOrDefault();
                    if (servico.status.Equals("Pendente"))
                    {
                        servico.status = "Em execução";
                    }
                    else if (servico.status.Equals("Em execução"))
                    {
                        servico.status = "Concluído";
                    }
                    else if (servico.status.Equals("Concluído"))
                    {
                        servico.status = "Pendente";
                    }
                    context.SaveChanges();
                    preencherTabela();
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Retornar_Click(object sender, EventArgs e)
        {
            Session["orcamento"] = null;
            Response.Redirect("orcamento_ListaOficina.aspx", false);
        }
    }
}