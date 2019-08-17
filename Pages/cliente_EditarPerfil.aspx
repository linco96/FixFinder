﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cliente_EditarPerfil.aspx.cs" Inherits="FixFinder.Pages.cliente_EditarPerfil" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Editar perfil</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Perfil</h1>
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
                <label for="date_DataNascimento">Data de nascimento</label>
                <asp:TextBox runat="server" ID="date_DataNascimento" CssClass="form-control" type="date" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <h5 aria-describedby="senha_Help">Alterar senha</h5>
                <small id="senha_Help" class="form-text text-muted">Para alterar a sua senha informe primeiramente a sua senha atual. Caso não deseje alterar a sua senha deixe estes campos em branco</small>
            </div>
            <div class="form-group">
                <label for="txt_Senha">Senha atual</label>
                <asp:TextBox runat="server" ID="txt_Senha" CssClass="form-control" type="password" minlength="6"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_SenhaNova">Nova senha</label>
                <asp:TextBox runat="server" ID="txt_SenhaNova" CssClass="form-control" type="password" minlength="6"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_SenhaNova">Confirme a nova senha</label>
                <asp:TextBox runat="server" ID="txt_SenhaNovaConfirma" CssClass="form-control" type="password" minlength="6"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Salvar" CssClass="btn btn-primary" OnClick="btn_Salvar_Click" Text="Salvar alterações"></asp:Button>
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