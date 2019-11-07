<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="FixFinder.Pages.admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin</title>
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
                    case "usuariosUnicos":
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
                                dataPoints: <% if (select_Grafico.SelectedValue.Equals("usuariosUnicos")) { Response.Write(jsonGrafico); } else { Response.Write("[]"); } %>,
                                type: "column",
                                color: "#5F9EA0",
                                xValueType: "dateTime",
                                xValueFormatString: "MM/YYYY"
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
        <nav class="navbar navbar-expand-lg navbar-light bg-dark">
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item ml-2 mt-2">
                        <h5 class="text-white font-weight-normal">FixFinder</h5>
                    </li>
                    <li class="nav-item ml-2 mt-2">
                        <p class="text-white">|</p>
                    </li>
                    <li class="nav-item ml-2 mt-2">
                        <asp:Label runat="server" CssClass="font-italic text-white" ID="lbl_Nome"></asp:Label>
                    </li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li class="nav-item">
                        <a runat="server" id="btn_Sair" href="~/Pages/saindo.aspx" class="text-white nav-link">Sair</a>
                    </li>
                </ul>
            </div>
        </nav>
        <div class="container mt-5">
            <h2 class="font-weight-bold text-primary text-center">Status FixFinder</h2>
            <div class="container mt-2">
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
                            <asp:ListItem Text="Usuários Únicos" Value="usuariosUnicos"></asp:ListItem>
                            <asp:ListItem Text="Oficinas Ativas x Inativas" Value="oficinaAtivaInativa"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </div>
                <div class="form-group">
                    <span class="w-50 text-left">
                        <asp:Button runat="server" ID="btn_GerarGrafico" CssClass="btn btn-outline-info btn-sm mt-2" Text="Gerar Gráfico" OnClick="btn_GerarGrafico_Click" />
                    </span>
                    <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger mt-2" role="alert">
                        <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                    </asp:Panel>
                    <hr class="border border-muted border-bottom-0" />

                    <div runat="server" id="div_Chart" class="container mt-4" style="position: relative; height: 400px">
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>