using FixFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FixFinder.Pages
{
    public partial class oficina_Configuracoes : System.Web.UI.Page
    {
        private Cliente c;
        private Funcionario funcionario;
        private Oficina oficina;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    c = (Cliente)Session["usuario"];
        //    if (c == null)
        //    {
        //        Response.Redirect("login.aspx", false);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            using (var context = new DatabaseEntities())
        //            {
        //                funcionario = context.Funcionario.Where(f => f.cpf.Equals(c.cpf)).FirstOrDefault();
        //                if (funcionario != null)
        //                {
        //                    oficina = context.Oficina.Where(o => o.cnpj.Equals(funcionario.cnpjOficina)).FirstOrDefault();
        //                    if (oficina != null)
        //                    {
        //                        pnl_Alert.Visible = false;
        //                        pnl_AlerSalvar.Visible = false;
        //                        if (!IsPostBack)
        //                            preencher_Campos();
        //                    }
        //                    else
        //                    {
        //                        Response.Redirect("home.aspx", false);
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Redirect("home.aspx", false);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write("<script>alert('" + ex.Message + "');</script>");
        //        }
        //    }
        //}

        //private void preencher_Campos()
        //{
        //    Endereco endereco;
        //    FotoOficina foto;
        //    try
        //    {
        //        using (var context = new DatabaseEntities())
        //        {
        //            txt_OficinaCNPJ.Text = oficina.cnpj;
        //            txt_OficinaNome.Text = oficina.nome;

        //            endereco = context.Endereco.Where(e => e.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();

        //            if (endereco != null)
        //            {
        //                txt_Rua.Text = endereco.logradouro;
        //                txt_Numero.Text = endereco.numero.ToString();
        //                if (endereco.complemento != null)
        //                    txt_Complemento.Text = endereco.complemento;
        //                txt_CEP.Text = endereco.cep;
        //                txt_Cidade.Text = endereco.cidade;
        //                txt_UF.Text = endereco.uf;
        //            }

        //            txt_Telefone.Text = oficina.telefone;
        //            txt_Email.Text = oficina.email;
        //            txt_Abertura.Text = oficina.horaAbertura.Hours.ToString("00") + ":" + oficina.horaAbertura.Minutes.ToString("00");
        //            txt_Fechamento.Text = oficina.horaFechamento.Hours.ToString("00") + ":" + oficina.horaFechamento.Minutes.ToString("00");
        //            if (oficina.descricao != null)
        //                txt_Descrição.Text = oficina.descricao;

        //            txt_DuracaoAtendimento.Text = oficina.duracaoAtendimento.Hours.ToString("00") + ":" + oficina.duracaoAtendimento.Minutes.ToString("00");
        //            txt_CapacidadeAtendimento.Text = oficina.capacidadeAgendamentos.ToString();

        //            //Foto
        //            foto = context.FotoOficina.Where(f => f.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();
        //            if (foto != null)
        //            {
        //                String base64 = Convert.ToBase64String(foto.foto);
        //                img_Oficina.ImageUrl = "data:Image/png;base64," + base64;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('" + ex.Message + "');</script>");
        //    }
        //}

        //protected void btn_CarregarImagem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (oficina != null)
        //        {
        //            HttpPostedFile arquivo = fileUpload.PostedFile;
        //            String arquivoNome = Path.GetFileName(arquivo.FileName);
        //            String arquivoExtensao = Path.GetExtension(arquivoNome);
        //            int arquivoTamanho = arquivo.ContentLength;

        //            if (arquivoTamanho > 0)
        //            {
        //                if (arquivoExtensao.ToLower() == ".jpg" || arquivoExtensao.ToLower() == ".png" || arquivoExtensao.ToLower() == ".jpeg")
        //                {
        //                    if (arquivoTamanho <= 5000000)
        //                    {
        //                        using (var context = new DatabaseEntities())
        //                        {
        //                            FotoOficina foto = context.FotoOficina.Where(f => f.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();

        //                            Stream stream = arquivo.InputStream;
        //                            BinaryReader bReader = new BinaryReader(stream);

        //                            if (foto == null)
        //                            {
        //                                foto = new FotoOficina()
        //                                {
        //                                    cnpjOficina = oficina.cnpj,
        //                                    nomeFoto = "fotoOficina",
        //                                    foto = bReader.ReadBytes((int)stream.Length)
        //                                };
        //                                context.FotoOficina.Add(foto);
        //                            }
        //                            else
        //                            {
        //                                foto.foto = bReader.ReadBytes((int)stream.Length);
        //                            }
        //                            context.SaveChanges();
        //                            String base64 = Convert.ToBase64String(foto.foto);
        //                            img_Oficina.ImageUrl = "data:Image/png;base64," + base64;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        pnl_Alert.Visible = true;
        //                        lbl_Alert.Text = "O arquivo não pode ser maior que 5MB";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                pnl_Alert.Visible = true;
        //                lbl_Alert.Text = "Apenas imagens (.png, . jpg e .jpeg) podem ser enviadas";
        //            }
        //        }
        //        else
        //        {
        //            Response.Write("<script>alert('Erro no BD, Oficina nao encontrada no BD');</script>");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('" + ex.Message + "');</script>");
        //    }
        //}

        //protected void btn_Salvar_Click(object sender, EventArgs e)
        //{
        //    if (oficina != null)
        //    {
        //        try
        //        {
        //            using (var context = new DatabaseEntities())
        //            {
        //                Endereco endereco = context.Endereco.Where(end => end.cnpjOficina.Equals(oficina.cnpj)).FirstOrDefault();

        //                if (endereco != null)
        //                {
        //                    endereco.cep = txt_CEP.Text.Replace("-", "");
        //                    endereco.cidade = txt_Cidade.Text;
        //                    endereco.cnpjOficina = oficina.cnpj;
        //                    endereco.complemento = txt_Complemento.Text;
        //                    endereco.logradouro = txt_Rua.Text;
        //                    endereco.numero = int.Parse(txt_Numero.Text);
        //                    endereco.uf = txt_UF.Text;
        //                    context.SaveChanges();
        //                    pnl_AlerSalvar.Visible = true;
        //                }
        //                else
        //                {
        //                    pnl_AlerSalvar.Visible = true;
        //                    pnl_AlerSalvar.CssClass = "alert-danger";
        //                    lblAlertSalvar.Text = "O endereco nao foi localizado no banco de dados";
        //                }

        //                oficina = context.Oficina.Where(o => o.cnpj.Equals(oficina.cnpj)).FirstOrDefault();

        //                if (oficina != null)
        //                {
        //                    oficina.nome = txt_OficinaNome.Text;
        //                    oficina.telefone = txt_Telefone.Text.Replace("-", "").Replace("(", "").Replace(")", "");
        //                    oficina.email = txt_Email.Text;
        //                    oficina.horaAbertura = TimeSpan.Parse(txt_Abertura.Text);
        //                    oficina.horaFechamento = TimeSpan.Parse(txt_Fechamento.Text);
        //                    oficina.descricao = txt_Descrição.Text;
        //                    oficina.capacidadeAgendamentos = int.Parse(txt_CapacidadeAtendimento.Text);
        //                    oficina.duracaoAtendimento = TimeSpan.Parse(txt_DuracaoAtendimento.Text);
        //                    context.SaveChanges();
        //                    pnl_AlerSalvar.Visible = true;
        //                }
        //                else
        //                {
        //                    pnl_AlerSalvar.Visible = true;
        //                    pnl_AlerSalvar.CssClass = "alert-danger";
        //                    lblAlertSalvar.Text = "A oficina nao foi localizada no banco de dados";
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write("<script>alert('" + ex.Message + "');</script>");
        //        }
        //    }
        //}
    }
}