using FixFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class relatorio : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        public bool gerarGrafico;
        public static String jsonGrafico;
        public static String jsonGrafico2;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];

            if (c == null)
                Response.Redirect("login.aspx", false);
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
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
                    if (f != null && f.cargo.ToLower().Equals("gerente"))
                    {
                        pnl_Alert.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("home.aspx", false);
                    }
                }
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        private bool validacaoDatas()
        {
            DateTime dtInicio, dtFim;

            if (txt_DataInicio.Text != "" && txt_DataFim.Text != "")
            {
                dtInicio = DateTime.Parse(txt_DataInicio.Text);
                dtFim = DateTime.Parse(txt_DataFim.Text);

                if (dtInicio >= DateTime.Today)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de início tem que ser menor do que a data de hoje";
                    return false;
                }

                if (dtFim > DateTime.Today)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de fim tem que ser menor ou igual a data de hoje";
                    return false;
                }

                if (dtInicio >= dtFim)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de início tem que ser menor do que a data fim";
                    return false;
                }
                return true;
            }
            return false;
        }

        protected void btn_GerarGrafico_Click(object sender, EventArgs e)
        {
            if (validacaoDatas())
            {
                gerarGrafico = true;
                switch (select_Grafico.SelectedValue)
                {
                    case "despesaReceita":
                        try
                        {
                            DateTime dtIncio = DateTime.Parse(txt_DataInicio.Text);
                            DateTime dtFim = DateTime.Parse(txt_DataFim.Text);
                            dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                            List<DataPointArea> DataPointAreas1 = new List<DataPointArea>();
                            List<DataPointArea> DataPointAreas2 = new List<DataPointArea>();

                            //DataPointAreas1.Add(new DataPointArea(data, valor));
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                //Receita
                                List<Orcamento> orcamentosTemp = context.Orcamento.Where(o => o.cnpjOficina.Equals(f.cnpjOficina) && o.status.ToLower().Equals("concluído")).ToList();
                                List<KeyValuePair<Orcamento, DateTime>> orcamentosConcluidos = new List<KeyValuePair<Orcamento, DateTime>>();
                                LogOrcamento log;

                                foreach (Orcamento orcamento in orcamentosTemp)
                                {
                                    log = context.LogOrcamento.Where(l => l.alteracao.ToLower().Equals("concluído") && l.dataAlteracao >= dtIncio && l.dataAlteracao <= dtFim && l.idOrcamento == orcamento.idOrcamento).FirstOrDefault();

                                    if (log != null)
                                        orcamentosConcluidos.Add(new KeyValuePair<Orcamento, DateTime>(orcamento, log.dataAlteracao));
                                }

                                orcamentosConcluidos = orcamentosConcluidos.OrderBy(o => o.Value).ToList();

                                String ex;
                                DataPointArea temp = null;
                                double mili;
                                DateTime dt;

                                foreach (KeyValuePair<Orcamento, DateTime> oConcluidos in orcamentosConcluidos)
                                {
                                    temp = null;
                                    dt = oConcluidos.Value;
                                    long ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, new CultureInfo("en-US", false).Calendar).Ticks;
                                    dt = new DateTime(ticks);

                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in DataPointAreas1)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            temp = dtpArea;
                                            break;
                                        }
                                    }
                                    if (temp != null)
                                    {
                                        temp.Y += oConcluidos.Key.valor;
                                    }
                                    else
                                    {
                                        temp = new DataPointArea(mili, oConcluidos.Key.valor);
                                        DataPointAreas1.Add(temp);
                                    }
                                }

                                temp = null;
                                //while (dtIncio < dtFim)
                                //{
                                //    dtAux = dtIncio;
                                //    if (dtAux >= dtFim)
                                //        dtAux = dtFim;

                                //    foreach (KeyValuePair<Orcamento, DateTime> oConcluidos in orcamentosConcluidos)
                                //    {
                                //        if ()
                                //    }

                                //    dtIncio.AddDays(30);
                                //}

                                //Despesa
                                List<Compra> listaCompra = context.Compra.Where(c => c.cnpjOficina.Equals(f.cnpjOficina) && c.data >= dtIncio && c.data <= dtFim).ToList();
                                listaCompra = listaCompra.OrderBy(c => c.data).ToList();
                                foreach (Compra compra in listaCompra)
                                {
                                    temp = null;
                                    dt = compra.data;
                                    long ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                    dt = new DateTime(ticks);
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in DataPointAreas2)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            temp = dtpArea;
                                            break;
                                        }
                                    }
                                    if (temp != null)
                                    {
                                        temp.Y += totalCompra(compra);
                                    }
                                    else
                                    {
                                        temp = new DataPointArea(mili, totalCompra(compra));

                                        DataPointAreas2.Add(temp);
                                    }
                                }
                            }
                            jsonGrafico = JsonConvert.SerializeObject(DataPointAreas1);
                            jsonGrafico2 = JsonConvert.SerializeObject(DataPointAreas2);
                        }
                        catch (Exception ex)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                            pnl_Alert.Visible = true;
                        }
                        break;
                }
            }
            else
            {
                gerarGrafico = false;
            }
        }

        private double totalCompra(Compra compra)
        {
            double total = 0;
            List<ProdutosCompra> lista;

            using (DatabaseEntities context = new DatabaseEntities())
            {
                lista = context.ProdutosCompra.Where(p => p.idCompra == compra.idCompra).ToList();
                foreach (ProdutosCompra pCompra in lista)
                {
                    total += pCompra.quantidade * pCompra.Produto.precoCompra;
                }
            }

            return total;
        }
    }
}