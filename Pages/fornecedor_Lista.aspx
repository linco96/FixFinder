<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fornecedor_Lista.aspx.cs" Inherits="FixFinder.Pages.fornecedor_Lista" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Fornecedor</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_ListaFornecedor" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Seus Fornecedores</h1>
            <hr class="border-primary" />
            <div>
                <asp:Button runat="server" ID="btn_CadastrarFornecedor" Text="Cadastrar Fornecedor" CssClass="btn btn-outline-primary btn-sm mb-2" aria-pressed="true" OnClick="btn_CadastrarFornecedor_Click" Visible="false" />
            </div>
            <div class="table-responsive-xl">
                <asp:Table runat="server" ID="tbl_Fornecedores" CssClass="table border rounded-lg">
                    <asp:TableHeaderRow runat="server" ID="tblH_Fornecedores" CssClass="thead-dark">
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">CNPJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Fornecedor</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Telefone</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">E-mail</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </div>
    </form>
</body>
</html>