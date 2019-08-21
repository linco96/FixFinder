using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class funcionario_Editar : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario f;
        private static bool alterar;

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
                    f = (Funcionario)Session["funcionario"];
                    if (f == null)
                    {
                        Response.Redirect("home.aspx", false);
                    }
                    else
                    {
                        if (!alterar)
                        {
                            f = context.Funcionario.Where(func => func.cpf.Equals(f.cpf)).FirstOrDefault();
                            txt_CPF.Text = f.cpf;
                            txt_Nome.Text = f.Cliente.nome;
                            txt_Cargo.Text = f.cargo;
                            txt_Salario.Text = f.salario.ToString();
                            ddl_Banco.SelectedValue = f.banco.ToString();
                            txt_Agencia.Text = f.agencia;
                            txt_Conta.Text = f.conta;
                            alterar = true;
                        }
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
                    f = context.Funcionario.Where(func => func.cpf.Equals(f.cpf)).FirstOrDefault();
                    f.cargo = txt_Cargo.Text;
                    f.salario = double.Parse(txt_Salario.Text);
                    f.banco = int.Parse(ddl_Banco.SelectedValue);
                    f.agencia = txt_Agencia.Text;
                    f.conta = txt_Conta.Text;
                    context.SaveChanges();

                    Session["funcionario"] = null;
                    Response.Redirect("funcionario_Lista.aspx", false);
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