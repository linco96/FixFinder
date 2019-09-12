using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class fornecedor_Editar : System.Web.UI.Page
    {
        private Fornecedor fornecedor;
        private Cliente c;
        private Funcionario funcionario;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
                Response.Redirect("login.aspx", false);
            else
            {
                try
                {
                    using (var context = new DatabaseEntities())
                    {
                        funcionario = context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();
                        if (funcionario == null)
                        {
                            Response.Redirect("home.aspx", false);
                        }
                        else
                        {
                            if (context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault() != null)
                            {
                                fornecedor = (Fornecedor)Session["fornecedor"];
                                fornecedor = context.Fornecedor.Where(f => f.idFornecedor == fornecedor.idFornecedor).FirstOrDefault();
                                if (fornecedor != null)
                                {
                                    lbl_Nome.Text = c.nome;
                                    if (funcionario == null)
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
                                        lbl_Nome.Text += " | " + funcionario.Oficina.nome;
                                        if (funcionario.cargo.ToLower().Equals("gerente"))
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
                                else
                                {
                                    Response.Redirect("fornecedor_Lista.aspx", false);
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('erro no banco - Oficina nao cadastrada');</script>");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void page_LoadComplete(object sender, EventArgs e)
        {
            fornecedor = (Fornecedor)Session["fornecedor"];
            if (fornecedor != null)
            {
                txt_CNPJ.Text = fornecedor.cnpjFornecedor;
                txt_RazaoSocial.Text = fornecedor.razaoSocial;
                txt_Email.Text = fornecedor.email;
                txt_Telefone.Text = fornecedor.telefone;
            }
        }

        protected void btn_Editar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new DatabaseEntities())
                {
                    fornecedor = context.Fornecedor.Where(f => f.idFornecedor == fornecedor.idFornecedor).FirstOrDefault();
                    fornecedor.email = txt_Email.Text;
                    fornecedor.telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    fornecedor.razaoSocial = txt_RazaoSocial.Text;
                    context.SaveChanges();
                    Session["fornecedor"] = null;
                    Response.Redirect("fornecedor_Lista.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btn_Cancelar_Click(object sender, EventArgs e)
        {
            Session["fornecedor"] = null;
            Response.Redirect("fornecedor_Lista.aspx", false);
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("login.aspx", false);
        }
    }
}