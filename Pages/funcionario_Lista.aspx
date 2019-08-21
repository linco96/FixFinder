<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="funcionario_Lista.aspx.cs" Inherits="FixFinder.Pages.funcionario_Lista" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Funcionários</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_ListaVeiculo" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Funcionários</h1>
            <hr class="border-primary" />
            <div>
                <asp:Button runat="server" ID="btn_RegistrarFuncionario" Text="Registrar funcionário" CssClass="btn btn-outline-primary btn-sm mb-2" aria-pressed="true" OnClick="btn_RegistrarFuncionario_Click" />
            </div>
            <div class="table-responsive-xl">
                <asp:Table runat="server" ID="tbl_Funcionarios" CssClass="table border rounded-lg">
                    <asp:TableHeaderRow runat="server" ID="tblH_Funcionarios" CssClass="thead-dark">
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Nome</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Telefone</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">E-mail</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Cargo</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Salário</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Banco</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Agência</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Conta</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
            </asp:Panel>
        </div>
    </form>
</body>
</html>