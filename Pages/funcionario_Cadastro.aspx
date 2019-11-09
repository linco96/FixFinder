<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="funcionario_Cadastro.aspx.cs" Inherits="FixFinder.Pages.funcionario_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro de funcionários</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txt_CPF").blur(function () {
                $("#btn_Puxar").trigger('click');
            });
        });
    </script>

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
                                    <a class="nav-link" href="compra_Lista.aspx">
                                        <i class="fas fa-shopping-basket fa-1x fa-fw mr-1"></i>
                                        Compras
                                    </a>
                                </li>
                                <li runat="server" id="btn_Funcionarios" class="nav-item">
                                    <a class="nav-link active" href="funcionario_Lista.aspx">
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
                        <h1 class="h2">Cadastro de funcionário</h1>
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

                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_CPF" style="display: block">CPF do Funcionário</label>
                                <asp:TextBox runat="server" ID="txt_CPF" CssClass="form-control w-100" minlength="14" onkeypress="$(this).mask('000.000.000-00');" required="required" ClientIDMode="Static"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left mb-1">
                                <label for="txt_Nome" style="display: block">Nome completo</label>
                                <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control w-100" minlength="4" ReadOnly="true" required="required"></asp:TextBox>
                            </span>
                        </div>

                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_Telefone" style="display: block">Telefone</label>
                                <asp:TextBox runat="server" ID="txt_Telefone" CssClass="form-control w-100" minlength="8" onkeypress="$(this).mask('(00) 0 0000-0000');" ReadOnly="true" required="required"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left mb-1">
                                <label for="txt_Email" style="display: block">E-mail</label>
                                <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control w-100" type="email" ReadOnly="true" required="required"></asp:TextBox>
                            </span>
                        </div>

                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_Cargo" style="display: block">Cargo</label>
                                <asp:TextBox runat="server" ID="txt_Cargo" CssClass="form-control w-100" minlength="2" required="required"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left mb-1">
                                <label for="txt_Salario" style="display: block">Salário</label>
                                <asp:TextBox runat="server" ID="txt_Salario" CssClass="form-control w-100" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
                            </span>
                        </div>

                        <div class="form-group">
                            <label for="ddl_Banco">Banco</label>
                            <asp:DropDownList runat="server" ID="ddl_Banco" CssClass="custom-select custom-select-sm">
                                <asp:ListItem Text="1 - Banco do Brasil S.A." Value="1"></asp:ListItem>
                                <asp:ListItem Text="3 - Banco da Amazônia S.A." Value="3"></asp:ListItem>
                                <asp:ListItem Text="4 - Banco do Nordeste do Brasil S.A." Value="4"></asp:ListItem>
                                <asp:ListItem Text="12 - Banco Inbursa S.A." Value="12"></asp:ListItem>
                                <asp:ListItem Text="17 - BNY Mellon Banco S.A." Value="17"></asp:ListItem>
                                <asp:ListItem Text="21 - BANESTES S.A. Banco do Estado do Espírito Santo" Value="21"></asp:ListItem>
                                <asp:ListItem Text="24 - Banco BANDEPE S.A." Value="24"></asp:ListItem>
                                <asp:ListItem Text="25 - Banco Alfa S.A." Value="25"></asp:ListItem>
                                <asp:ListItem Text="29 - Banco Itaú Consignado S.A." Value="29"></asp:ListItem>
                                <asp:ListItem Text="33 - Banco Santander (Brasil) S.A." Value="33"></asp:ListItem>
                                <asp:ListItem Text="36 - Banco Bradesco BBI S.A." Value="36"></asp:ListItem>
                                <asp:ListItem Text="37 - Banco do Estado do Pará S.A." Value="37"></asp:ListItem>
                                <asp:ListItem Text="40 - Banco Cargill S.A." Value="40"></asp:ListItem>
                                <asp:ListItem Text="41 - Banco do Estado do Rio Grande do Sul S.A." Value="41"></asp:ListItem>
                                <asp:ListItem Text="47 - Banco do Estado de Sergipe S.A." Value="47"></asp:ListItem>
                                <asp:ListItem Text="62 - Hipercard Banco Múltiplo S.A." Value="62"></asp:ListItem>
                                <asp:ListItem Text="63 - Banco Bradescard S.A." Value="63"></asp:ListItem>
                                <asp:ListItem Text="64 - Goldman Sachs do Brasil Banco Múltiplo S.A." Value="64"></asp:ListItem>
                                <asp:ListItem Text="65 - Banco Andbank (Brasil) S.A." Value="65"></asp:ListItem>
                                <asp:ListItem Text="69 - Banco Crefisa S.A." Value="69"></asp:ListItem>
                                <asp:ListItem Text="70 - BRB - Banco de Brasília S.A." Value="70"></asp:ListItem>
                                <asp:ListItem Text="74 - Banco J. Safra S.A." Value="74"></asp:ListItem>
                                <asp:ListItem Text="75 - Banco ABN AMRO S.A." Value="75"></asp:ListItem>
                                <asp:ListItem Text="77 - Banco Inter S.A." Value="77"></asp:ListItem>
                                <asp:ListItem Text="81 - BancoSeguro S.A." Value="81"></asp:ListItem>
                                <asp:ListItem Text="82 - Banco Topázio S.A." Value="82"></asp:ListItem>
                                <asp:ListItem Text="83 - Banco da China Brasil S.A." Value="83"></asp:ListItem>
                                <asp:ListItem Text="95 - Travelex Banco de Câmbio S.A." Value="95"></asp:ListItem>
                                <asp:ListItem Text="96 - Banco B3 S.A." Value="96"></asp:ListItem>
                                <asp:ListItem Text="104 - Caixa Econômica Federal" Value="104"></asp:ListItem>
                                <asp:ListItem Text="107 - Banco BOCOM BBM S.A." Value="107"></asp:ListItem>
                                <asp:ListItem Text="118 - Standard Chartered Bank (Brasil) S/A–Bco Invest." Value="118"></asp:ListItem>
                                <asp:ListItem Text="119 - Banco Western Union do Brasil S.A." Value="119"></asp:ListItem>
                                <asp:ListItem Text="120 - Banco Rodobens S.A." Value="120"></asp:ListItem>
                                <asp:ListItem Text="121 - Banco Agibank S.A." Value="121"></asp:ListItem>
                                <asp:ListItem Text="125 - Brasil Plural S.A. - Banco Múltiplo" Value="125"></asp:ListItem>
                                <asp:ListItem Text="128 - MS Bank S.A. Banco de Câmbio" Value="128"></asp:ListItem>
                                <asp:ListItem Text="129 - UBS Brasil Banco de Investimento S.A." Value="129"></asp:ListItem>
                                <asp:ListItem Text="144 - BEXS Banco de Câmbio S.A." Value="144"></asp:ListItem>
                                <asp:ListItem Text="169 - Banco Olé Bonsucesso Consignado S.A." Value="169"></asp:ListItem>
                                <asp:ListItem Text="184 - Banco Itaú BBA S.A." Value="184"></asp:ListItem>
                                <asp:ListItem Text="204 - Banco Bradesco Cartões S.A." Value="204"></asp:ListItem>
                                <asp:ListItem Text="208 - Banco BTG Pactual S.A." Value="208"></asp:ListItem>
                                <asp:ListItem Text="212 - Banco Original S.A." Value="212"></asp:ListItem>
                                <asp:ListItem Text="217 - Banco John Deere S.A." Value="217"></asp:ListItem>
                                <asp:ListItem Text="218 - Banco BS2 S.A." Value="218"></asp:ListItem>
                                <asp:ListItem Text="222 - Banco Credit Agricole Brasil S.A." Value="222"></asp:ListItem>
                                <asp:ListItem Text="224 - Banco Fibra S.A." Value="224"></asp:ListItem>
                                <asp:ListItem Text="233 - Banco Cifra S.A." Value="233"></asp:ListItem>
                                <asp:ListItem Text="237 - Banco Bradesco S.A." Value="237"></asp:ListItem>
                                <asp:ListItem Text="246 - Banco ABC Brasil S.A." Value="246"></asp:ListItem>
                                <asp:ListItem Text="249 - Banco Investcred Unibanco S.A." Value="249"></asp:ListItem>
                                <asp:ListItem Text="250 - BCV - Banco de Crédito e Varejo S.A." Value="250"></asp:ListItem>
                                <asp:ListItem Text="254 - Paraná Banco S.A." Value="254"></asp:ListItem>
                                <asp:ListItem Text="265 - Banco Fator S.A." Value="265"></asp:ListItem>
                                <asp:ListItem Text="269 - HSBC Brasil S.A. - Banco de Investimento" Value="269"></asp:ListItem>
                                <asp:ListItem Text="318 - Banco BMG S.A." Value="318"></asp:ListItem>
                                <asp:ListItem Text="320 - China Construction Bank (Brasil) Banco Múltiplo S.A." Value="320"></asp:ListItem>
                                <asp:ListItem Text="341 - Itaú Unibanco S.A." Value="341"></asp:ListItem>
                                <asp:ListItem Text="366 - Banco Société Générale Brasil S.A." Value="366"></asp:ListItem>
                                <asp:ListItem Text="370 - Banco Mizuho do Brasil S.A." Value="370"></asp:ListItem>
                                <asp:ListItem Text="376 - Banco J. P. Morgan S.A." Value="376"></asp:ListItem>
                                <asp:ListItem Text="389 - Banco Mercantil do Brasil S.A." Value="389"></asp:ListItem>
                                <asp:ListItem Text="394 - Banco Bradesco Financiamentos S.A." Value="394"></asp:ListItem>
                                <asp:ListItem Text="399 - Kirton Bank S.A. - Banco Múltiplo" Value="399"></asp:ListItem>
                                <asp:ListItem Text="422 - Banco Safra S.A." Value="422"></asp:ListItem>
                                <asp:ListItem Text="456 - Banco MUFG Brasil S.A." Value="456"></asp:ListItem>
                                <asp:ListItem Text="464 - Banco Sumitomo Mitsui Brasileiro S.A." Value="464"></asp:ListItem>
                                <asp:ListItem Text="473 - Banco Caixa Geral - Brasil S.A." Value="473"></asp:ListItem>
                                <asp:ListItem Text="477 - Citibank N.A." Value="477"></asp:ListItem>
                                <asp:ListItem Text="479 - Banco ItauBank S.A" Value="479"></asp:ListItem>
                                <asp:ListItem Text="487 - Deutsche Bank S.A. - Banco Alemão" Value="487"></asp:ListItem>
                                <asp:ListItem Text="488 - JPMorgan Chase Bank, National Association" Value="488"></asp:ListItem>
                                <asp:ListItem Text="492 - ING Bank N.V." Value="492"></asp:ListItem>
                                <asp:ListItem Text="505 - Banco Credit Suisse (Brasil) S.A." Value="505"></asp:ListItem>
                                <asp:ListItem Text="600 - Banco Luso Brasileiro S.A." Value="600"></asp:ListItem>
                                <asp:ListItem Text="604 - Banco Industrial do Brasil S.A." Value="604"></asp:ListItem>
                                <asp:ListItem Text="610 - Banco VR S.A." Value="610"></asp:ListItem>
                                <asp:ListItem Text="611 - Banco Paulista S.A." Value="611"></asp:ListItem>
                                <asp:ListItem Text="612 - Banco Guanabara S.A." Value="612"></asp:ListItem>
                                <asp:ListItem Text="623 - Banco PAN S.A." Value="623"></asp:ListItem>
                                <asp:ListItem Text="633 - Banco Rendimento S.A." Value="633"></asp:ListItem>
                                <asp:ListItem Text="634 - Banco Triângulo S.A." Value="634"></asp:ListItem>
                                <asp:ListItem Text="641 - Banco Alvorada S.A." Value="641"></asp:ListItem>
                                <asp:ListItem Text="643 - Banco Pine S.A." Value="643"></asp:ListItem>
                                <asp:ListItem Text="652 - Itaú Unibanco Holding S.A." Value="652"></asp:ListItem>
                                <asp:ListItem Text="653 - Banco Indusval S.A." Value="653"></asp:ListItem>
                                <asp:ListItem Text="655 - Banco Votorantim S.A." Value="655"></asp:ListItem>
                                <asp:ListItem Text="707 - Banco Daycoval S.A." Value="707"></asp:ListItem>
                                <asp:ListItem Text="739 - Banco Cetelem S.A." Value="739"></asp:ListItem>
                                <asp:ListItem Text="743 - Banco Semear S.A." Value="743"></asp:ListItem>
                                <asp:ListItem Text="745 - Banco Citibank S.A." Value="745"></asp:ListItem>
                                <asp:ListItem Text="746 - Banco Modal S.A." Value="746"></asp:ListItem>
                                <asp:ListItem Text="747 - Banco Rabobank International Brasil S.A." Value="747"></asp:ListItem>
                                <asp:ListItem Text="748 - Banco Cooperativo Sicredi S.A." Value="748"></asp:ListItem>
                                <asp:ListItem Text="751 - Scotiabank Brasil S.A. Banco Múltiplo" Value="751"></asp:ListItem>
                                <asp:ListItem Text="752 - Banco BNP Paribas Brasil S.A." Value="752"></asp:ListItem>
                                <asp:ListItem Text="755 - Bank of America Merrill Lynch Banco Múltiplo S.A." Value="755"></asp:ListItem>
                                <asp:ListItem Text="756 - Banco Cooperativo do Brasil S.A. - BANCOOB" Value="756"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-inline">
                            <span class="w-50 text-left pr-3 mb-1">
                                <label for="txt_Agencia" style="display: block">Agência</label>
                                <asp:TextBox runat="server" ID="txt_Agencia" CssClass="form-control w-100" onkeypress="$(this).mask('0000000000');" required="required"></asp:TextBox>
                            </span>
                            <span class="w-50 text-left mb-1">
                                <label for="txt_Conta" style="display: block">Conta</label>
                                <asp:TextBox runat="server" ID="txt_Conta" CssClass="form-control w-100" onkeypress="$(this).mask('0000000000');" required="required"></asp:TextBox>
                            </span>
                        </div>

                        <div class="form-group mt-1">
                            <asp:Button runat="server" ID="btn_Registro" CssClass="btn btn-success" OnClick="btn_Registro_Click" Text="Registrar" />
                        </div>
                        <div class="form-group">
                            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                    <asp:Button runat="server" ID="btn_Puxar" OnClick="btn_Puxar_Click" formnovalidate="true" ClientIDMode="Static" Style="display: none" />
                </main>
            </div>
        </div>
    </form>
</body>
</html>