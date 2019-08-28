<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="compra_Cadastrar.aspx.cs" Inherits="FixFinder.Pages.compra_Cadastrar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Comprar</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_Compra" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Realizar Compras</h1>
            <div class="form-group">
                <label for="txt_cnpj">Selecione um fornecedor</label>
                <asp:DropDownList runat="server" ID="select_Fornecedores" CssClass="form-control" OnSelectedIndexChanged="select_Fornecedores_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                    <small class="form-text text-muted">Nenhum fornecedor cadastrado.</small>
                </asp:Panel>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3">
                    <label for="txt_CNPJ" style="display: block">CNPJ</label>
                    <asp:TextBox runat="server" ID="txt_CNPJ" CssClass="form-control w-100" minlength="18" onkeypress="$(this).mask('00.000.000/0000-00');" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_Nome" style="display: block">Nome</label>
                    <asp:TextBox runat="server" ID="txt_Nome" CssClass="form-control w-100" minlength="4" ReadOnly="true"></asp:TextBox>
                </span>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3">
                    <label for="txt_Telefone" style="display: block">Telefone</label>
                    <asp:TextBox runat="server" ID="txt_Telefone" CssClass="form-control w-100" onkeypress="$(this).mask('(00) 0 0000-0000');" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_Email" style="display: block">E-mail</label>
                    <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control w-100" type="email" ReadOnly="true"></asp:TextBox>
                </span>
            </div>
        </div>
    </form>
</body>
</html>