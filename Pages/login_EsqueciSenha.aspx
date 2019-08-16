<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login_EsqueciSenha.aspx.cs" Inherits="FixFinder.Pages.login_EsqueciSenha" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Esqueci minha senha</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_EsqueceuSenha" runat="server">
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <h1 style="text-align: center" class="display-4 text-primary">Fix Finder</h1>
        <div class="container w-25 p-3">
            <div class="form-group">
                <label>Forneça seu</label>
                <br />
                <label class="font-weight-bold text-primary">CPF</label>
                <asp:TextBox runat="server" ID="txt_CPF" minlength="14" onkeypress="$(this).mask('000.000.000-00');" class="form-control" placeholder="Digite seu CPF"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Ou</label>
                <br />
                <label class="font-weight-bold text-primary">E-mail</label>
                <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control" aria-describedby="emailHelp" placeholder="Digite seu E-Mail"></asp:TextBox>
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