<%@ Page Language="C#" Async="true" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="oficina_Pesquisar.aspx.cs" Inherits="FixFinder.Pages.oficina_Pesquisar" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pesquisa</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="https://kit.fontawesome.com/1729574db6.js"></script>
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
                        <button onclick="getLocation(); return false" class="btn btn-primary"><i class="fas fa-map-marker-alt fa-1x fa-fw mr-1"></i>Localização atual</button>
                    </span>
                    <asp:TextBox runat="server" ID="txt_Pesquisa" ClientID="txt_Pesquisa" CssClass="form-control width100" ClientIDMode="Static" placeholder="Informe a sua localização..." />
                    <span class="input-group-append">
                        <asp:LinkButton runat="server" ID="btn_Pesquisar" CssClass="btn btn-success" OnClick="btn_Pesquisar_Click"><i class="fas fa-search fa-1x fa-fw mr-1"></i>Pesquisar</asp:LinkButton>
                    </span>
                </div>
            </div>

            <div class="form-group">
                <%--SORT--%>
                <div class="input-group float-right" style="width: auto">
                    <span class="input-group-prepend">
                        <asp:Button runat="server" ID="btn_OrdenarDistancia" Text="Ordenar por distância" CssClass="btn btn-primary" OnClick="btn_OrdenarDistancia_Click" />
                    </span>
                    <span class="input-group-append">
                        <asp:Button runat="server" ID="btn_OrdenarNota" Text="Ordenar por nota" CssClass="btn btn-outline-primary" OnClick="btn_OrdenarNota_Click" />
                    </span>
                </div>

                <%--FILTRO--%>
                <div class="dropdown float-right mr-2">
                    <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <i class="fas fa-filter fa-1x fa-fw"></i>
                        Filtrar
                    </button>
                    <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                        <h6 class="dropdown-header">Distância</h6>
                        <asp:Button runat="server" ID="btn_Filtro1" CssClass="dropdown-item" CommandArgument="50" OnClick="btn_Filtro_Click" Text="Nenhum"></asp:Button>
                        <asp:Button runat="server" ID="btn_Filtro2" CssClass="dropdown-item" CommandArgument="5" OnClick="btn_Filtro_Click" Text="Menor que 5km"></asp:Button>
                        <asp:Button runat="server" ID="btn_Filtro3" CssClass="dropdown-item" CommandArgument="10" OnClick="btn_Filtro_Click" Text="Menor que 10km"></asp:Button>
                        <asp:Button runat="server" ID="btn_Filtro4" CssClass="dropdown-item" CommandArgument="20" OnClick="btn_Filtro_Click" Text="Menor que 20km"></asp:Button>
                    </div>
                </div>
            </div>

            <br />

            <div class="form-group mt-4">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger " role="alert">
                    <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                </asp:Panel>
            </div>
            <%--RESULTADO PESQUISA--%>
            <div runat="server" id="div_Resultados" class="container mt-4 text-left p-0">
            </div>
        </div>

        <%--HIDDEN BOYS--%>
        <asp:Button runat="server" ID="btn_CarregarEndereco" Style="display: none" OnClick="btn_CarregarEndereco_Click" ClientIDMode="Static" />
        <asp:TextBox runat="server" ID="txt_LatLon" ClientID="txtLatLon" Style="display: none" ClientIDMode="Static" />
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