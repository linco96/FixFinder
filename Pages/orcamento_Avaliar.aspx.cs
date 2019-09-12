using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class orcamento_Avaliar : System.Web.UI.Page
    {
        private Cliente c;
        private Orcamento orcamento;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Session["orcamento"] = null;
                Response.Redirect("login.aspx", false);
            }
            else
            {
                orcamento = (Orcamento)Session["orcamento"];
                if (orcamento != null && orcamento.cpfCliente == c.cpf)
                {
                    pnl_Alert.Visible = false;
                    if (!IsPostBack)
                        preencher_Orcamento(orcamento);
                }
                else
                {
                    Response.Redirect("orcamento_ListaCliente.aspx", false);
                }
            }
        }

        private void preencher_Orcamento(Orcamento orcamento)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    Oficina oficina = context.Oficina.Where(o => o.cnpj.Equals(orcamento.cnpjOficina)).FirstOrDefault();
                    if (oficina != null)
                    {
                        Veiculo veiculo = context.Veiculo.Where(v => v.idVeiculo == orcamento.idVeiculo).FirstOrDefault();
                        if (veiculo != null)
                        {
                            lbl_Oficina.InnerHtml = oficina.nome;
                            if (oficina.reputacao == null)
                                lbl_Reputacao.Text = "-/10";
                            else
                                lbl_Reputacao.Text = oficina.reputacao + "/10";
                            lbl_Endereco.InnerHtml = oficina.Endereco.logradouro + ", " + oficina.Endereco.numero + "<br />" + oficina.Endereco.cep + " - " + oficina.Endereco.cidade + " - " + oficina.Endereco.uf.ToUpper();
                            txt_Veiculo.Text = veiculo.marca + " " + veiculo.modelo + " " + veiculo.ano + " | " + veiculo.placa;
                            txt_PrecoTotal.Text = "R$ " + orcamento.valor.ToString("0.00");
                        }
                        else
                        {
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "O veículo utilizado nesse orçamento não existe mais";
                            pnl_Alert.Visible = true;
                        }
                    }
                    else
                    {
                        pnl_Alert.CssClass = "alert alert-danger";
                        lbl_Alert.Text = "A oficina responsável por esse orçamento não existe mais";
                        pnl_Alert.Visible = true;
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

        protected void btn_Avaliar_Click(object sender, EventArgs e)
        {
            if (orcamento != null)
            {
                if (radio_AvaliacaoServico.SelectedItem != null)
                {
                    int nota;
                    Double media;
                    nota = int.Parse(radio_AvaliacaoServico.SelectedValue);
                    try
                    {
                        using (var context = new DatabaseEntities())
                        {
                            Avaliacao avaliacao;
                            List<Avaliacao> avaliacoes = context.Avaliacao.Where(a => a.cnpjOficina.Equals(orcamento.cnpjOficina)).ToList();
                            if (avaliacoes != null)
                            {
                                if (avaliacoes.Count > 0)
                                {
                                    avaliacao = new Avaliacao()
                                    {
                                        cnpjOficina = orcamento.cnpjOficina,
                                        cpfCliente = c.cpf,
                                        descricao = txt_Descrição.Text,
                                    };
                                }
                            }
                            else
                            {
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
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Selecione uma nota para registrar sua avaliação";
                    pnl_Alert.Visible = true;
                }
            }
            else
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Ocorreu um erro com o orcamento";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Session["orcamento"] = null;
            Response.Redirect("orcamento_ListaCliente.aspx", false);
        }
    }
}