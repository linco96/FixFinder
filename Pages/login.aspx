<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="FixFinder.Pages.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.0.0.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
</head>
<body>
    <form id="form_Login" runat="server">
        <div class="container w-25 p-3">
            <div class="form-group">
                <asp:Label runat="server">Nome de usuário</asp:Label>
                <asp:TextBox runat="server" ID="txt_NomeUsuario" class="form-control" aria-describedby="emailHelp" placeholder="Digite seu nome de usuário"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label runat="server">Senha</asp:Label>
                <asp:TextBox runat="server" ID="txt_Senha" type="password" class="form-control" placeholder="Senha"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Button runat="server" ID="btn_Login" Text="Efetuar Login" OnClick="btn_Login_Click" class="btn btn-primary" />
                <br />
                <asp:LinkButton runat="server" ID="btn_Esqueci_Senha" Text="Esqueceu seu usuário ou senha?" OnClick="btn_Esqueci_Senha_Click"></asp:LinkButton>
            </div>
        </div>
    </form>
</body>
</html>