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
                    try
                    {
                        using (var context = new DatabaseEntities())
                        {
                            if (context.Avaliacao.Where(a => a.idOrcamento == orcamento.idOrcamento && a.cpfCliente.Equals(c.cpf) && orcamento.cnpjOficina.Equals(orcamento.cnpjOficina)).FirstOrDefault() != null)
                            {
                                btn_Avaliar.Visible = false;
                                btn_Cancelar.Text = "Voltar";
                                txt_Descrição.ReadOnly = true;
                                radio_AvaliacaoServico.Enabled = false;
                            }
                            else
                            {
                                btn_Avaliar.Visible = true;
                                txt_Descrição.ReadOnly = false;
                                radio_AvaliacaoServico.Enabled = true;
                            }

                            pnl_Alert.Visible = false;
                            if (!IsPostBack)
                                preencher_Orcamento(orcamento);

                            //CODIGO DASHBOARD
                            Funcionario f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
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
                                lbl_Reputacao.Text = oficina.reputacao.ToString().Replace(",", ".") + "/10";
                            lbl_Endereco.InnerHtml = oficina.Endereco.logradouro + ", " + oficina.Endereco.numero + "<br />" + oficina.Endereco.cep + " - " + oficina.Endereco.cidade + " - " + oficina.Endereco.uf.ToUpper();
                            txt_Veiculo.Text = veiculo.marca + " " + veiculo.modelo + " " + veiculo.ano + " | " + veiculo.placa;
                            txt_PrecoTotal.Text = "R$ " + orcamento.valor.ToString("0.00");

                            Avaliacao avaliacao = context.Avaliacao.Where(a => a.idOrcamento == orcamento.idOrcamento && a.cpfCliente.Equals(c.cpf) && orcamento.cnpjOficina.Equals(orcamento.cnpjOficina)).FirstOrDefault();

                            if (avaliacao != null)
                            {
                                txt_Descrição.Text = avaliacao.descricao;
                                radio_AvaliacaoServico.SelectedIndex = int.Parse(avaliacao.notaServico.ToString());
                                if (avaliacao.comentarioFuncionario != null)
                                {
                                    txt_Comentario.Text = avaliacao.comentarioFuncionario;
                                    pnl_ComentarioOficina.Visible = true;
                                    pnl_ComentarioOficinaTitulo.Visible = true;
                                }
                                else
                                {
                                    pnl_ComentarioOficina.Visible = false;
                                    pnl_ComentarioOficinaTitulo.Visible = false;
                                }
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
                    double nota;
                    nota = double.Parse(radio_AvaliacaoServico.SelectedValue);
                    Double media = nota;
                    try
                    {
                        using (var context = new DatabaseEntities())
                        {
                            Avaliacao avaliacao;
                            List<Avaliacao> avaliacoes = context.Avaliacao.Where(a => a.cnpjOficina.Equals(orcamento.cnpjOficina)).ToList();
                            if (avaliacoes.Count > 0)
                            {
                                avaliacao = new Avaliacao()
                                {
                                    cnpjOficina = orcamento.cnpjOficina,
                                    cpfCliente = c.cpf,
                                    descricao = txt_Descrição.Text,
                                    notaServico = nota,
                                    idOrcamento = orcamento.idOrcamento
                                };
                                context.Avaliacao.Add(avaliacao);
                                context.SaveChanges();

                                foreach (Avaliacao av in avaliacoes)
                                {
                                    media += av.notaServico;
                                }
                                media /= avaliacoes.Count + 1;

                                Oficina oficina = context.Oficina.Where(o => o.cnpj.Equals(orcamento.cnpjOficina)).FirstOrDefault();

                                if (oficina != null)
                                {
                                    oficina.reputacao = Math.Round(media, 1);
                                    context.SaveChanges();
                                    Session["orcamento"] = null;
                                    Response.Redirect("orcamento_ListaCliente.aspx", false);
                                }
                                else
                                {
                                    pnl_Alert.CssClass = "alert alert-danger";
                                    lbl_Alert.Text = "Erro: Oficina nao existe mais no banco";
                                    pnl_Alert.Visible = true;
                                }
                            }
                            else
                            {
                                avaliacao = new Avaliacao()
                                {
                                    cnpjOficina = orcamento.cnpjOficina,
                                    cpfCliente = c.cpf,
                                    descricao = txt_Descrição.Text,
                                    notaServico = nota,
                                    idOrcamento = orcamento.idOrcamento
                                };
                                context.Avaliacao.Add(avaliacao);
                                context.SaveChanges();
                                Oficina oficina = context.Oficina.Where(o => o.cnpj.Equals(orcamento.cnpjOficina)).FirstOrDefault();

                                if (oficina != null)
                                {
                                    oficina.reputacao = nota;
                                    context.SaveChanges();
                                    Session["orcamento"] = null;
                                    Response.Redirect("orcamento_ListaCliente.aspx", false);
                                }
                                else
                                {
                                    pnl_Alert.CssClass = "alert alert-danger";
                                    lbl_Alert.Text = "Erro: Oficina nao existe mais no banco";
                                    pnl_Alert.Visible = true;
                                }
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
                    lbl_Alert.Text = "Selecione uma nota de serviço para registrar sua avaliação";
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

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}