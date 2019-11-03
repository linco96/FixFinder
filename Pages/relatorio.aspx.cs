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
        public static String jsonGrafico3;
        public static String jsonGrafico4;

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

                if ((dtFim - dtInicio).TotalDays > 720)
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "O intervalo máximo para a geraçao de gráfico é de 2 anos";
                    return false;
                }

                if (dtInicio < new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                {
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "A data de início tem que ser maior que 01/01/2010";
                    return false;
                }

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

                            List<DataPointArea> dataPoints1 = new List<DataPointArea>();
                            List<DataPointArea> dataPoints2 = new List<DataPointArea>();

                            //dataPoints1.Add(new DataPointArea(data, valor));
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
                                DataPointArea dtp1, dtp2;
                                double mili;
                                long ticks;
                                DateTime dt;

                                dt = dtIncio;
                                ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                dt = new DateTime(ticks);
                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                    dtp1 = new DataPointArea(mili, 0);
                                    dtp2 = new DataPointArea(mili, 0);
                                    dataPoints1.Add(dtp1);
                                    dataPoints2.Add(dtp2);

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        dt = dt.AddMonths(1);
                                    }
                                }

                                foreach (KeyValuePair<Orcamento, DateTime> oConcluido in orcamentosConcluidos)
                                {
                                    dt = oConcluido.Value;
                                    ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                    dt = new DateTime(ticks);

                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in dataPoints1)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            dtpArea.Y += oConcluido.Key.valor;
                                            break;
                                        }
                                    }
                                }

                                //Despesa
                                List<Compra> listaCompra = context.Compra.Where(c => c.cnpjOficina.Equals(f.cnpjOficina) && c.data >= dtIncio && c.data <= dtFim).ToList();
                                listaCompra = listaCompra.OrderBy(c => c.data).ToList();
                                foreach (Compra compra in listaCompra)
                                {
                                    dt = compra.data;
                                    ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                    dt = new DateTime(ticks);
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in dataPoints2)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            dtpArea.Y += totalCompra(compra);
                                            break;
                                        }
                                    }
                                }
                            }
                            jsonGrafico = JsonConvert.SerializeObject(dataPoints1);
                            jsonGrafico2 = JsonConvert.SerializeObject(dataPoints2);
                        }
                        catch (Exception ex)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                            pnl_Alert.Visible = true;
                        }
                        break;

                    case "lucroBruto":
                        try
                        {
                            DateTime dtIncio = DateTime.Parse(txt_DataInicio.Text);
                            DateTime dtFim = DateTime.Parse(txt_DataFim.Text);
                            dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                            List<DataPointArea> dataPoints1 = new List<DataPointArea>();

                            //dataPoints1.Add(new DataPointArea(data, valor));
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

                                DataPointArea dtp1;
                                double mili;
                                long ticks;
                                DateTime dt;

                                dt = dtIncio;
                                ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                dt = new DateTime(ticks);
                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                    dtp1 = new DataPointArea(mili, 0);
                                    dataPoints1.Add(dtp1);

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        dt = dt.AddMonths(1);
                                    }
                                }

                                foreach (KeyValuePair<Orcamento, DateTime> oConcluido in orcamentosConcluidos)
                                {
                                    dt = oConcluido.Value;
                                    ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                    dt = new DateTime(ticks);

                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in dataPoints1)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            dtpArea.Y += oConcluido.Key.valor;
                                            break;
                                        }
                                    }
                                }

                                //Despesa
                                List<Compra> listaCompra = context.Compra.Where(c => c.cnpjOficina.Equals(f.cnpjOficina) && c.data >= dtIncio && c.data <= dtFim).ToList();
                                listaCompra = listaCompra.OrderBy(c => c.data).ToList();
                                foreach (Compra compra in listaCompra)
                                {
                                    dt = compra.data;
                                    ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                    dt = new DateTime(ticks);
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in dataPoints1)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            dtpArea.Y -= totalCompra(compra);
                                            break;
                                        }
                                    }
                                }
                                jsonGrafico = JsonConvert.SerializeObject(dataPoints1);
                            }
                        }
                        catch (Exception ex)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                            pnl_Alert.Visible = true;
                        }

                        break;

                    case "totalClientes":
                        try
                        {
                            DateTime dtIncio = DateTime.Parse(txt_DataInicio.Text);
                            DateTime dtFim = DateTime.Parse(txt_DataFim.Text);
                            dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                            List<DataPointArea> dataPoints1 = new List<DataPointArea>();

                            using (DatabaseEntities context = new DatabaseEntities())
                            {
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

                                DataPointArea dtp1;
                                double mili;
                                long ticks;
                                DateTime dt;

                                dt = dtIncio;
                                ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                dt = new DateTime(ticks);
                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                                    dtp1 = new DataPointArea(mili, 0);
                                    dataPoints1.Add(dtp1);

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        dt = dt.AddMonths(1);
                                    }
                                }

                                List<KeyValuePair<Cliente, DataPointArea>> listClientes = new List<KeyValuePair<Cliente, DataPointArea>>();
                                KeyValuePair<Cliente, DataPointArea> kvpCliente;

                                foreach (KeyValuePair<Orcamento, DateTime> oConcluido in orcamentosConcluidos)
                                {
                                    dt = oConcluido.Value;
                                    ticks = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                    dt = new DateTime(ticks);

                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    foreach (DataPointArea dtpArea in dataPoints1)
                                    {
                                        if (dtpArea.X == mili)
                                        {
                                            kvpCliente = listClientes.Find(l => l.Value.Equals(dtpArea) && l.Key.cpf.Equals(oConcluido.Key.cpfCliente));

                                            if (kvpCliente.Key == null)
                                            {
                                                dtpArea.Y++;
                                                listClientes.Add(new KeyValuePair<Cliente, DataPointArea>(oConcluido.Key.Cliente, dtpArea));
                                                break;
                                            }
                                        }
                                    }
                                }
                                jsonGrafico = JsonConvert.SerializeObject(dataPoints1);
                            }
                        }
                        catch (Exception ex)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                            pnl_Alert.Visible = true;
                        }

                        break;

                    case "historicoOrcamentoCriacao":
                        try
                        {
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                DateTime dtIncio = DateTime.Parse(txt_DataInicio.Text);
                                DateTime dtFim = DateTime.Parse(txt_DataFim.Text);
                                dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);
                                List<DataPointArea> dataPointsConcluido = new List<DataPointArea>();
                                List<DataPointArea> dataPointsCancelado = new List<DataPointArea>();
                                List<DataPointArea> dataPointsAprovacaoPendente = new List<DataPointArea>();
                                List<DataPointArea> dataPointsAprovado = new List<DataPointArea>();

                                List<Orcamento> orcamentos = context.Orcamento.Where(o => o.cnpjOficina.Equals(f.cnpjOficina) && o.data >= dtIncio && o.data <= dtFim).ToList();

                                //INICIO - Adiciona o datapoint na lista de datapoints
                                double mili;
                                long ticks;
                                DateTime dt;

                                ticks = new DateTime(dtIncio.Year, dtIncio.Month, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
                                dt = new DateTime(ticks);
                                int nConcluido, nCancelado, nAprovacaoPendente, nAprovado;
                                while (true)
                                {
                                    mili = dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

                                    nConcluido = 0;
                                    nCancelado = 0;
                                    nAprovacaoPendente = 0;
                                    nAprovado = 0;

                                    foreach (Orcamento orc in orcamentos)
                                    {
                                        if (orc.data.Year == dt.Year && orc.data.Month == dt.Month)
                                        {
                                            switch (orc.status)
                                            {
                                                case "Aprovado":
                                                    nAprovado++;
                                                    break;

                                                case "Pagamento pendente":
                                                    nAprovado++;
                                                    break;

                                                case "Aprovação da gerencia pendente":
                                                    nAprovacaoPendente++;
                                                    break;

                                                case "Aprovação do cliente pendente":
                                                    nAprovacaoPendente++;
                                                    break;

                                                case "Rejeitado pelo cliente":
                                                    nCancelado++;
                                                    break;

                                                case "Rejeitado pela gerencia":
                                                    nCancelado++;
                                                    break;

                                                case "Cancelado":
                                                    nCancelado++;
                                                    break;

                                                case "Concluído":
                                                    nConcluido++;
                                                    break;
                                            }
                                        }
                                    }

                                    dataPointsConcluido.Add(new DataPointArea(mili, nConcluido));
                                    dataPointsCancelado.Add(new DataPointArea(mili, nCancelado));
                                    dataPointsAprovacaoPendente.Add(new DataPointArea(mili, nAprovacaoPendente));
                                    dataPointsAprovado.Add(new DataPointArea(mili, nAprovado));

                                    if (dt.Year == dtFim.Year && dt.Month == dtFim.Month)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        dt = dt.AddMonths(1);
                                    }
                                }
                                //FIM --- Adiciona o datapoint na lista de datapoints

                                jsonGrafico = JsonConvert.SerializeObject(dataPointsConcluido);
                                jsonGrafico2 = JsonConvert.SerializeObject(dataPointsCancelado);
                                jsonGrafico3 = JsonConvert.SerializeObject(dataPointsAprovacaoPendente);
                                jsonGrafico4 = JsonConvert.SerializeObject(dataPointsAprovado);
                            }
                        }
                        catch (Exception ex)
                        {
                            pnl_Alert.CssClass = "alert alert-danger mt-3";
                            lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                            pnl_Alert.Visible = true;
                        }
                        break;

                    case "despesaFornecedor":
                        try
                        {
                            using (DatabaseEntities context = new DatabaseEntities())
                            {
                                DateTime dtIncio = DateTime.Parse(txt_DataInicio.Text);
                                DateTime dtFim = DateTime.Parse(txt_DataFim.Text);
                                dtFim = dtFim.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(59);

                                List<Compra> compras = context.Compra.Where(com => com.cnpjOficina.Equals(f.cnpjOficina) && com.data >= dtIncio && com.data <= dtFim).ToList();

                                List<DataPointPie> dataPoints = new List<DataPointPie>();
                                double total;
                                Fornecedor frn;
                                bool adicionado;
                                foreach (Compra com in compras)
                                {
                                    adicionado = false;
                                    total = totalCompra(com);
                                    frn = com.Fornecedor;
                                    foreach (DataPointPie dt in dataPoints)
                                    {
                                        if (dt.Label.Equals(frn.cnpjFornecedor))
                                        {
                                            dt.Y += total;
                                            adicionado = true;
                                        }
                                    }
                                    if (!adicionado)
                                    {
                                        dataPoints.Add(new DataPointPie(frn.cnpjFornecedor, total));
                                    }
                                }
                                Double aux;
                                foreach (DataPointPie dt in dataPoints)
                                {
                                    dt.Label = context.Fornecedor.Where(forn => forn.cnpjFornecedor.Equals(dt.Label)).FirstOrDefault().razaoSocial;
                                    aux = (Double)dt.Y;
                                    dt.Value = aux.ToString("0.00");
                                }

                                jsonGrafico = JsonConvert.SerializeObject(dataPoints);
                            }
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