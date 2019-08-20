using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
                //mandar pra home
            }
            else
            {
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    List<RequisicaoFuncionario> requisicoes = context.RequisicaoFuncionario.Where(req => req.cpfCliente.Equals(c.cpf)).ToList();
                    foreach (RequisicaoFuncionario req in requisicoes)
                    {
                        div_Cards.InnerHtml += "<div class='card text-center w-25 ml-3 mr-3 mt-5'>"
                                            + "<div class='card-header'>"
                                            + "<h4 class='card-title pt-2'>" + req.Oficina.nome + "</h4>"
                                            + "</div>"
                                            + "<div class='card-body'>"
                                            + "<h5 class='card-title'>Cargo: " + req.cargo + "</h5>"
                                            + "<h5 class='card-title'>Salário: R$ " + req.salario + "</h5>"
                                            + "<p class='card-text'>Dados bancários:<br />Banco: " + req.banco + "<br />Agência: " + req.agencia + "<br />Conta: " + req.conta + "</p>"
                                            + "<a href = '#' class='btn btn-primary mr-1'>Aceitar</a>"
                                            + "<a href = '#' class='btn btn-danger ml-1'>Rejeitar</a>"
                                            + "</div></div>";
                    }
                }
            }
        }
    }
}