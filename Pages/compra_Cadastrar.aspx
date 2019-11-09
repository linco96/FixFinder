<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="compra_Cadastrar.aspx.cs" Inherits="FixFinder.Pages.compra_Cadastrar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nova Compra</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
</head>
<body>
    <form id="form_Compra" runat="server">
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
            <span class="navbar-brand p-3 mr-0"><a href="home.aspx" class="text-decoration-none text-white">FixFinder</a></span>
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
                                    <a class="nav-link active" href="compra_Lista.aspx">
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
                        <h1 class="h2">Cadastro de compra</h1>
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
                        <%--FORNECEDOR--%>
                        <div class="form-group">
                            <label class="h4">Fornecedor</label>
                            <asp:DropDownList runat="server" ID="select_Fornecedores" CssClass="form-control" OnSelectedIndexChanged="select_Fornecedores_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label runat="server" ID="lbl_Alert" CssClass="text-danger form-text text-muted" Text="Nenhum fornecedor cadastrado."></asp:Label>
                            </asp:Panel>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_FornecedorCNPJ" style="display: block">CNPJ</label>
                                <asp:TextBox runat="server" ID="txt_FornecedorCNPJ" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3">
                                <label for="txt_FornecedorNome" style="display: block">Nome</label>
                                <asp:TextBox runat="server" ID="txt_FornecedorNome" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_FornecedorTelefone" style="display: block">Telefone</label>
                                <asp:TextBox runat="server" ID="txt_FornecedorTelefone" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3">
                                <label for="txt_FornecedorEmail" style="display: block">E-mail</label>
                                <asp:TextBox runat="server" ID="txt_FornecedorEmail" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                        </div>

                        <hr class="border border-primary border-bottom-0" />

                        <%--PRODUTO--%>
                        <div class="form-group">
                            <label class="h4">Produto</label>
                            <asp:DropDownList runat="server" ID="select_Produto" CssClass="form-control" OnSelectedIndexChanged="select_Produto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Panel runat="server" ID="pnl_AlertProduto" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label runat="server" ID="lbl_AlertProduto" CssClass="text-danger form-text text-muted" Text="Nenhum produto cadastrado."></asp:Label>
                            </asp:Panel>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_Produto" style="display: block">Produto</label>
                                <asp:TextBox runat="server" ID="txt_Produto" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3 mb-1">
                                <label for="txt_ProdutoQuantidadeAtual" style="display: block">Quantidade atual</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoQuantidadeAtual" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_ProdutoMarca" style="display: block">Marca</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoMarca" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3 mb-1">
                                <label for="txt_ProdutoCategoria" style="display: block">Categoria</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoCategoria" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                        </div>

                        <div class="form-inline">
                            <span class="w-50 text-left pr-3">
                                <label for="txt_ProdutoPrecoCompra" style="display: block">Preço Compra (R$)</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoPrecoCompra" onkeypress="$(this).mask('#.##0,00', {reverse: true});" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3">
                                <label for="txt_ProdutoPrecoVenda" style="display: block">Preço Venda (R$)</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoPrecoVenda" onkeypress="$(this).mask('#.##0,00', {reverse: true});" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-inline">
                            <span class="w-50 text-left pr-3">
                                <label for="txt_ProdutoQuantidade" style="display: block">Quantidade</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoQuantidade" autocomplete="off" CssClass="form-control w-100" minlength="1" required onkeypress="$(this).mask('#.##0', {reverse: true});" placeholder="Digite a quantidade de produtos a serem comprados..."></asp:TextBox>
                            </span>
                            <span class="w-50 text-left pl-3">
                                <label for="txt_ProdutoValidade" style="display: block">Validade</label>
                                <asp:TextBox runat="server" ID="txt_ProdutoValidade" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                            </span>
                        </div>
                        <div class="form-group">
                            <asp:Button runat="server" ID="btn_AdicionarProduto" CssClass="btn btn-outline-success btn-sm mt-2" Text="Adicionar Produto" OnClick="btn_AdicionarProduto_Click" Enabled="false" />
                            <asp:Button runat="server" ID="btn_CadastrarProduto" CssClass="btn btn-outline-primary btn-sm mt-2" Text="Cadastrar Produto" OnClick="btn_CadastrarProduto_Click" formnovalidate />
                        </div>
                        <div class="form-group">
                            <asp:Panel runat="server" ID="pnl_AlertProdutoDuplicado" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label runat="server" ID="txt_AlertProduto" CssClass="text-danger form-text text-muted" Text="Não é possivel selecionar o mesmo produto"></asp:Label>
                            </asp:Panel>
                        </div>
                        <%--LISTA PRODUTOS--%>
                        <div class="table-responsive-xl">
                            <asp:Table runat="server" ID="tbl_Produtos" CssClass="table border rounded-lg">
                                <%--<asp:TableHeaderRow runat="server" ID="tblH_Produtos" CssClass="thead-light">--%>
                                <%--<asp:TableHeaderCell Scope="Column" CssClass="text-center">Produto</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Marca</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Categoria</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Quantidade</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço Compra</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço Venda</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>--%>
                                <%--</asp:TableHeaderRow>--%>
                            </asp:Table>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="text-success font-weight-bold" ID="lbl_TotalCompra"></asp:Label>
                        </div>

                        <div class="form-group">
                            <asp:Button runat="server" ID="btn_ConcluirCompra" CssClass="btn btn-success mt-2" Text="Concluir Compra" OnClick="btn_ConcluirCompra_Click" formnovalidate />
                            <asp:Button runat="server" ID="btn_Voltar" CssClass="btn btn-danger mt-2" Text="Voltar" OnClick="btn_Voltar_Click" formnovalidate />
                        </div>

                        <div class="form-group">
                            <asp:Panel runat="server" ID="pnl_Concluir" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label runat="server" ID="lbl_AlertConcluir" CssClass="text-danger form-text text-muted" Text=""></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                </main>
            </div>
        </div>
    </form>
</body>
</html>