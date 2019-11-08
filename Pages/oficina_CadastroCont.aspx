<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="oficina_CadastroCont.aspx.cs" Inherits="FixFinder.Pages.oficina_CadastroCont" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de oficina</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
            </asp:Panel>
        </div>
    </form>
</body>
</html>