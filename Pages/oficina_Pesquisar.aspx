<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="oficina_Pesquisar.aspx.cs" Inherits="FixFinder.Pages.oficina_Pesquisar" %>

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
        <div class="container mt-5 w-75">
            <div class="form-group">
                <div class="input-group">
                    <span class="input-group-prepend">
                        <button onclick="getLocation(); return false" class="btn btn-primary">Localização atual</button>
                    </span>
                    <asp:TextBox runat="server" ID="txt_Pesquisa" ClientID="txt_Pesquisa" CssClass="form-control width100" ClientIDMode="Static" />
                    <asp:Label runat="server" ID="biglbl"></asp:Label>
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
        </div>
        <div class="container mt-2">
            <div class="card mb-3" style="max-height: 400px;">
                <div class="row no-gutters">
                    <div class="col-md-3 p-1 border-right">
                        <img src="../Content/01-SCOPINO-PATIO-OFICINA_setembro2017.jpg" class="card-img" alt="..." />
                    </div>
                    <div class="col-md-9">
                        <div class="card-header bg-light p-1">
                            <h5 class="card-title mt-2 ml-3">
                                <span class="align-middle">Paje Motors</span>
                                <span class="float-right">
                                    <label runat="server" id="lbl_Reputacao" class="align-middle">7/10</label>
                                    <img class="align-top" src="../Content/star_24.png" />
                                </span>
                            </h5>
                        </div>
                        <div class="card-body">
                            <p class="card-text">Rua Visconde de Guarapuava, 1400 - Curitiba, PR</p>
                            <button class="btn btn-primary ">Solicitar agendamento</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Button runat="server" ID="btn_CarregarEndereco" Style="display: none" OnClick="btn_CarregarEndereco_Click" ClientIDMode="Static" />
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