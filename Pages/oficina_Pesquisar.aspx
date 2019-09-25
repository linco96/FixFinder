﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oficina_Pesquisar.aspx.cs" Inherits="FixFinder.Pages.oficina_Pesquisar" %>

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
        <div class="container mt-5">
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-prepend">
                        <button onclick="getLocation(); return false" class="btn btn-primary">Localização atual</button>
                    </span>
                    <asp:TextBox runat="server" ID="txt_Pesquisa" ClientID="txt_Pesquisa" CssClass="form-control width100" ClientIDMode="Static" />
                    <asp:Label runat="server" ID="biglbl"></asp:Label>
                    <span class="input-group-append">
                        <asp:Button runat="server" ID="btn_Pesquisar" Text="Pesquisar" CssClass="btn btn-primary" OnClick="btn_Pesquisar_Click" />
                    </span>
                </div>
            </div>
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
            $("#txt_Pesquisa").val(latlon);
            alert(latlon);
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