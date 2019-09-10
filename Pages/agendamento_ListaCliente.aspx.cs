using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class agendamento_ListaCliente : System.Web.UI.Page
    {
        private Cliente c;
        private static bool mostrarPassados;

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
                    headerRow.CssClass = "thead-dark";

                    headerCell = new TableHeaderCell();
                    headerCell.CssClass = "text-center";
                    headerCell.Text = "Oficina";
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
                    agendamentos = agendamentos.OrderBy(a => a.data + a.hora).ToList();

                    if (agendamentos.Count > 0)
                    {
                        foreach (Agendamento a in agendamentos)
                        {
                            if (mostrarPassados)
                            {
                                row = new TableRow();

                                cell = new TableCell();
                                cell.Text = a.Oficina.nome;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = a.Oficina.telefone;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = a.Oficina.email;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                dataHora = a.data + a.hora;
                                cell.Text = dataHora.ToString("dd/MM/yyyy") + " - " + dataHora.Hour.ToString("00") + ":" + dataHora.Minute.ToString("00");
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = a.Veiculo.modelo + " - " + a.Veiculo.placa;
                                cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.Text = a.status;
                                if (a.status.Equals("Confirmado"))
                                    cell.CssClass = "text-center text-success align-middle";
                                else if (a.status.Equals("Cancelado pelo cliente") || a.status.Equals("Cancelado pela oficina"))
                                    cell.CssClass = "text-center text-danger align-middle";
                                else
                                    cell.CssClass = "text-center align-middle";
                                row.Cells.Add(cell);

                                cell = new TableCell();
                                cell.CssClass = "text-center align-middle";
                                if (dataHora.CompareTo(DateTime.Now) > 0)
                                {
                                    if (a.status.Equals("Confirmação pendente") || a.status.Equals("Confirmado"))
                                    {
                                        btn = new Button();
                                        btn.Click += new EventHandler(btn_Cancelar_Click);
                                        btn.ID = "btn_Cancelar" + a.idAgendamento;
                                        btn.Text = "Cancelar";
                                        btn.CssClass = "btn btn-danger";
                                        btn.CommandArgument = a.idAgendamento.ToString();
                                        cell.Controls.Add(btn);
                                    }
                                }
                                row.Cells.Add(cell);

                                tbl_Agendamentos.Rows.Add(row);
                            }
                            else
                            {
                                if (a.data.CompareTo(DateTime.Now.Date) >= 0)
                                {
                                    row = new TableRow();

                                    cell = new TableCell();
                                    cell.Text = a.Oficina.nome;
                                    cell.CssClass = "text-center align-middle";
                                    row.Cells.Add(cell);

                                    cell = new TableCell();
                                    cell.Text = a.Oficina.telefone;
                                    cell.CssClass = "text-center align-middle";
                                    row.Cells.Add(cell);

                                    cell = new TableCell();
                                    cell.Text = a.Oficina.email;
                                    cell.CssClass = "text-center align-middle";
                                    row.Cells.Add(cell);

                                    cell = new TableCell();
                                    dataHora = a.data + a.hora;
                                    cell.Text = dataHora.ToString("dd/MM/yyyy") + " - " + dataHora.Hour.ToString("00") + ":" + dataHora.Minute.ToString("00");
                                    cell.CssClass = "text-center align-middle";
                                    row.Cells.Add(cell);

                                    cell = new TableCell();
                                    cell.Text = a.Veiculo.modelo + " - " + a.Veiculo.placa;
                                    cell.CssClass = "text-center align-middle";
                                    row.Cells.Add(cell);

                                    cell = new TableCell();
                                    cell.Text = a.status;
                                    if (a.status.Equals("Confirmado"))
                                        cell.CssClass = "text-center text-success align-middle";
                                    else if (a.status.Equals("Cancelado pelo cliente") || a.status.Equals("Cancelado pela oficina"))
                                        cell.CssClass = "text-center text-danger align-middle";
                                    else
                                        cell.CssClass = "text-center align-middle";
                                    row.Cells.Add(cell);

                                    cell = new TableCell();
                                    cell.CssClass = "text-center align-middle";
                                    if (a.status.Equals("Confirmação pendente") || a.status.Equals("Confirmado"))
                                    {
                                        btn = new Button();
                                        btn.Click += new EventHandler(btn_Cancelar_Click);
                                        btn.ID = "btn_Cancelar" + a.idAgendamento;
                                        btn.Text = "Cancelar";
                                        btn.CssClass = "btn btn-danger";
                                        btn.CommandArgument = a.idAgendamento.ToString();
                                        cell.Controls.Add(btn);
                                    }
                                    row.Cells.Add(cell);

                                    tbl_Agendamentos.Rows.Add(row);
                                }
                            }
                        }
                    }
                    else
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = "Nenhum agendamento foi encontrado";
                        cell.ColumnSpan = 7;
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

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(btn.CommandArgument);
                    Agendamento agendamento = context.Agendamento.Where(a => a.idAgendamento == id).FirstOrDefault();
                    agendamento.status = "Cancelado pelo cliente";
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

        protected void btn_MostrarEsconderPassados_Click(object sender, EventArgs e)
        {
            if (mostrarPassados)
            {
                btn_MostrarEsconderPassados.Text = "Mostrar agendamentos passados";
                mostrarPassados = false;
                preencherTabela();
            }
            else
            {
                btn_MostrarEsconderPassados.Text = "Esconder agendamentos passados";
                mostrarPassados = true;
                preencherTabela();
            }
        }
    }
}