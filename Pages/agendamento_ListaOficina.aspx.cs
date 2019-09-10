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
                using (DatabaseEntities context = new DatabaseEntities())
                {
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
                            cell.Text = a.Oficina.nome;
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
                                btn.CssClass = "btn btn-success ml-2";
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
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
        }
    }
}