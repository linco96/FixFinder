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

                    List<Agendamento> agendamentos = context.Agendamento.Where(a => a.cpfCliente.Equals(c.cpf)).ToList();
                    foreach (Agendamento a in agendamentos)
                    {
                        //row = new TableRow();

                        //cell = new TableCell();
                        //cell.Text = a.Oficina.nome;
                        //cell.CssClass = "text-center align-middle";
                        //row.Cells.Add(cell);

                        //cell = new TableCell();
                        //cell.Text = veiculo.modelo;
                        //cell.CssClass = "text-center align-middle";
                        //row.Cells.Add(cell);

                        //cell = new TableCell();
                        //cell.Text = veiculo.ano.ToString();
                        //cell.CssClass = "text-center align-middle";
                        //row.Cells.Add(cell);

                        //cell = new TableCell();
                        //cell.Text = veiculo.placa;
                        //cell.CssClass = "text-center align-middle";
                        //row.Cells.Add(cell);

                        ////Botao Excluir
                        //cell = new TableCell();
                        //cell.CssClass = "text-center align-middle";
                        //btn = new Button();
                        //btn.Click += new EventHandler(btn_Acao_Click);
                        //btn.Text = "Excluir";
                        //btn.CssClass = "btn btn-danger ml-2";
                        //btn.CommandName = "excluirVeiculo";
                        //btn.CommandArgument = veiculo.idVeiculo.ToString();
                        //cell.Controls.Add(btn);

                        //row.Cells.Add(cell);

                        //tbl_Veiculos.Rows.Add(row);
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
    }
}