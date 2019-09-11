<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="orcamento_Cadastro.aspx.cs" Inherits="FixFinder.Pages.orcamento_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Criar orçamento</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txt_CPF").blur(function () {
                $("#btn_CarregarCliente").trigger('click');
            });
            $("#txt_Desconto").blur(function () {
                $("#btn_AtualizarTotal").trigger('click');
            });
        });
    </script>
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
                                    <a class="nav-link active" href="orcamento_ListaOficina.aspx">
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
                        <h1 class="h2">Novo orçamento</h1>
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
                            <h4>Cliente</h4>
                        </div>
                        <div class="form-group">
                            <label for="txt_CPF">CPF</label>
                            <asp:TextBox runat="server" ID="txt_CPF" CssClass="form-control" minlength="14" onkeypress="$(this).mask('000.000.000-00');" ClientIDMode="Static"></asp:TextBox>
                            <small runat="server" id="alert_CPF" visible="false" class="form-text text-danger"></small>
                        </div>
                        <div class="form-group">
                            <label for="txt_Nome">Nome completo</label>
                            <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control" minlength="4" required="required" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txt_Veiculo">Veículo</label>
                            <asp:DropDownList runat="server" ID="txt_Veiculo" Enabled="false" CssClass="form-control custom-select">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group align-content-cen
                ter mt-5">
                            <h4>Serviços</h4>
                            <div>
                                <asp:Button runat="server" ID="btn_NovoServico" Text="Novo serviço" CssClass="btn btn-outline-primary btn-sm mt-1" aria-pressed="true" OnClick="btn_NovoServico_Click" formnovalidate="true" />
                            </div>
                        </div>
                        <div runat="server" id="form_CadastroServico" visible="false">
                            <div class="form-group">
                                <label for="txt_Descricao">Descrição</label>
                                <asp:TextBox runat="server" ID="txt_DescricaoServico" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txt_Valor">Valor</label>
                                <asp:TextBox runat="server" ID="txt_ValorServico" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btn_CadastrarServico" CssClass="btn btn-primary" OnClick="btn_CadastrarServico_Click" Text="Cadastrar" />
                                <asp:Button runat="server" ID="btn_CancelarCadastroServico" Text="Cancelar" CssClass="btn btn-danger" OnClick="btn_CancelarCadastroServico_Click" formnovalidate="true" />
                            </div>
                            <hr class="border-primary" />
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:DropDownList runat="server" ID="txt_ServicoSelecionado" CssClass="form-control custom-select width100">
                                </asp:DropDownList>
                                <span class="input-group-append">
                                    <asp:Button runat="server" ID="btn_AdicionarServico" Text="Adicionar" CssClass="btn btn-primary" OnClick="btn_AdicionarServico_Click" />
                                </span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="table-responsive-xl">
                                <asp:Table runat="server" ID="tbl_Servicos" CssClass="table border rounded-lg">
                                    <%--<asp:TableHeaderRow runat="server" ID="tblH_Servicos" CssClass="thead-light">
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Descrição</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Valor</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                        </asp:TableHeaderRow>--%>
                                </asp:Table>
                            </div>
                        </div>

                        <div class="form-group align-content-center mt-5">
                            <h4>Produtos</h4>
                            <div>
                                <asp:Button runat="server" ID="btn_NovoProduto" Text="Novo produto" CssClass="btn btn-outline-primary btn-sm mt-1" aria-pressed="true" OnClick="btn_NovoProduto_Click" formnovalidate="true" />
                            </div>
                        </div>
                        <div runat="server" id="form_CadastroProduto" visible="false">
                            <div class="form-inline">
                                <span class="w-50 text-left pr-3 mb-1">
                                    <label for="txt_DescricaoProduto" style="display: block">Descrição</label>
                                    <asp:TextBox runat="server" ID="txt_DescricaoProduto" CssClass="form-control w-100" required="required"></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3 mb-1">
                                    <label for="txt_ProdutoQuantidadeAtual" style="display: block">Quantidade atual</label>
                                    <asp:TextBox runat="server" ID="txt_QuantidadeAtualProduto" CssClass="form-control w-100" required="required"></asp:TextBox>
                                </span>
                            </div>
                            <div class="form-inline">
                                <span class="w-50 text-left pr-3 mb-1">
                                    <label for="txt_ProdutoMarca" style="display: block">Marca</label>
                                    <asp:TextBox runat="server" ID="txt_MarcaProduto" CssClass="form-control w-100" required="required"></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3 mb-1">
                                    <label for="txt_ProdutoCategoria" style="display: block">Categoria</label>
                                    <asp:TextBox runat="server" ID="txt_CategoriaProduto" CssClass="form-control w-100" required="required"></asp:TextBox>
                                </span>
                            </div>

                            <div class="form-inline">
                                <span class="w-50 text-left pr-3">
                                    <label for="txt_ProdutoPrecoCompra" style="display: block">Preço Compra</label>
                                    <asp:TextBox runat="server" ID="txt_PrecoCompraProduto" CssClass="form-control w-100" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3">
                                    <label for="txt_ProdutoPrecoVenda" style="display: block">Preço Venda</label>
                                    <asp:TextBox runat="server" ID="txt_PrecoVendaProduto" CssClass="form-control w-100" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                                </span>
                            </div>
                            <div class="form-inline">
                                <span class="w-50 text-left pr-3">
                                    <label for="txt_ProdutoValidade" style="display: block">Validade</label>
                                    <asp:TextBox runat="server" ID="txt_ValidadeProduto" CssClass="form-control w-100" type="date"></asp:TextBox>
                                </span>
                                <span class="w-50 text-left pl-3">
                                    <label for="txt_ProdutoQuantidade" style="display: block">Quantidade a ser utilizada</label>
                                    <asp:TextBox runat="server" ID="txt_QuantidadeUtilizadaProduto" CssClass="form-control w-100" minlength="1" onkeypress="$(this).mask('#.##0', {reverse: true});" required="required"></asp:TextBox>
                                </span>
                            </div>
                            <div class="form-group mt-3">
                                <asp:Button runat="server" ID="btn_CadastrarProduto" CssClass="btn btn-primary" OnClick="btn_CadastrarProduto_Click" Text="Cadastrar" />
                                <asp:Button runat="server" ID="btn_CancelarCadastroProduto" Text="Cancelar" CssClass="btn btn-danger" OnClick="btn_CancelarCadastroProduto_Click" formnovalidate="true" />
                            </div>
                            <hr class="border-primary" />
                        </div>
                        <div class="form-group">
                            <div class="input-group">
                                <asp:DropDownList runat="server" ID="txt_ProdutoSelecionado" CssClass="form-control custom-select">
                                </asp:DropDownList>

                                <span class="input-group-append">
                                    <asp:TextBox runat="server" ID="txt_ProdutoQuantidade" type="number" CssClass="form-control" placeholder="Qtde" Style="border-radius: 0px"></asp:TextBox>
                                    <asp:Button runat="server" ID="btn_AdicionarProduto" Text="Adicionar" CssClass="btn btn-primary" OnClick="btn_AdicionarProduto_Click" />
                                </span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="table-responsive-xl">
                                <asp:Table runat="server" ID="tbl_Produtos" CssClass="table border rounded-lg">
                                    <%--<asp:TableHeaderRow runat="server" ID="tblH_Produtos" CssClass="thead-light">
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Descrição</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Marca</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Validade</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Quantidade em<br />estoque</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Quantidade<br />selecionada</asp:TableHeaderCell>
                            <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                        </asp:TableHeaderRow>--%>
                                </asp:Table>
                            </div>
                        </div>

                        <div class="form-group mt-5">
                            <label for="txtDesconto">Desconto/Acréscimo</label>
                            <div class="input-group">
                                <span class="input-group-prepend">
                                    <asp:DropDownList runat="server" ID="txt_DescontoAcrescimo" CssClass="form-control custom-select bg-light" Style="border-top-right-radius: 0; border-bottom-right-radius: 0;" OnSelectedIndexChanged="txt_DescontoAcrescimo_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="d">Desconto</asp:ListItem>
                                        <asp:ListItem Value="a">Acréscimo</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <asp:TextBox runat="server" ID="txt_Desconto" CssClass="form-control" ClientIDMode="Static" onkeypress="$(this).mask('#.##0,00', { reverse: true });"></asp:TextBox>
                            </div>
                            <h3 runat="server" id="lbl_ValorTotal" class="mt-2 text-center text-primary">Valor total: R$ 0,00</h3>
                        </div>

                        <div class="form-group">
                            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>

                        <div class="form-group mt-5 text-center">

                            <asp:Button runat="server" ID="btn_Cadastro" CssClass="btn btn-success btn-lg" OnClick="btn_Cadastro_Click" Text="Criar orçamento" />
                        </div>
                    </div>
                    <asp:Button runat="server" ID="btn_CarregarCliente" Style="display: none" OnClick="btn_CarregarCliente_Click" ClientIDMode="Static" formnovalidate="formnovalidate" />
                    <asp:Button runat="server" ID="btn_AtualizarTotal" Style="display: none" OnClick="btn_AtualizarTotal_Click" ClientIDMode="Static" formnovalidate="formnovalidate" />
                </main>
            </div>
        </div>
    </form>
</body>
</html>