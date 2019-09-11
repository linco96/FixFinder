<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="servico_Lista.aspx.cs" Inherits="FixFinder.Pages.servico_Lista" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de produto</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
</head>
<body>
    <form id="form1" runat="server">
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
                                    <a class="nav-link active" href="servico_Lista.aspx">
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
                        <h1 class="h2">Serviços</h1>
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

                        <%--FORMULÁRIO DE CADASTRO--%>
                        <div runat="server" id="form_Cadastro" visible="false">
                            <hr class="border-primary" />
                            <div class="form-group">
                                <label for="txt_Descricao">Descrição</label>
                                <asp:TextBox runat="server" ID="txt_Descricao" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txt_Valor">Valor</label>
                                <asp:TextBox runat="server" ID="txt_Valor" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btn_Cadastro" CssClass="btn btn-success" OnClick="btn_Cadastro_Click" Text="Cadastrar" />
                                <asp:Button runat="server" ID="btn_CancelarCadastro" Text="Cancelar" CssClass="btn btn-danger" OnClick="btn_CancelarCadastro_Click" formnovalidate="true" />
                            </div>
                        </div>

                        <%--FORMULÁRIO DE EDIÇÃO--%>
                        <div runat="server" id="form_Edicao" visible="false">
                            <hr class="border-primary" />
                            <h4 runat="server" id="head_Edicao"></h4>
                            <div class="form-group">
                                <label for="txt_DescricaoEdicao">Descrição</label>
                                <asp:TextBox runat="server" ID="txt_DescricaoEdicao" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txt_Valor">Valor</label>
                                <asp:TextBox runat="server" ID="txt_ValorEdicao" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btn_SalvarEdicao" CssClass="btn btn-success" OnClick="btn_SalvarEdicao_Click" Text="Salvar" />
                                <asp:Button runat="server" ID="btn_CancelarEdicao" Text="Cancelar" CssClass="btn btn-danger" OnClick="btn_CancelarEdicao_Click" formnovalidate="true" />
                            </div>
                        </div>

                        <hr class="border-primary" />
                        <div>
                            <asp:Button runat="server" ID="btn_CadastrarServico" Text="Novo serviço" CssClass="btn btn-outline-primary btn-sm mb-2" aria-pressed="true" OnClick="btn_CadastrarServico_Click" formnovalidate="true" />
                        </div>

                        <%--TABELA DE SERVIÇOS--%>
                        <div class="table-responsive-xl">
                            <asp:Table runat="server" ID="tbl_Servicos" CssClass="table border rounded-lg">
                                <asp:TableHeaderRow runat="server" ID="tblH_Veiculos" CssClass="thead-dark">
                                    <asp:TableHeaderCell Scope="Column" CssClass="text-center">Descrição</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Scope="Column" CssClass="text-center">Valor</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                </main>
            </div>
        </div>
    </form>
</body>
</html>