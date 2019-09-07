<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="compra_Lista.aspx.cs" Inherits="FixFinder.Pages.compra_Lista" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compras</title>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form_Compras" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Suas Compras</h1>
            <hr class="border-primary" />
            <asp:Button runat="server" ID="btn_Cadastrar" Text="Nova Compra" CssClass="btn btn-outline-primary" OnClick="btn_Cadastrar_Click" />
            <asp:Table runat="server" ID="tbl_Compras" CssClass="table border rounded-lg mt-3">
            </asp:Table>
        </div>
    </form>
</body>
</html>