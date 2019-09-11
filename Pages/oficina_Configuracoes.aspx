<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oficina_Configuracoes.aspx.cs" Inherits="FixFinder.Pages.oficina_Configuracoes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configurações Oficina</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
</head>
<body>
    <form id="form_Configuracoes" runat="server">

        <nav class="navbar navbar-dark fixed-top bg-dark flex-md-nowrap p-0 shadow">
            <asp:Label runat="server" ID="lbl_Nome" CssClass="navbar-brand p-3 mr-0"></asp:Label>
            <ul class="navbar-nav px-3">
                <li class="nav-item text-nowrap">
                    <a class="nav-link text-white" href="oficina_Pesquisar.aspx">Pesquisar</a>
                </li>
            </ul>
            <ul class="navbar-nav px-3">
                <li class="nav-item text-nowrap">
                    <asp:LinkButton runat="server" ID="btn_Sair" OnClick="btn_Sair_Click" class="nav-link text-white" fromnovalidate>Sair</asp:LinkButton>
                </li>
            </ul>

            <a class="navbar-brand p-0 mr-0 mx-auto" href="oficina_Cadastro.aspx"></a>
            <ul class="navbar-nav px-3">
                <li runat="server" id="btn_CadastroOficina" class="nav-item text-nowrap">
                    <a class="nav-link text-white" href="oficina_Cadastro.aspx">Cadastre a sua oficina</a>
                </li>
            </ul>
            <span class="navbar-brand p-3 mr-0">FixFinder</span>
        </nav>

        <div class="container-fluid">
            <div class="row">
                <nav class="col-md-2 d-none d-md-block bg-light sidebar">
                    <div class="sidebar-sticky">
                        <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                            <span>Cliente</span>

                            <span class="d-flex align-items-center text-muted"></span>
                        </h6>

                        <div id="menu_cliente">
                            <ul class="nav flex-column">
                                <li class="nav-item">
                                    <a class="nav-link " href="veiculo_Lista.aspx">
                                        <i class="fas fa-car fa-1x fa-fw mr-1"></i>
                                        Meus veículos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="agendamento_ListaCliente.aspx">
                                        <i class="fas fa-calendar-alt fa-1x fa-fw mr-1"></i>
                                        Agendamentos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="orcamento_ListaCliente.aspx">
                                        <i class="fas fa-clipboard fa-1x fa-fw mr-1"></i>
                                        Orçamentos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="pagamento_ListaCliente.aspx">
                                        <i class="fas fa-dollar-sign fa-1x fa-fw mr-1"></i>
                                        Pagamentos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="cliente_EditarPerfil.aspx">
                                        <i class="fas fa-user-edit fa-1x fa-fw mr-1"></i>
                                        Editar perfil
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <asp:Panel runat="server" ID="pnl_Oficina">
                            <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                                <span>Oficina</span>
                            </h6>
                            <ul class="nav flex-column mb-2">
                                <li class="nav-item">
                                    <a class="nav-link" href="agendamento_ListaOficina.aspx">
                                        <i class="fas fa-calendar-alt fa-1x fa-fw mr-1"></i>
                                        Agendamentos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="orcamento_ListaOficina.aspx">
                                        <i class="fas fa-clipboard fa-1x fa-fw mr-1"></i>
                                        Orçamentos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="servico_Lista.aspx">
                                        <i class="fas fa-tools fa-1x fa-fw mr-1"></i>
                                        Serviços
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="produto_Lista.aspx">
                                        <i class="fas fa-oil-can fa-1x fa-fw mr-1"></i>
                                        Produtos
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="fornecedor_Lista.aspx">
                                        <i class="fas fa-boxes fa-1x fa-fw mr-1"></i>
                                        Fornecedores
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="compra_Lista.aspx">
                                        <i class="fas fa-shopping-basket fa-1x fa-fw mr-1"></i>
                                        Compras
                                    </a>
                                </li>
                                <li runat="server" id="btn_Funcionarios" class="nav-item">
                                    <a class="nav-link" href="funcionario_Lista.aspx">
                                        <i class="fas fa-user-friends fa-1x fa-fw mr-1"></i>
                                        Funcionários
                                    </a>
                                </li>
                                <li runat="server" id="btn_Configuracoes" class="nav-item">
                                    <a class="nav-link active" href="oficina_Configuracoes.aspx">
                                        <i class="fas fa-cog fa-1x fa-fw mr-1"></i>
                                        Configurações
                                    </a>
                                </li>
                            </ul>
                        </asp:Panel>

                        <%--SECAO DE ACEITAR CADASTRO NA OFICINA--%>
                        <asp:Panel runat="server" ID="pnl_Funcionario">
                            <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
                                <span>Oficina</span>
                            </h6>
                            <ul class="nav flex-column mb-2">
                                <li class="nav-item">
                                    <a class="nav-link" href="funcionario_Requisicoes.aspx">
                                        <i class="fas fa-user-plus fa-1x fa-fw mr-1"></i>
                                        Requisições<span runat="server" id="badge_Requisicoes" class="badge badge-danger ml-2"></span>
                                    </a>
                                </li>
                            </ul>
                        </asp:Panel>
                    </div>
                </nav>

                <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-4">
                    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                        <h1 class="h2">Configurações</h1>
                        <%--<div class="btn-toolbar mb-2 mb-md-0">
                            <div class="btn-group mr-2">
                                <button class="btn btn-sm btn-outline-secondary">Share</button>
                                <button class="btn btn-sm btn-outline-secondary">Export</button>
                            </div>
                            <button class="btn btn-sm btn-outline-secondary dropdown-toggle">
                                <span data-feather="calendar"></span>
                                This week
                            </button>
                        </div>--%>
                    </div>
                    <div class="container mt-5">

                        <div class="form-group">
                            <div class="form-inline">
                                <label class="h4">Oficina</label>
                            </div>
                            <div class="form-inline">
                                <span class="w-50 text-left pr-3">
                                    <label for="txt_OficinaCNPJ" style="display: block">CNPJ</label>
                                    <asp:TextBox runat="server" ID="txt_OficinaCNPJ" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3">
                                    <label for="txt_OficinaNome" style="display: block">Nome</label>
                                    <asp:TextBox runat="server" ID="txt_OficinaNome" CssClass="form-control w-100" autocomplete="off" MaxLength="100" required></asp:TextBox>
                                </span>
                            </div>
                        </div>

                        <%--<hr class="border border-primary border-bottom-0 mt-2 mb-1" />--%>

                        <%--<div class="form-inline">
                <label class="h3">Configurações</label>
            </div>--%>

                        <%--ENDERECO--%>
                        <div class="form-group">
                            <div class="form-inline">
                                <label class="h4">Endereço</label>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-75 text-left pr-3">
                                    <label for="txt_Rua" style="display: block">Rua</label>
                                    <asp:TextBox runat="server" ID="txt_Rua" CssClass="form-control w-100" MaxLength="200" required></asp:TextBox>
                                </span>
                                <span class="w-25 text-left">
                                    <label for="txt_Numero" style="display: block">Numero</label>
                                    <asp:TextBox runat="server" ID="txt_Numero" CssClass="form-control w-100" required></asp:TextBox>
                                </span>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-25 text-left pr-3">
                                    <label for="txt_Complemento" style="display: block">Complemento</label>
                                    <asp:TextBox runat="server" ID="txt_Complemento" CssClass="form-control w-100" MaxLength="50"></asp:TextBox>
                                </span>
                                <span class="w-25 text-left pr-3">
                                    <label for="txt_CEP" style="display: block">CEP</label>
                                    <asp:TextBox runat="server" ID="txt_CEP" CssClass="form-control w-100" onkeypress="$(this).mask('00000-000', {reverse: true});" required></asp:TextBox>
                                </span>
                                <span class="w-25 text-left pr-3">
                                    <label for="txt_Cidade" style="display: block">Cidade</label>
                                    <asp:TextBox runat="server" ID="txt_Cidade" CssClass="form-control w-100" MaxLength="50" required></asp:TextBox>
                                </span>
                                <span class="w-25 text-left">
                                    <label for="txt_UF" style="display: block">UF</label>
                                    <asp:TextBox runat="server" ID="txt_UF" CssClass="form-control w-100" MaxLength="2" required></asp:TextBox>
                                </span>
                            </div>
                        </div>

                        <%--<hr class="border border-primary border-bottom-0 mt-1 mb-1" />--%>

                        <%--DADOS OFICINA--%>
                        <div class="form-group">
                            <div class="form-inline">
                                <label class="h4">Dados Oficina</label>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-50 text-left pr-3">
                                    <label for="txt_Telefone" style="display: block">Telefone</label>
                                    <asp:TextBox runat="server" ID="txt_Telefone" onkeypress="$(this).mask('(00) 0 0000-0000');" CssClass="form-control w-100" required></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3">
                                    <label for="txt_Email" style="display: block">E-Mail</label>
                                    <asp:TextBox runat="server" ID="txt_Email" type="email" CssClass="form-control w-100" MaxLength="100" required></asp:TextBox>
                                </span>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-100 text-left">
                                    <label for="txt_Fechamento" style="display: block">Descrição</label>
                                    <asp:TextBox runat="server" ID="txt_Descrição" CssClass="form-control w-100" TextMode="MultiLine" autocomplete="off" Columns="100" Rows="3" MaxLength="500"></asp:TextBox>
                                </span>
                            </div>
                        </div>

                        <%--HORARIO DE FUNCIONAMENTO--%>
                        <div class="form-group">
                            <h4>Segunda à Sexta</h4>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_HorarioAberturaUtil" style="display: block">Horário de abertura</label>
                                <asp:TextBox runat="server" ID="txt_HorarioAberturaUtil" CssClass="form-control w-100" type="time"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3 mb-1">
                                <label for="txt_HorarioFechamentoUtil" style="display: block">Horário de fechamento</label>
                                <asp:TextBox runat="server" ID="txt_HorarioFechamentoUtil" CssClass="form-control w-100" type="time"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-group mt-3">
                            <h4>Sábado</h4>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_HorarioAberturaSabado" style="display: block">Horário de abertura</label>
                                <asp:TextBox runat="server" ID="txt_HorarioAberturaSabado" CssClass="form-control w-100" type="time"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3 mb-1">
                                <label for="txt_HorarioFechamentoSabado" style="display: block">Horário de fechamento</label>
                                <asp:TextBox runat="server" ID="txt_HorarioFechamentoSabado" CssClass="form-control w-100" type="time"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-group mt-3">
                            <h4>Domingo</h4>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_HorarioAberturaDomingo" style="display: block">Horário de abertura</label>
                                <asp:TextBox runat="server" ID="txt_HorarioAberturaDomingo" CssClass="form-control w-100" type="time"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3 mb-1">
                                <label for="txt_HorarioFechamentoDomingo" style="display: block">Horário de fechamento</label>
                                <asp:TextBox runat="server" ID="txt_HorarioFechamentoDomingo" CssClass="form-control w-100" type="time"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-group">
                            <small id="horario_Help" class="form-text text-muted">Caso a oficina não abra no dia especificado, deixe os campos vazios</small>
                        </div>

                        <%--<hr class="border border-primary border-bottom-0 mt-1 mb-1" />--%>

                        <%--AGENDAMENTOS--%>
                        <div class="form-group">
                            <div class="form-inline">
                                <label class="h4">Agendamentos</label>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-50 text-left pr-3">
                                    <label for="txt_DuracaoAtendimento" style="display: block">Duração do Atendimento</label>
                                    <asp:TextBox runat="server" ID="txt_DuracaoAtendimento" CssClass="form-control w-100" type="time" autocomplete="off" required></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3">
                                    <label for="txt_CapacidadeAtendimento" style="display: block">Capacidade de Atendimentos</label>
                                    <asp:TextBox runat="server" ID="txt_CapacidadeAtendimento" CssClass="form-control w-100" onkeypress="$(this).mask('#.##0', {reverse: true});" autocomplete="off" required></asp:TextBox>
                                </span>
                            </div>
                        </div>

                        <%--<hr class="border border-primary border-bottom-0 mt-1 mb-1" />--%>

                        <%--BUTAO--%>
                        <div class="form-group">
                            <div class="form-inline">
                                <span class="w-50 text-left">
                                    <asp:Button runat="server" ID="btn_Salvar" CssClass="btn btn-success" Text="Salvar" OnClick="btn_Salvar_Click" />
                                </span>
                            </div>
                            <div class="form-inline mt-1">
                                <asp:Panel runat="server" ID="pnl_AlerSalvar" Visible="false" CssClass="alert alert-success w-100" role="alert">
                                    <asp:Label runat="server" ID="lblAlertSalvar" CssClass="text-danger form-text text-muted">Informações alteradas com sucesso</asp:Label>
                                </asp:Panel>
                            </div>
                        </div>

                        <hr class="border border-primary border-bottom-0 mt-1 mb-1" />

                        <%--FOTO--%>
                        <div class="form-group">
                            <div class="form-inline">
                                <label class="h4">Alterar Foto</label>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-25 pr-3">
                                    <asp:Image runat="server" Style="max-width: 256px; max-height: 256px" CssClass="Responsive image rounded border border-info" ID="img_Oficina" ImageUrl="~/Content/no-image.png" />
                                </span>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-100 text-left">
                                    <asp:FileUpload runat="server" ID="fileUpload" />
                                </span>
                            </div>
                            <div class="form-inline mt-1">
                                <asp:Button runat="server" ID="btn_CarregarImagem" CssClass="btn btn-info btn-sm" formnovalidate Text="Alterar Foto" OnClick="btn_CarregarImagem_Click" />
                            </div>
                            <small class="form-text text-muted">Selecione uma imagem (.png, .jpg ou .jpeg) e clique em "Alterar Foto"</small>
                            <div class="form-inline mt-1">
                                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                                    <asp:Label runat="server" ID="lbl_Alert" CssClass="text-danger form-text text-muted"></asp:Label>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </main>
            </div>
        </div>
    </form>
</body>
</html>