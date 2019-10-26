<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orcamento_Avaliar.aspx.cs" Inherits="FixFinder.Pages.orcamento_Avaliar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avaliar Orçamento</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
</head>
<body>
    <form id="form_Avaliar" runat="server">
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
                                    <a class="nav-link" href="veiculo_Lista.aspx">
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
                                    <a class="nav-link active" href="orcamento_ListaCliente.aspx">
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
                                <li runat="server" id="btn_Relatorios" class="nav-item">
                                    <a class="nav-link" href="relatorio.aspx">
                                        <i class="fas fa-chart-pie fa-fw mr-1"></i>
                                        Relatórios
                                    </a>
                                </li>
                                <li runat="server" id="btn_Configuracoes" class="nav-item">
                                    <a class="nav-link" href="oficina_Configuracoes.aspx">
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
                        <h1 class="h2">Nova avaliação</h1>
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
                    <%--CONTEUDO--%>

                    <div class="container mt-5">
                        <%--AVALIACAOD DA OFICINA--%>
                        <div class="form-group">
                            <h2 style="text-align: center"><span runat="server" id="lbl_Oficina" class="align-middle mr-2">Paje Motors</span>
                                <img class="align-middle ml-2" src="../Content/star_32.png" />
                                <asp:Label runat="server" ID="lbl_Reputacao" CssClass="align-middle">7/10</asp:Label></h2>
                            <h5 runat="server" id="lbl_Endereco" class="text-muted" style="text-align: center">Rua DAS na Torre, 2062<br />
                                NteInteressa, Curitiba</h5>
                        </div>
                        <hr class="border border-primary border-bottom-0 mt-1 mb-1" />

                        <%--DADOS ORCAMENTO--%>
                        <div class="form-group">
                            <label for="txt_Veiculo">Veiculo</label>
                            <asp:TextBox runat="server" ID="txt_Veiculo" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txt_PrecoTotal">Preço Total</label>
                            <asp:TextBox runat="server" ID="txt_PrecoTotal" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>

                        <%--AVALIAR--%>
                        <div class="form-group">
                            <div class="form-inline">
                                <h4>Faça sua avaliação</h4>
                            </div>
                            <div class="form-inline">
                                <asp:RadioButtonList runat="server" ID="radio_AvaliacaoServico" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="false" sytle="display: block" CssClass="w-25 text-center">
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="form-inline">
                                <small class="form-text text-muted">Selecione 1 para muito ruim ou 10 para muito bom</small>
                            </div>
                            <%--COMENTARIO--%>
                            <div class="form-inline">
                                <h4>Escreva seu comentário</h4>
                            </div>
                            <div class="form-inline mt-1">
                                <span class="w-100 text-left">
                                    <asp:TextBox runat="server" ID="txt_Descrição" CssClass="form-control w-100" TextMode="MultiLine" autocomplete="off" Columns="100" Rows="3" MaxLength="200" required></asp:TextBox>
                                </span>
                            </div>
                            <%--COMENTARIO MECANICO--%>
                            <asp:Panel runat="server" ID="pnl_ComentarioOficinaTitulo" CssClass="form-inline" Visible="false">
                                <h4>Comentário da oficina</h4>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnl_ComentarioOficina" CssClass="form-inline" Visible="false">
                                <span class="w-100 text-left">
                                    <asp:TextBox runat="server" ID="txt_Comentario" CssClass="form-control w-100" TextMode="MultiLine" autocomplete="off" Columns="100" Rows="3" MaxLength="200" ReadOnly="true"></asp:TextBox>
                                </span>
                            </asp:Panel>
                        </div>
                        <div class="form-inline mt-1">
                            <asp:Panel ID="pnl_Alert" runat="server" Visible="false" CssClass="alert alert-success w-100" role="alert">
                                <asp:Label runat="server" ID="lbl_Alert" CssClass="form-text text-muted"></asp:Label>
                            </asp:Panel>
                        </div>
                        <div class="form-inline mt-1">
                            <asp:Button runat="server" ID="btn_Avaliar" Text="Avaliar" OnClick="btn_Avaliar_Click" CssClass="btn btn-success mr-1" />
                            <asp:Button runat="server" ID="btn_Cancelar" Text="Cancelar" OnClick="btn_Cancelar_Click" CssClass="btn btn-danger" formnovalidate />
                        </div>
                    </div>
                    <%--FIM DO CONTEUDO--%>
                </main>
            </div>
        </div>
    </form>
</body>
</html>