<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="relatorio.aspx.cs" Inherits="FixFinder.Pages.relatorio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relatórios</title>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            if (<%= gerarGrafico.ToString().ToLower() %>) {
                switch ($("#select_Grafico").val()) {
                    case "despesaReceita":
                        var chart = new CanvasJS.Chart("div_Chart", {
                            theme: "light2",
                            exportEnabled: true,
                            animationEnabled: true,
                            title: {
                                text: "Despesa x Receita"
                            },
                            subtitles: [{
                                text: ""
                            }],
                            axisX: {
                                valueFormatString: "MM/YYYY",
                                intervalType: "month",
                                interval: 1
                            },
                            axisY: {
                                includeZero: false,
                                prefix: "R$"
                            },
                            toolTip: {
                                shared: true
                            },
                            data: [{
                                type: "area",
                                name: "Receita",
                                markerSize: 0,
                                xValueType: "dateTime",
                                xValueFormatString: "MM/YYYY",
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("despesaReceita")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                yValueFormatString: "R$ #,##0.##"

                            },
                                {
                                    type: "area",
                                    name: "Despesa",
                                    markerSize: 0,
                                    xValueType: "dateTime",
                                    xValueFormatString: "MM/YYYY",
                                    dataPoints: <% if (select_Grafico.SelectedValue.Equals("despesaReceita")) { Response.Write(jsonGrafico2); } else { Response.Write("[]"); } %>,
                                    yValueFormatString: "R$ #,##0.##"
                                }
                            ]
                        });
                        chart.render();
                        break;

                    case "lucroBruto":
                        var chart = new CanvasJS.Chart("div_Chart", {
                            animationEnabled: true,
                            exportEnabled: true,
                            animationEnabled: true,
                            theme: "light2", // "light1", "light2", "dark1", "dark2"
                            title: {
                                text: "Lucro Bruto"
                            },
                            axisX: {
                                valueFormatString: "MM/YYYY",
                                intervalType: "month",
                                interval: 1
                            },
                            axisY: {
                                includeZero: false,
                                prefix: "R$"
                            },
                            data: [{
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("lucroBruto")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                type: "column",
                                color: "#5F9EA0",
                                yValueFormatString: "R$ #,##0.##",
                                xValueType: "dateTime",
                                xValueFormatString: "MM/YYYY"
                            }]
                        });
                        chart.render();
                        break;
                    case "totalClientes":
                        var chart = new CanvasJS.Chart("div_Chart", {
                            exportEnabled: true,
                            animationEnabled: true,
                            theme: "light2", // "light1", "light2", "dark1", "dark2"
                            title: {
                                text: "Total Clientes"
                            },
                            axisX: {
                                valueFormatString: "MM/YYYY",
                                intervalType: "month",
                                interval: 1
                            },
                            axisY: {
                                includeZero: false,
                                interval: 1
                            },
                            data: [{
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("totalClientes")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                type: "column",
                                color: "#5F9EA0",
                                xValueType: "dateTime",
                                xValueFormatString: "MM/YYYY"
                            }]
                        });
                        chart.render();
                        break;

                    case "historicoOrcamentoCriacao":
                        var chart = new CanvasJS.Chart("div_Chart", {
                            exportEnabled: true,
                            animationEnabled: true,
                            title: {
                                text: "Histórico Orçamentos"
                            },
                            toolTip: {
                                shared: true
                            },
                            axisX: {
                                valueFormatString: "MM/YYYY",
                                intervalType: "month",
                                interval: 1
                            },
                            axisY: {
                                includeZero: false,
                                interval: 1
                            },
                            legend: {
                                cursor: "pointer",
                                verticalAlign: "center",
                                horizontalAlign: "right",
                                itemclick: toggleDataSeries
                            },
                            data: [{
                                type: "stackedColumn",
                                name: "Concluído",
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("historicoOrcamentoCriacao")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                showInLegend: "true",
                                xValueType: "dateTime",
                                xValueFormatString: "MM/YYYY"
                            },
                                {
                                    type: "stackedColumn",
                                    name: "Cancelado",
                                    dataPoints: <% if (select_Grafico.SelectedValue.Equals("historicoOrcamentoCriacao")) { Response.Write(jsonGrafico2); } else { Response.Write("[]"); } %>,
                                    showInLegend: "true",
                                    xValueType: "dateTime",
                                    xValueFormatString: "MM/YYYY"
                                },
                                {
                                    type: "stackedColumn",
                                    name: "Aprovação Pendente",
                                    dataPoints: <% if (select_Grafico.SelectedValue.Equals("historicoOrcamentoCriacao")) { Response.Write(jsonGrafico3); } else { Response.Write("[]"); } %>,
                                    showInLegend: "true",
                                    xValueType: "dateTime",
                                    xValueFormatString: "MM/YYYY"
                                },
                                {
                                    type: "stackedColumn",
                                    name: "Aprovado",
                                    dataPoints: <% if (select_Grafico.SelectedValue.Equals("historicoOrcamentoCriacao")) { Response.Write(jsonGrafico4); } else { Response.Write("[]"); } %>,
                                    showInLegend: "true",
                                    xValueType: "dateTime",
                                    xValueFormatString: "MM/YYYY"
                                }]
                        });
                        chart.render();
                        function toggleDataSeries(e) {
                            if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                                e.dataSeries.visible = false;
                            }
                            else {
                                e.dataSeries.visible = true;
                            }
                            chart.render();
                        }
                        break;
                    case "despesaFornecedor":
                        var chart = new CanvasJS.Chart("div_Chart", {
                            theme: "light2", // "light1", "light2", "dark1", "dark2"
                            exportEnabled: true,
                            animationEnabled: true,
                            title: {
                                text: "Despesas Fornecedor"
                            },
                            subtitles: [{

                            }],
                            data: [{
                                type: "pie",
                                startAngle: 180,
                                toolTipContent: "<b>{label}</b>: {value}",
                                showInLegend: "true",
                                legendText: "{label}",
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("despesaFornecedor")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                indexLabel: "{label} - {value}"
                            }]
                        });
                        chart.render();
                        break;

                    case "novosClientes":
                        chart = new CanvasJS.Chart("div_Chart", {
                            theme: "light2", // "light1", "light2", "dark1", "dark2"
                            exportEnabled: true,
                            animationEnabled: true,
                            title: {
                                text: "Novos Clientes"
                            },
                            subtitles: [{

                            }],
                            data: [{
                                type: "pie",
                                startAngle: 180,
                                toolTipContent: "<b>{label}</b>: {y}",
                                showInLegend: "true",
                                legendText: "{label}",
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("novosClientes")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                indexLabel: "{label} - {y}"
                            }]
                        });
                        chart.render();
                        break;
                }
            }
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
                                    <a class="nav-link" href="funcionario_Lista.aspx">
                                        <i class="fas fa-user-friends fa-1x fa-fw mr-1"></i>
                                        Funcionários
                                    </a>
                                </li>
                                <li runat="server" id="btn_Relatorios" class="nav-item">
                                    <a class="nav-link active" href="relatorio.aspx">
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
                    <%--Conteudo--%>
                    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
                        <h1 class="h2">Relatórios</h1>
                    </div>

                    <div class="form-group">
                        <div class="form-inline w-100">
                            <span class="w-25 text-left pr-3">
                                <label style="display: block">Data Início</label>
                                <asp:TextBox runat="server" ID="txt_DataInicio" CssClass="form-control w-100 mr-2 mt-2" type="date" required="required"></asp:TextBox>
                            </span>
                            <span class="w-25 text-left pl-3">
                                <label style="display: block">Data Fim</label>
                                <asp:TextBox runat="server" ID="txt_DataFim" CssClass="form-control w-100 mt-2" type="date" required="required"></asp:TextBox>
                            </span>
                        </div>
                    </div>
                    <div class="form-group">

                        <span class="w-100 text-left">
                            <asp:DropDownList runat="server" ID="select_Grafico" CssClass="form-control w-50">
                                <asp:ListItem Text="Despesas x Receita" Value="despesaReceita"></asp:ListItem>
                                <asp:ListItem Text="Lucro Bruto" Value="lucroBruto"></asp:ListItem>
                                <asp:ListItem Text="Total Clientes" Value="totalClientes"></asp:ListItem>
                                <asp:ListItem Text="Histórico Orçamentos por Criação" Value="historicoOrcamentoCriacao"></asp:ListItem>
                                <asp:ListItem Text="Despesas por Fornecedor" Value="despesaFornecedor"></asp:ListItem>
                                <asp:ListItem Text="Novos Clientes" Value="novosClientes"></asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="form-group">
                        <span class="w-50 text-left">
                            <asp:Button runat="server" ID="btn_GerarGrafico" CssClass="btn btn-outline-info btn-sm mt-2" Text="Gerar Gráfico" OnClick="btn_GerarGrafico_Click" />
                            <%--<button id="btn_GerarGrafico" class="btn btn-outline-info btn-sm mt-2" onclick="odeioJQuery()">Gerar Gráfico</button>--%>
                        </span>
                        <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger mt-2" role="alert">
                            <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                        </asp:Panel>
                        <hr class="border border-muted border-bottom-0" />

                        <div runat="server" id="div_Chart" class="container mt-4" style="position: relative; height: 400px">
                        </div>
                    </div>
                </main>
            </div>
        </div>
    </form>
</body>
</html>