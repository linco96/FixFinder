using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class pagamento : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private Orcamento o;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            o = (Orcamento)Session["orcamento"];
            if (c == null)
                Response.Redirect("login.aspx", false);
            else if (o == null)
                Response.Redirect("orcamento_ListaCliente.aspx", false);
            else if (!o.status.Equals("Pagamento pendente"))
                Response.Redirect("orcamento_ListaCliente.aspx", false);
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    Oficina oficina = context.Oficina.Where(of => of.cnpj.Equals(o.cnpjOficina)).FirstOrDefault();
                    lbl_Oficina.InnerText = oficina.nome;
                    if (oficina.reputacao == null)
                        lbl_Reputacao.Text = "-/10";
                    else
                        lbl_Reputacao.Text = oficina.reputacao + "/10";
                    lbl_Valor.InnerText = "Valor do pagamento: R$ " + o.valor.ToString("0.00");

                    if (!IsPostBack)
                    {
                        List<Cartao> cartoes = context.Cartao.Where(card => card.cpfCliente.Equals(c.cpf)).ToList();
                        if (cartoes.Count > 0)
                        {
                            ListItem item;
                            string mes;
                            foreach (Cartao card in cartoes)
                            {
                                if (card.mesVencimento < 10)
                                    mes = "0" + card.mesVencimento;
                                else
                                    mes = card.mesVencimento.ToString();

                                item = new ListItem();
                                item.Value = card.idCartao.ToString();
                                item.Text = card.bandeira.ToUpper() + " - " + card.numero.Substring(card.numero.Length - 4) + " - Vence em " + mes + "/" + card.anoVencimento;
                                txt_Cartao.Items.Add(item);
                            }
                        }
                        else
                        {
                            div_Quick.Visible = false;
                        }
                    }

                    //Dash Time
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
                }
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }

        protected void btn_Pagar_Click(object sender, EventArgs e)
        {
        }

        protected void txt_Cartao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    int id = int.Parse(txt_Cartao.SelectedValue);
                    Cartao card = context.Cartao.Where(cartao => cartao.idCartao == id).FirstOrDefault();
                    txt_NumeroCartao.Text = card.numero;
                    txt_Titular.Text = card.titular;

                    String mes;
                    if (card.mesVencimento < 10)
                        mes = "0" + card.mesVencimento;
                    else
                        mes = card.mesVencimento.ToString();
                    txt_Vencimento.Text = mes + "/" + card.anoVencimento;
                    div_Check.Visible = false;
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