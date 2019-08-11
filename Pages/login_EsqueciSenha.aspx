<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login_EsqueciSenha.aspx.cs" Inherits="FixFinder.Pages.login_EsqueciSenha" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.0.0.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
</head>
<body>
    <form id="form_EsqueceuSenha" runat="server">
        <div class="container w-25 p-3">
            <div class="form-group">
                <h1 style="text-align: center" class="display-4 text-primary">Digite seu CPF e ou e-mail</h1>
                <asp:TextBox runat="server" ID="txt_CPF" class="form-control" aria-describedby="emailHelp" placeholder="Digite seu CPF" required></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_EnviarEmail" Text="Solicitar senha" OnClick="btn_EnviarEmail_Click" class="btn btn-primary" />
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