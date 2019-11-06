<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_Login.aspx.cs" Inherits="FixFinder.Pages.admin_Login" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Admin</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <div class="container w-25 p-3">
            <h1 style="text-align: center" class="display-4 text-primary">Login</h1>
            <div class="form-group">
                <asp:Label runat="server">Usuário</asp:Label>
                <asp:TextBox runat="server" ID="txt_NomeUsuario" class="form-control" aria-describedby="emailHelp" placeholder="Digite seu usuário" required></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server">Senha</asp:Label>
                <asp:TextBox runat="server" ID="txt_Senha" type="password" class="form-control" placeholder="Digite sua senha" required></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Button runat="server" ID="btn_Login" Text="Efetuar Login" OnClick="btn_Login_Click" class="btn btn-primary" />
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                    <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>