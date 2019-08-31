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
            <%--FORNECEDOR--%>
            <div class="form-group">
                <label class="h4">Fornecedor</label>
                <asp:DropDownList runat="server" ID="select_Fornecedores" CssClass="form-control" OnSelectedIndexChanged="select_Fornecedores_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                    <asp:Label runat="server" ID="lbl_Alert" CssClass="text-danger form-text text-muted" Text="Nenhum fornecedor cadastrado."></asp:Label>
                </asp:Panel>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_FornecedorCNPJ" style="display: block">CNPJ</label>
                    <asp:TextBox runat="server" ID="txt_FornecedorCNPJ" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_FornecedorNome" style="display: block">Nome</label>
                    <asp:TextBox runat="server" ID="txt_FornecedorNome" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_FornecedorTelefone" style="display: block">Telefone</label>
                    <asp:TextBox runat="server" ID="txt_FornecedorTelefone" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_FornecedorEmail" style="display: block">E-mail</label>
                    <asp:TextBox runat="server" ID="txt_FornecedorEmail" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>

            <hr class="border border-primary border-bottom-0" />

            <%--PRODUTO--%>
            <div class="form-group">
                <label class="h4">Produto</label>
                <asp:DropDownList runat="server" ID="select_Produto" CssClass="form-control" OnSelectedIndexChanged="select_Produto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Panel runat="server" ID="pnl_AlertProduto" Visible="false" CssClass="alert alert-danger" role="alert">
                    <asp:Label runat="server" ID="lbl_AlertProduto" CssClass="text-danger form-text text-muted" Text="Nenhum produto cadastrado."></asp:Label>
                </asp:Panel>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_Produto" style="display: block">Produto</label>
                    <asp:TextBox runat="server" ID="txt_Produto" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3 mb-1">
                    <label for="txt_ProdutoQuantidadeAtual" style="display: block">Quantidade atual</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoQuantidadeAtual" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3 mb-1">
                    <label for="txt_ProdutoMarca" style="display: block">Marca</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoMarca" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3 mb-1">
                    <label for="txt_ProdutoCategoria" style="display: block">Categoria</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoCategoria" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>

            <div class="form-inline">
                <span class="w-50 text-left pr-3">
                    <label for="txt_ProdutoPrecoCompra" style="display: block">Preço Compra (R$)</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoPrecoCompra" onkeypress="$(this).mask('#.##0,00', {reverse: true});" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_ProdutoPrecoVenda" style="display: block">Preço Venda (R$)</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoPrecoVenda" onkeypress="$(this).mask('#.##0,00', {reverse: true});" CssClass="form-control w-100" ReadOnly="true"></asp:TextBox>
                </span>
            </div>
            <div class="form-inline">
                <span class="w-50 text-left pr-3">
                    <label for="txt_ProdutoQuantidade" style="display: block">Quantidade</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoQuantidade" CssClass="form-control w-100" minlength="1" required onkeypress="$(this).mask('#.##0', {reverse: true});" placeholder="Digite a quantidade de produtos a serem comprados..."></asp:TextBox>
                </span>
                <span class="w-50 text-left pl-3">
                    <label for="txt_ProdutoValidade" style="display: block">Validade</label>
                    <asp:TextBox runat="server" ID="txt_ProdutoValidade" CssClass="form-control w-100" type="date"></asp:TextBox>
                </span>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_AdicionarProduto" CssClass="btn btn-outline-success btn-sm mt-2" Text="Adicionar Produto" OnClick="btn_AdicionarProduto_Click" Enabled="false" />
                <asp:Button runat="server" ID="btn_CadastrarProduto" CssClass="btn btn-outline-primary btn-sm mt-2" Text="Cadastrar Produto" OnClick="btn_CadastrarProduto_Click" formnovalidate />
            </div>
            <%--LISTA PRODUTOS--%>
            <div class="table-responsive-xl">
                <asp:Table runat="server" ID="tbl_Produtos" CssClass="table border rounded-lg">
                    <%--<asp:TableHeaderRow runat="server" ID="tblH_Produtos" CssClass="thead-light">--%>
                    <%--<asp:TableHeaderCell Scope="Column" CssClass="text-center">Produto</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Marca</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Categoria</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Quantidade</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço Compra</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Preço Venda</asp:TableHeaderCell>
                        <asp:TableHeaderCell Scope="Column" CssClass="text-center">Ações</asp:TableHeaderCell>--%>
                    <%--</asp:TableHeaderRow>--%>
                </asp:Table>
            </div>
        </div>
    </form>
</body>
</html>