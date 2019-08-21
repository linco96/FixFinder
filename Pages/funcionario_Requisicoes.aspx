<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="funcionario_Requisicoes.aspx.cs" Inherits="FixFinder.Pages.funcionario_Requisicoes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Requisições</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_ListaVeiculo" runat="server">
        <div class="container-fluid mt-5 pl-5 row" runat="server" id="div_Cards">
        </div>
        <div class="container container-fluid mt-5 pl-5">
            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
            </asp:Panel>
        </div>
    </form>
</body>
</html>