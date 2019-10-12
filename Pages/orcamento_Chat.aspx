<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orcamento_Chat.aspx.cs" Inherits="FixFinder.Pages.orcamento_Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chat</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>

    <script type="text/javascript">

        window.onload = function () {
            var notFirstTime = <% if (postback) { Response.Write("true"); } else { Response.Write("false"); }  %>;

            if (!notFirstTime) {
                SetScrollBottom();
            } else {
                SetDivScrollPosition();
            }

            $("#txt_Mensagem").focus();
            if ($("#txt_Mensagem").val().length > 0) {
                var posStart = $("#txt_posStart").val();
                var posEnd = $("#txt_posEnd").val();
                setSelectionRange(document.getElementById("txt_Mensagem"), posStart, posEnd);
            }
        }

        $(document).ready(function () {
            window.setInterval(function () {
                StoreDivPosition();
                $("#txt_posStart").val($("#txt_Mensagem")[0].selectionStart);
                $("#txt_posEnd").val($("#txt_Mensagem")[0].selectionEnd);
                $("#btn_GambiButton").click();
            }, 5000);
        });

        //RIP função lixo

        function setSelectionRange(input, selectionStart, selectionEnd) {
            if (input.setSelectionRange) {
                input.focus();
                input.setSelectionRange(selectionStart, selectionEnd);
            }
            else if (input.createTextRange) {
                var range = input.createTextRange();
                range.collapse(true);
                range.moveEnd('character', selectionEnd);
                range.moveStart('character', selectionStart);
                range.select();
            }
        }

        function SetScrollBottom() {
            var objDiv = document.getElementById("pnl_Mensagens");
            objDiv.scrollTop = objDiv.scrollHeight;
            StoreDivPosition();
        }

        function StoreDivPosition() {
            var intY = document.getElementById("pnl_Mensagens").scrollTop;
            document.cookie = "yPos=!~" + intY + "~!";
        }

        function SetDivScrollPosition() {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("pnl_Mensagens").scrollTop = strPos;
            }
        }
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
                                    <a runat="server" id="a_OrcamentoCliente" class="nav-link" href="orcamento_ListaCliente.aspx">
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
                                    <a runat="server" id="a_OrcamentoOficina" class="nav-link" href="orcamento_ListaOficina.aspx">
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
                    <%--Conteudo--%>
                    <div class="container mt-5">
                        <div class="form-group" align="center">
                            <h2 style="text-align: center">
                                <span runat="server" id="lbl_Oficina" class="align-middle text-info">Paje Motors</span>
                            </h2>

                            <asp:Table runat="server" ID="tbl_Dados">
                                <asp:TableRow CssClass="border-bottom border-info">
                                    <asp:TableCell CssClass="h5 text-center w-50 ">
                            <asp:Label runat="server" CssClass="align-middle text-right">Status Orçamento</asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell CssClass="h5 text-center w-50 ">
                                        <asp:Label runat="server" ID="lbl_Status" CssClass="align-middle text-secondary">Aprovado</asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow CssClass="border-bottom border-info">
                                    <asp:TableCell CssClass="h5 text-center w-50">
                            <asp:Label runat="server" CssClass="align-middle">Veículo</asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell CssClass="h5 text-center w-50">
                                        <asp:Label runat="server" ID="lbl_Veiculo" CssClass="align-middle text-center text-secondary">Corolla - AYK-3929</asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow CssClass="border-bottom border-info">
                                    <asp:TableCell CssClass="h5 text-center w-50">
                            <asp:Label runat="server" CssClass="align-middle">Mecânico Responsável</asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell CssClass="h5 text-center w-50">
                                        <asp:Label runat="server" ID="lbl_MecanicoResponsavel" CssClass="align-middle text-center text-secondary">Um Fera ae</asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <hr class="border border-dark border-bottom-0 mt-1 mb-1 w-75" />

                        <div class="container mt-3" align="center">
                            <%--<asp:Panel runat="server" ID="pnl_Mensagens" CssClass="overflow-auto bg-light w-75 rounded border" Style="height: 350px">--%>
                            <div runat="server" id="pnl_Mensagens" onscroll="StoreDivPosition()" class="overflow-auto bg-light w-75 rounded border" style="height: 350px">

                                <%--Mecanico--%>
                                <%--<div class="bg-secondary ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-left text-light text-break">
                        <div class="font-italic">Enviado 08/03/2019 às 08:32 - Mecânico</div>
                        <br />
                        TESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEE
                    </div>--%>
                                <%--Eu--%>
                                <%--<div class="bg-info ml-2 mr-2 mt-1 mb-1 rounded text-left w-75 p-2 float-right text-light text-break">
                        <div class="font-italic">Enviado 08/03/2019 às 08:32 - Eu</div>
                        <br />
                        TESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEETESTEEEEEEEEEEE
                    </div>--%>
                            </div>
                            <%--</asp:Panel>--%>
                            <div>
                                <asp:TextBox runat="server" CssClass="text-left w-75 bg-light border rounded mt-1 p-1" ID="txt_Mensagem" MaxLength="500" placeholder="Tecle enter para enviar sua mensagem..." required autocomplete="off"></asp:TextBox>
                                <asp:Button runat="server" ID="btn_EnviarMSG" OnClick="btn_EnviarMSG_Click" Style="display: none" required ClientIDMode="Static" />
                                <asp:Button runat="server" ID="btn_GambiButton" Style="display: none" OnClick="btn_GambiButton_Click" formnovalidate />
                                <asp:TextBox runat="server" ID="txt_posStart" ClientIDMode="Static" Style="display: none"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txt_posEnd" ClientIDMode="Static" Style="display: none"></asp:TextBox>
                            </div>
                            <div>
                                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger mt-2 w-75 text-left" role="alert">
                                    <asp:Label ID="lbl_Alert" runat="server">Porra jovem</asp:Label>
                                </asp:Panel>
                            </div>
                            <div class="form-group">
                                <asp:Button runat="server" ID="btn_Voltar" CssClass="btn btn-danger mt-3" Text="Voltar" OnClick="btn_Voltar_Click" formnovalidate />
                            </div>
                        </div>
                    </div>
                    <%--Fim Conteudo--%>
                </main>
            </div>
        </div>
    </form>
</body>
</html>