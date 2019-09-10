using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class agendamento_ListaOficina : System.Web.UI.Page
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
            }
        }

        protected void preencherTabela()
        {
            try
            {
                if (tbl_Agendamentos.Rows.Count > 0)
                    tbl_Agendamentos.Rows.Clear();
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    TableHeaderRow headerRow;
                    TableHeaderCell headerCell;

                    headerRow = new TableHeaderRow();

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Cliente";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Telefone";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "E-mail";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Data/Hora";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Veículo";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Status";
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Ações";
                    headerRow.Cells.Add(headerCell);

                    tbl_Agendamentos.Rows.Add(headerRow);

                    TableRow row;
                    TableCell cell;
                    Button btn;
                    DateTime dataHora;

                    List<Agendamento> agendamentos = context.Agendamento.Where(a => a.cpfCliente.Equals(c.cpf)).ToList();

                    if (agendamentos.Count > 0)
                    {
                        foreach (Agendamento a in agendamentos)
                        {
                            row = new TableRow();

                            cell = new TableCell();
                            cell.Text = a.Cliente.nome;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = a.Cliente.telefone;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = a.Cliente.email;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            dataHora = a.data + a.hora;
                            cell.Text = dataHora.ToString();
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.Text = a.Veiculo.marca + " " + a.Veiculo.modelo;
                            cell.CssClass = "text-center align-middle";
                            row.Cells.Add(cell);

                            cell = new TableCell();
                            cell.CssClass = "text-center align-middle";
                            if (a.status.Equals("Confirmação pendente"))
                            {
                                btn = new Button();
                                btn.Click += new EventHandler(btn_Confirmar_Click);
                                btn.Text = "Confirmar";
                                btn.CssClass = "btn btn-success mr-2";
                                btn.CommandArgument = a.idAgendamento.ToString();
                                cell.Controls.Add(btn);
                            }
                            if (a.status.Equals("Confirmação pendente") || a.status.Equals("Confirmado"))
                            {
                                btn = new Button();
                                btn.Click += new EventHandler(btn_Cancelar_Click);
                                btn.Text = "Cancelar";
                                btn.CssClass = "btn btn-danger ml-2";
                                btn.CommandArgument = a.idAgendamento.ToString();
                                cell.Controls.Add(btn);
                            }
                            row.Cells.Add(cell);

                            tbl_Agendamentos.Rows.Add(row);
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Nenhum agendamento foi encontrado";
                        cell.ColumnSpan = 6;
                        cell.CssClass = "text-center align-middle font-weight-bold text-primary";
                        row.Cells.Add(cell);
                        tbl_Agendamentos.Rows.Add(row);
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

        protected void btn_Confirmar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Agendamento agendamento = context.Agendamento.Where(a => a.idAgendamento == id).FirstOrDefault();
                    agendamento.status = "Confirmado";
                    context.SaveChanges();
                    preencherTabela();
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "Agendamento confirmado";
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

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Agendamento agendamento = context.Agendamento.Where(a => a.idAgendamento == id).FirstOrDefault();
                    agendamento.status = "Cancelado pela oficina";
                    context.SaveChanges();
                    preencherTabela();
                    pnl_Alert.CssClass = "alert alert-success";
                    lbl_Alert.Text = "Agendamento cancelado";
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
}