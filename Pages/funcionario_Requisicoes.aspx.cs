using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class funcionario_Requisicoes : System.Web.UI.Page
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
                div_Cards.InnerHtml = "";
                carregar_Requisicoes();
                using (DatabaseEntities context = new DatabaseEntities())
                {
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
            if (Session["msgSuccess"] != null)
            {
                pnl_Alert.CssClass = "alert alert-success";
                lbl_Alert.Text = (String)Session["msgSuccess"];
                pnl_Alert.Visible = true;
            }
        }

        protected void carregar_Requisicoes()
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(req => req.cpfCliente.Equals(c.cpf)).ToList();
                    Panel card;
                    Panel header;
                    Panel body;
                    HtmlGenericControl nomeOficina;
                    HtmlGenericControl cargo;
                    HtmlGenericControl salario;
                    Label dadosBancarios;
                    Button btn_Aceitar;
                    Button btn_Rejeitar;
                    foreach (RequisicaoFuncionario req in requisicoes)
                    {
                        card = new Panel();
                        card.CssClass = "card text-center w-25 ml-3 mr-3 mt-5";

                        header = new Panel();
                        header.CssClass = "card-header";

                        nomeOficina = new HtmlGenericControl("h4");
                        nomeOficina.Attributes.Add("class", "card-title pt-2");
                        nomeOficina.InnerHtml = req.Oficina.nome;

                        header.Controls.Add(nomeOficina);
                        card.Controls.Add(header);

                        body = new Panel();
                        body.CssClass = "card-body";

                        cargo = new HtmlGenericControl("h5");
                        cargo.Attributes.Add("class", "card-title");
                        cargo.InnerHtml = "Cargo: " + req.cargo;

                        salario = new HtmlGenericControl("h5");
                        salario.Attributes.Add("class", "card-title");
                        salario.InnerHtml = "Salário: R$ " + req.salario;

                        dadosBancarios = new Label();
                        dadosBancarios.CssClass = "card-text";
                        dadosBancarios.Text = "Dados bancários:<br />Banco: " + req.banco + "<br />Agência: " + req.agencia + "<br />Conta: " + req.conta + "<br />";

                        btn_Aceitar = new Button();
                        btn_Aceitar.Click += new EventHandler(btn_Aceitar_Click);
                        btn_Aceitar.Text = "Aceitar";
                        btn_Aceitar.CssClass = "btn btn-primary mr-1 mt-3";
                        btn_Aceitar.CommandArgument = req.Oficina.cnpj;

                        btn_Rejeitar = new Button();
                        btn_Rejeitar.Click += new EventHandler(btn_Rejeitar_Click);
                        btn_Rejeitar.Text = "Rejeitar";
                        btn_Rejeitar.CssClass = "btn btn-danger ml-1 mt-3";
                        btn_Rejeitar.CommandArgument = req.Oficina.cnpj;

                        body.Controls.Add(cargo);
                        body.Controls.Add(salario);
                        body.Controls.Add(dadosBancarios);
                        body.Controls.Add(btn_Aceitar);
                        body.Controls.Add(btn_Rejeitar);

                        card.Controls.Add(body);
                        div_Cards.Controls.Add(card);
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

        protected void btn_Aceitar_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    RequisicaoFuncionario req = context.RequisicaoFuncionario.Where(requisicao => requisicao.cpfCliente == c.cpf && requisicao.cnpjOficina == btn.CommandArgument).FirstOrDefault();
                    Funcionario funcionario = new Funcionario
                    {
                        cpf = c.cpf,
                        cnpjOficina = btn.CommandArgument,
                        cargo = req.cargo,
                        salario = req.salario,
                        banco = req.banco,
                        agencia = req.agencia,
                        conta = req.conta
                    };
                    context.Funcionario.Add(funcionario);

                    List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(requisicao => requisicao.cpfCliente == c.cpf).ToList();
                    context.RequisicaoFuncionario.RemoveRange(requisicoes);

                    context.SaveChanges();

                    Session["msgSuccess"] = "Requisição aceita<br />As opções de funcionário estão agora disponíveis no menu lateral";

                    Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Rejeitar_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    RequisicaoFuncionario req = context.RequisicaoFuncionario.Where(requisicao => requisicao.cpfCliente == c.cpf && requisicao.cnpjOficina == btn.CommandArgument).FirstOrDefault();
                    context.RequisicaoFuncionario.Remove(req);
                    context.SaveChanges();

                    Session["msgSuccess"] = "Requisição rejeitada";

                    Response.Redirect(Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session["usuario"] = null;
            Response.Redirect("login.aspx", false);
        }
    }
}