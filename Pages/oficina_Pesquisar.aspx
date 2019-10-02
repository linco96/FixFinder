<%@ Page Language="C#" Async="true" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="oficina_Pesquisar.aspx.cs" Inherits="FixFinder.Pages.oficina_Pesquisar" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pesquisa</title>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <%--FORMULARIO PESQUISA--%>
        <div class="container mt-5 w-75 text-center">
            <asp:Image runat="server" ID="logo" CssClass="Responsive image" Style="max-width: 50%" ImageUrl="~/Content/logo_milk.png" />
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-prepend">
                        <button onclick="getLocation(); return false" class="btn btn-primary">Localização atual</button>
                    </span>
                    <asp:TextBox runat="server" ID="txt_Pesquisa" ClientID="txt_Pesquisa" CssClass="form-control width100" ClientIDMode="Static" placeholder="Informe a sua localização..." />
                    <span class="input-group-append">
                        <asp:Button runat="server" ID="btn_Pesquisar" Text="Pesquisar" CssClass="btn btn-success" OnClick="btn_Pesquisar_Click" />
                    </span>
                </div>
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                    <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                </asp:Panel>
            </div>
            <div class="form-group">
                <div class="input-group float-right" style="width: auto">
                    <span class="input-group-prepend">
                        <asp:Button runat="server" ID="btn_OrdenarDistancia" Text="Ordenar por distância" CssClass="btn btn-primary" OnClick="btn_OrdenarDistancia_Click" />
                    </span>
                    <span class="input-group-append">
                        <asp:Button runat="server" ID="btn_OrdenarNota" Text="Ordenar por nota" CssClass="btn btn-outline-primary" OnClick="btn_OrdenarNota_Click" />
                    </span>
                </div>
            </div>
            <br />
        </div>

        <%--HIDDEN BOYS--%>
        <asp:Button runat="server" ID="btn_CarregarEndereco" Style="display: none" OnClick="btn_CarregarEndereco_Click" ClientIDMode="Static" />
        <asp:TextBox runat="server" ID="txt_LatLon" ClientID="txtLatLon" Style="display: none" ClientIDMode="Static" />

        <%--RESULTADO PESQUISA--%>
        <div runat="server" id="div_Resultados" class="container mt-4">
        </div>
    </form>
    <script type="text/javascript">
        var x = document.getElementById("demo");
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            }
            else { x.innerHTML = "Geolocation is not supported by this browser."; }
        }

        function showPosition(position) {
            var latlon = position.coords.latitude + "," + position.coords.longitude;
            $("#txt_LatLon").val(latlon);
            $("#btn_CarregarEndereco").trigger('click');
        }

        function showError(error) {
            if (error.code == 1) {
                x.innerHTML = "User denied the request for Geolocation."
            }
            else if (err.code == 2) {
                x.innerHTML = "Location information is unavailable."
            }
            else if (err.code == 3) {
                x.innerHTML = "The request to get user location timed out."
            }
            else {
                x.innerHTML = "An unknown error occurred."
            }
        }
    </script>
</body>
</html>