<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cliente_cadastro.aspx.cs" Inherits="FixFinder.Pages.cadastroCliente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.0.0.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-group">
                <label for="txt_CPF">CPF</label>
                <asp:TextBox runat="server" ID="txt_CPF" CssClass="form-control" onkeypress="$(this).mask('000.000.000-00');"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Nome">Nome completo</label>
                <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Telefone">Telefone</label>
                <asp:TextBox runat="server" ID="txt_Telefone" CssClass="form-control" onkeypress="$(this).mask('(000) 0 0000-0000');"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Email">E-mail</label>
                <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control" type="email"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Login">Login</label>
                <asp:TextBox runat="server" ID="txt_Login" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Senha">Senha</label>
                <asp:TextBox runat="server" ID="txt_Senha" CssClass="form-control" type="password"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="date_DataNascimento">Data de nascimento</label>
                <asp:TextBox runat="server" ID="date_DataNascimento" CssClass="form-control" type="date"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastro" CssClass="btn btn-primary" OnClick="btn_Cadastro_Click" Text="Cadastrar" />
            </div>
        </div>
    </form>
</body>
</html>