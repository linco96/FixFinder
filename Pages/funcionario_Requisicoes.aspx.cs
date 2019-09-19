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
        private static List<Button> aceitarButtons;
        private static List<Button> rejeitarButtons;
        private static List<Label> labels;
        private static List<Button> confirmarButtons;
        private static List<Button> recusarButtons;

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
                if (aceitarButtons == null)
                    aceitarButtons = new List<Button>();
                else
                    aceitarButtons.Clear();
                if (rejeitarButtons == null)
                    rejeitarButtons = new List<Button>();
                else
                    rejeitarButtons.Clear();
                if (labels == null)
                    labels = new List<Label>();
                else
                    labels.Clear();
                if (confirmarButtons == null)
                    confirmarButtons = new List<Button>();
                else
                    confirmarButtons.Clear();
                if (recusarButtons == null)
                    recusarButtons = new List<Button>();
                else
                    recusarButtons.Clear();
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
                    HtmlGenericControl hr;
                    Label label;
                    Button btn;
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

                        label = new Label();
                        label.CssClass = "card-text";
                        label.Text = "Dados bancários:<br />Banco: " + req.banco + "<br />Agência: " + req.agencia + "<br />Conta: " + req.conta + "<br />";

                        hr = new HtmlGenericControl("hr");
                        hr.Attributes.Add("class", "border-dark mt-3 mb-3");

                        body.Controls.Add(cargo);
                        body.Controls.Add(salario);
                        body.Controls.Add(label);
                        body.Controls.Add(hr);

                        btn = new Button();
                        btn.Click += new EventHandler(btn_Confirmacao_Click);
                        btn.ID = "btn_Aceitar" + req.cnpjOficina;
                        btn.Text = "Aceitar";
                        btn.CssClass = "btn btn-success mr-1";
                        btn.CommandArgument = req.Oficina.cnpj;
                        btn.CommandName = "Aceite";
                        body.Controls.Add(btn);
                        aceitarButtons.Add(btn);

                        btn = new Button();
                        btn.Click += new EventHandler(btn_Confirmacao_Click);
                        btn.ID = "btn_Rejeitar" + req.cnpjOficina;
                        btn.Text = "Rejeitar";
                        btn.CssClass = "btn btn-danger ml-1";
                        btn.CommandArgument = req.Oficina.cnpj;
                        btn.CommandName = "Rejeite";
                        body.Controls.Add(btn);
                        rejeitarButtons.Add(btn);

                        label = new Label();
                        label.CssClass = "card-text font-weight-bold";
                        label.ID = "lbl_" + req.cnpjOficina;
                        label.Text = "Teu cu";
                        label.Visible = false;
                        body.Controls.Add(label);
                        labels.Add(label);

                        btn = new Button();
                        btn.Click += new EventHandler(btn_Confirmar_Click);
                        btn.ID = "btn_Confirmar" + req.cnpjOficina;
                        btn.Text = "Sim";
                        btn.CssClass = "btn btn-success mr-1 mt-3";
                        btn.CommandArgument = req.Oficina.cnpj;
                        btn.Visible = false;
                        body.Controls.Add(btn);
                        confirmarButtons.Add(btn);

                        btn = new Button();
                        btn.Click += new EventHandler(btn_Recusar_Click);
                        btn.ID = "btn_Recusar" + req.cnpjOficina;
                        btn.Text = "Não";
                        btn.CssClass = "btn btn-danger ml-1 mt-3";
                        btn.CommandArgument = req.Oficina.cnpj;
                        btn.Visible = false;
                        body.Controls.Add(btn);
                        recusarButtons.Add(btn);

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

        protected void btn_Confirmacao_Click(object sender, EventArgs e)
        {
            Button origin = sender as Button;
            try
            {
                Button btn_Aceitar = new Button(), btn_Rejeitar = new Button(), btn_Confirmar = new Button(), btn_Recusar = new Button();
                Label label = new Label();
                foreach (Button btn in aceitarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Aceitar = btn;
                        break;
                    }
                }
                foreach (Button btn in rejeitarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Rejeitar = btn;
                        break;
                    }
                }
                foreach (Button btn in confirmarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Confirmar = btn;
                        break;
                    }
                }
                foreach (Button btn in recusarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Recusar = btn;
                        break;
                    }
                }
                foreach (Label lbl in labels)
                {
                    if (lbl.ID.Split('_')[1].Equals(origin.CommandArgument))
                    {
                        label = lbl;
                        break;
                    }
                }

                btn_Aceitar.Visible = false;
                btn_Rejeitar.Visible = false;

                label.Visible = true;

                btn_Confirmar.Visible = true;
                btn_Recusar.Visible = true;

                if (origin.CommandName.Equals("Aceite"))
                {
                    btn_Confirmar.CommandName = "Aceite";
                    btn_Recusar.CommandName = "Aceite";
                    label.Text = "Tem certeza que deseja confirmar esta requisição?<br />";
                }
                else
                {
                    btn_Confirmar.CommandName = "Rejeite";
                    btn_Recusar.CommandName = "Rejeite";
                    label.Text = "Tem certeza que deseja rejeitar esta requisição?<br />";
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
            Button origin = sender as Button;
            try
            {
                if (origin.CommandName.Equals("Aceite"))
                {
                    aceitarRequisicao(origin.CommandArgument);
                }
                else
                {
                    rejeitarRequisicao(origin.CommandArgument);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Recusar_Click(object sender, EventArgs e)
        {
            Button origin = sender as Button;
            try
            {
                Button btn_Aceitar = new Button(), btn_Rejeitar = new Button(), btn_Confirmar = new Button(), btn_Recusar = new Button();
                Label label = new Label();
                foreach (Button btn in aceitarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Aceitar = btn;
                        break;
                    }
                }
                foreach (Button btn in rejeitarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Rejeitar = btn;
                        break;
                    }
                }
                foreach (Button btn in confirmarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Confirmar = btn;
                        break;
                    }
                }
                foreach (Button btn in recusarButtons)
                {
                    if (btn.CommandArgument.Equals(origin.CommandArgument))
                    {
                        btn_Recusar = btn;
                        break;
                    }
                }
                foreach (Label lbl in labels)
                {
                    if (lbl.ID.Split('_')[1].Equals(origin.CommandArgument))
                    {
                        label = lbl;
                        break;
                    }
                }

                btn_Aceitar.Visible = true;
                btn_Rejeitar.Visible = true;

                label.Visible = false;

                btn_Confirmar.Visible = false;
                btn_Recusar.Visible = false;
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void aceitarRequisicao(string cnpj)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    RequisicaoFuncionario req = context.RequisicaoFuncionario.Where(requisicao => requisicao.cpfCliente == c.cpf && requisicao.cnpjOficina == cnpj).FirstOrDefault();
                    Funcionario funcionario = new Funcionario
                    {
                        cpf = c.cpf,
                        cnpjOficina = cnpj,
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

        protected void rejeitarRequisicao(string cnpj)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    RequisicaoFuncionario req = context.RequisicaoFuncionario.Where(requisicao => requisicao.cpfCliente == c.cpf && requisicao.cnpjOficina == cnpj).FirstOrDefault();
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
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }
    }
}