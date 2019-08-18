﻿using System;
using FixFinder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class veiculo_Lista : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
                Response.Redirect("login.aspx", false);
            preencher_Tabela();
        }

        protected void btn_CadastrarVeiculo_Click(object sender, EventArgs e)
        {
            Response.Redirect("veiculo_Cadastro.aspx", false);
        }

        private void preencher_Tabela()
        {
            using (var context = new DatabaseEntities())
            {
                var query = context.Veiculo.Where(v => v.cpfCliente.Equals(c.cpf)).ToList();
                TableRow row;
                TableCell cell;
                Button btn;
                if (query.Count > 0)
                {
                    foreach (var veiculo in query)
                    {
                        row = new TableRow();

                        cell = new TableCell();
                        cell.Text = veiculo.marca;
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.Text = veiculo.modelo;
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.Text = veiculo.ano.ToString();
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        cell = new TableCell();
                        cell.Text = veiculo.placa;
                        cell.CssClass = "text-center align-middle";
                        row.Cells.Add(cell);

                        //Botao Editar
                        cell = new TableCell();
                        btn = new Button();
                        btn.Click += new EventHandler(btn_Acao_Click);
                        btn.Text = "Editar";
                        btn.CssClass = "btn btn-primary";
                        btn.CommandName = "editarVeiculo";
                        btn.CommandArgument = veiculo.idVeiculo.ToString();
                        cell.Controls.Add(btn);
                        row.Cells.Add(cell);

                        //Botao Excluir
                        cell = new TableCell();
                        btn = new Button();
                        btn.Click += new EventHandler(btn_Acao_Click);
                        btn.Text = "Excluir";
                        btn.CssClass = "btn btn-danger ml-2";
                        btn.CommandName = "excluirVeiculo";
                        btn.CommandArgument = veiculo.idVeiculo.ToString();
                        cell.Controls.Add(btn);
                        row.Cells.Add(cell);

                        tbl_Veiculos.Rows.Add(row);
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
                    tbl_Veiculos.Rows.Add(row);
                }
            }
        }

        private void btn_Acao_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Veiculo veiculo = null;
            switch (btn.CommandName)
            {
                case "editarVeiculo":
                    using (var context = new DatabaseEntities())
                    {
                        veiculo = (Veiculo)context.Veiculo.Where(v => v.idVeiculo.ToString().Equals(btn.CommandArgument)).FirstOrDefault();
                        if (veiculo == null)
                            Response.Write("<script>alert('Erro no banco de dados - Veiculo não encontrado');</script>");
                        else
                        {
                            Session["veiculo"] = veiculo;
                            Response.Redirect("veiculo_Editar.aspx", false);
                        }
                    }
                    break;

                case "excluirVeiculo":
                    using (var context = new DatabaseEntities())
                    {
                        veiculo = (Veiculo)context.Veiculo.Where(v => v.idVeiculo.ToString().Equals(btn.CommandArgument)).FirstOrDefault();
                        if (veiculo == null)
                            Response.Write("<script>alert('Erro no banco de dados - Veiculo não encontrado');</script>");
                        else
                        {
                            context.Veiculo.Remove(veiculo);
                            preencher_Tabela();
                        }
                    }
                    break;

                default:
                    Response.Write("<script>alert('Erro na opção');</script>");
                    break;
            }
        }
    }
}