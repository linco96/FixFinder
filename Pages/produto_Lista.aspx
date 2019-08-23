<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="produto_Lista.aspx.cs" Inherits="FixFinder.Pages.produto_Lista" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista produtos</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_ListaProduto" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Seus Produtos</h1>
            <hr class="border-primary" />
            <div>
                <asp:Button runat="server" ID="btn_CadastrarProduto" Text="Cadastrar Produto" CssClass="btn btn-outline-primary btn-sm mb-2" aria-pressed="true" OnClick="btn_CadastrarProduto_Click" />
            </div>
            <div class="table-responsive-xl">
                <asp:Table runat="server" ID="tbl_Produtos" CssClass="table border rounded-lg">
                    <asp:TableHeaderRow runat="server" ID="tblH_Produtos" CssClass="thead-dark">
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Produto</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Marca</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Categoria</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Qtd.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Validade</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço Compra</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço Venda</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </div>
    </form>
</body>
</html>