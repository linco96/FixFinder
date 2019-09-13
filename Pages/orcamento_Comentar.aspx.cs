using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class orcamento_Comentar : System.Web.UI.Page
    {
        private Cliente c;
        private Orcamento orcamento;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Session.Clear();
                Response.Redirect("login.aspx");
            }
            else
            {
                orcamento = (Orcamento)Session["orcamento"];
                if (orcamento != null)//o fera pode avaliar o proprio orcamento????
                {
                    try
                    {
                        using (var context = new DatabaseEntities())
                        {
                            Avaliacao avaliacao = context.Avaliacao.Where(a => a.idOrcamento == orcamento.idOrcamento && orcamento.cnpjOficina.Equals(orcamento.cnpjOficina)).FirstOrDefault();
                            if (avaliacao != null)
                            {
                                pnl_Alert.Visible = false;
                                if (avaliacao.comentarioFuncionario != null)
                                {
                                    txt_Comentario.ReadOnly = true;
                                    btn_Avaliar.Visible = false;
                                    btn_Cancelar.Text = "Voltar";
                                }
                                else
                                {
                                    txt_Comentario.ReadOnly = false;
                                    btn_Avaliar.Visible = true;
                                    btn_Cancelar.Text = "Cancelar";
                                }
                                if (!IsPostBack)
                                    preencher_Dados();
                            }
                            else
                            {
                                Session["orcamento"] = null;
                                Response.Redirect("orcamento_ListaOficina.aspx", false);
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
                    Response.Redirect("orcamento_ListaOficina.aspx", false);
                }
            }
        }

        private void preencher_Dados()
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    if (orcamento != null)
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
                                    lbl_Reputacao.Text = oficina.reputacao.ToString().Replace(",", ".") + "/10";
                                lbl_Endereco.InnerHtml = oficina.Endereco.logradouro + ", " + oficina.Endereco.numero + "<br />" + oficina.Endereco.cep + " - " + oficina.Endereco.cidade + " - " + oficina.Endereco.uf.ToUpper();
                                txt_Veiculo.Text = veiculo.marca + " " + veiculo.modelo + " " + veiculo.ano + " | " + veiculo.placa;
                                txt_PrecoTotal.Text = "R$ " + orcamento.valor.ToString("0.00");

                                Avaliacao avaliacao = context.Avaliacao.Where(a => a.idOrcamento == orcamento.idOrcamento && orcamento.cnpjOficina.Equals(orcamento.cnpjOficina)).FirstOrDefault();

                                if (avaliacao != null)
                                {
                                    txt_Descrição.Text = avaliacao.descricao;
                                    radio_AvaliacaoServico.SelectedIndex = int.Parse(avaliacao.notaServico.ToString());
                                    if (avaliacao.comentarioFuncionario != null)
                                    {
                                        txt_Comentario.Text = avaliacao.comentarioFuncionario;
                                        txt_Comentario.ReadOnly = true;
                                    }
                                    else
                                    {
                                        txt_Comentario.ReadOnly = false;
                                    }
                                }
                                else
                                {
                                    Session["orcamento"] = null;
                                    Response.Redirect("lista_OrcamentoOficina.aspx", false);
                                }
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
                    else
                    {
                        Session["orcamento"] = null;
                        Response.Redirect("orcamento_ListaOficina.aspx", false);
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
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        Avaliacao avaliacao = context.Avaliacao.Where(a => a.idOrcamento == orcamento.idOrcamento && a.cnpjOficina.Equals(orcamento.cnpjOficina)).FirstOrDefault();

                        if (avaliacao != null)
                        {
                            avaliacao.comentarioFuncionario = txt_Comentario.Text;
                            context.SaveChanges();
                            Session["orcamento"] = null;
                            Response.Redirect("orcamento_ListaOficina.aspx", false);
                        }
                        else
                        {
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Erro: Avaliação indisponível. Por favor entre em contato com o suporte";
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
            else
            {
                Session["orcamento"] = null;
                Response.Redirect("orcamento_ListaOficina.aspx", false);
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Session["orcamento"] = null;
            Response.Redirect("orcamento_ListaOficina.aspx", false);
        }
    }
}