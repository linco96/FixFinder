using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class funcionario_Cadastro : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private Oficina o;

        protected void Page_Load(object sender, EventArgs e)
        {
            c = (Cliente)Session["usuario"];
            if (c == null)
            {
                Response.Redirect("login.aspx", false);
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    c = context.Cliente.Where(cliente => cliente.cpf.Equals(c.cpf)).FirstOrDefault();
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                    o = context.Oficina.Where(oficina => oficina.cnpj.Equals(c.Funcionario.cnpjOficina)).FirstOrDefault();
                    if (o == null || f == null || f.cargo.ToLower() != "gerente")
                    {
                        //mandar pra home
                    }
                }
            }
        }

        protected void btn_Registro_Click(object sender, EventArgs e)
        {
            try
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    RequisicaoFuncionario req = new RequisicaoFuncionario
                    {
                        cpfCliente = txt_CPF.Text.Replace(".", "").Replace("-", ""),
                        cargo = txt_Cargo.Text,
                        salario = double.Parse(txt_Salario.Text),
                        banco = int.Parse(ddl_Banco.SelectedValue),
                        agencia = txt_Agencia.Text,
                        conta = txt_Conta.Text,
                        cnpjOficina = o.cnpj
                    };
                    context.RequisicaoFuncionario.Add(req);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
                pnl_Alert.Visible = true;
            }
        }

        protected void btn_Puxar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_CPF.Text.Length > 0)
                {
                    using (DatabaseEntities context = new DatabaseEntities())
                    {
                        Cliente nFuncionario = context.Cliente.Where(cliente => cliente.cpf.Equals(txt_CPF.Text.Replace(".", "").Replace("-", ""))).FirstOrDefault();
                        if (nFuncionario == null)
                        {
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Não encontramos um usuário com o CPF especificado";
                            pnl_Alert.Visible = true;
                        }
                        else if (nFuncionario.Funcionario != null || nFuncionario.cpf.Equals(c.cpf))
                        {
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Este usuário não está disponível";
                            pnl_Alert.Visible = true;
                        }
                        else
                        {
                            txt_Nome.Text = nFuncionario.nome;
                            txt_Telefone.Text = nFuncionario.telefone;
                            txt_Email.Text = nFuncionario.email;
                            pnl_Alert.Visible = false;
                        }
                    }
                }
                else
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    lbl_Alert.Text = "Informe um CPF por favor";
                    pnl_Alert.Visible = true;
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