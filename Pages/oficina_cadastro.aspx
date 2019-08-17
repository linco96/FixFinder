<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oficina_Cadastro.aspx.cs" Inherits="FixFinder.Pages.oficina_cadastro" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>
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
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Cadastro de Oficina</h1>
            <div class="form-group">
                <label for="txt_cnpj">CNPJ</label>
                <asp:TextBox runat="server" ID="txt_CNPJ" CssClass="form-control" minlength="18" onkeypress="$(this).mask('00.000.000/0000-00');" required="required"></asp:TextBox>
                <%--    32.154.993/0001-42      --%>
            </div>
            <div class="form-group">
                <label for="txt_nome">Nome</label>
                <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Telefone">Telefone</label>
                <asp:TextBox runat="server" ID="txt_Telefone" CssClass="form-control" onkeypress="$(this).mask('(00) 0 0000-0000');" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Email">E-mail</label>
                <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control" type="email" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="num_Agendamentos">Capacidade de agendamentos</label>
                <asp:TextBox runat="server" ID="num_Agendamentos" CssClass="form-control" aria-describedby="agendamento_Help" type="number" required="required"></asp:TextBox>
                <small id="agendamento_Help" class="form-text text-muted">Informe quantos clientes a sua oficina pode atender simultâneamente</small>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastrar" CssClass="btn btn-primary" OnClick="btn_Cadastrar_Click" Text="Cadastrar" />
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