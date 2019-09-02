<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cliente_Cadastro.aspx.cs" Inherits="FixFinder.Pages.cliente_Cadastro" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <div class="container mt-5">
            <%--formnovalidate--%>
            <h1 class="display-4 text-primary" style="text-align: center">Cadastro</h1>
            <div class="form-group">
                <label for="txt_CPF">CPF</label>
                <asp:TextBox runat="server" ID="txt_CPF" CssClass="form-control" minlength="14" onkeypress="$(this).mask('000.000.000-00');" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Nome">Nome completo</label>
                <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Telefone">Telefone</label>
                <asp:TextBox runat="server" ID="txt_Telefone" CssClass="form-control" minlength="8" onkeypress="$(this).mask('(00) 0 0000-0000');" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Email">E-mail</label>
                <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control" type="email" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Login">Login</label>
                <asp:TextBox runat="server" ID="txt_Login" CssClass="form-control" minlength="6" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Senha">Senha</label>
                <asp:TextBox runat="server" ID="txt_Senha" CssClass="form-control" minlength="6" type="password" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_SenhaConfirma">Confirme a senha</label>
                <asp:TextBox runat="server" ID="txt_SenhaConfirma" CssClass="form-control" minlength="6" type="password" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="date_DataNascimento">Data de nascimento</label>
                <asp:TextBox runat="server" ID="date_DataNascimento" CssClass="form-control" type="date" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastro" CssClass="btn btn-primary" OnClick="btn_Cadastro_Click" Text="Cadastrar" />
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