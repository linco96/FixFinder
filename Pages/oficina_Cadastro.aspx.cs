using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FixFinder.Models;

namespace FixFinder.Pages
{
    public partial class oficina_Cadastro : System.Web.UI.Page
    {
        private Cliente c;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                c = (Cliente)Session["usuario"];

                if (c == null)
                {
                    Response.Redirect("login.aspx", false);
                }

                Funcionario f;
                using (DatabaseEntities context = new DatabaseEntities())
                {
                    f = context.Funcionario.Where(func => func.cpf.Equals(c.cpf)).FirstOrDefault();
                }

                if (f != null)
                {
                    Response.Redirect("home.aspx", false);
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }

        protected void btn_Cadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txt_HorarioAberturaUtil.Text.Length == 0 || txt_HorarioFechamentoUtil.Text.Length == 0) && (txt_HorarioAberturaSabado.Text.Length == 0 || txt_HorarioFechamentoSabado.Text.Length == 0) && (txt_HorarioAberturaDomingo.Text.Length == 0 || txt_HorarioFechamentoDomingo.Text.Length == 0))
                {
                    pnl_Alert.CssClass = "alert alert-danger";
                    pnl_Alert.Visible = true;
                    lbl_Alert.Text = "Informe os horários de abertura e fechamento de pelo menos um dia das semana";
                }
                else
                {
                    using (var context = new DatabaseEntities())
                    {
                        Oficina o = new Oficina
                        {
                            cnpj = txt_CNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", ""),
                            nome = txt_Nome.Text,
                            telefone = txt_Telefone.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                            email = txt_Email.Text,
                            capacidadeAgendamentos = int.Parse(num_Agendamentos.Text),
                            statusAssinatura = 1,
                            duracaoAtendimento = TimeSpan.Parse(txt_TempoAtendimento.Text)
                        };

                        Endereco endereco = new Endereco()
                        {
                            cep = txt_CEP.Text.Replace("-", ""),
                            cidade = txt_Cidade.Text,
                            cnpjOficina = o.cnpj,
                            complemento = txt_Complemento.Text,
                            logradouro = txt_Rua.Text,
                            numero = int.Parse(txt_Numero.Text),
                            uf = txt_UF.Text
                        };

                        if (context.Oficina.Where(oficina => oficina.cnpj.Equals(o.cnpj)).FirstOrDefault() != null)
                        {
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-danger";
                            lbl_Alert.Text = "Uma oficina com este CNPJ já existe";
                        }
                        else
                        {
                            context.Oficina.Add(o);
                            context.Endereco.Add(endereco);

                            DiaFuncionamento dia;
                            if (txt_HorarioAberturaUtil.Text.Length > 0 && txt_HorarioFechamentoUtil.Text.Length > 0)
                            {
                                dia = new DiaFuncionamento()
                                {
                                    diaSemana = "util",
                                    cnpjOficina = o.cnpj,
                                    horaAbertura = TimeSpan.Parse(txt_HorarioAberturaUtil.Text),
                                    horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoUtil.Text)
                                };
                                context.DiaFuncionamento.Add(dia);
                                context.SaveChanges();
                            }
                            if (txt_HorarioAberturaSabado.Text.Length > 0 && txt_HorarioFechamentoSabado.Text.Length > 0)
                            {
                                dia = new DiaFuncionamento()
                                {
                                    diaSemana = "sabado",
                                    cnpjOficina = o.cnpj,
                                    horaAbertura = TimeSpan.Parse(txt_HorarioAberturaSabado.Text),
                                    horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoSabado.Text)
                                };
                                context.DiaFuncionamento.Add(dia);
                                context.SaveChanges();
                            }
                            if (txt_HorarioAberturaDomingo.Text.Length > 0 && txt_HorarioFechamentoDomingo.Text.Length > 0)
                            {
                                dia = new DiaFuncionamento()
                                {
                                    diaSemana = "domingo",
                                    cnpjOficina = o.cnpj,
                                    horaAbertura = TimeSpan.Parse(txt_HorarioAberturaDomingo.Text),
                                    horaFechamento = TimeSpan.Parse(txt_HorarioFechamentoDomingo.Text)
                                };
                                context.DiaFuncionamento.Add(dia);
                                context.SaveChanges();
                            }

                            Funcionario f = new Funcionario
                            {
                                cargo = "Gerente",
                                cpf = c.cpf,
                                cnpjOficina = o.cnpj,
                            };

                            context.Funcionario.Add(f);

                            context.SaveChanges();
                            pnl_Alert.Visible = true;
                            pnl_Alert.CssClass = "alert alert-success";
                            lbl_Alert.Text = "Oficina cadastrada com sucesso";

                            Response.Redirect("home.aspx", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pnl_Alert.CssClass = "alert alert-danger";
                pnl_Alert.Visible = true;
                lbl_Alert.Text = "Erro: " + ex.Message + Environment.NewLine + "Por favor entre em contato com o suporte";
            }
        }
    }
}