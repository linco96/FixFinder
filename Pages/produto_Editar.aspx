<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="produto_Editar.aspx.cs" Inherits="FixFinder.Pages.produto_Editar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Editar Produtro</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_EditarProduto" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Cadastro de produto</h1>
            <div class="form-group">
                <label for="txt_Descrição">Descrição</label>
                <asp:TextBox runat="server" ID="txt_Descricao" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Quantidade">Quantidade</label>
                <asp:TextBox runat="server" ID="txt_Quantidade" CssClass="form-control" type="number" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_PrecoCompra">Preço de compra</label>
                <asp:TextBox runat="server" ID="txt_PrecoCompra" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_PrecoVenda">Preço de venda (Opcional)</label>
                <asp:TextBox runat="server" ID="txt_PrecoVenda" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Marca">Marca</label>
                <asp:TextBox runat="server" ID="txt_Marca" CssClass="form-control" minlength="2" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Validade">Validade (Opcional)</label>
                <asp:TextBox runat="server" ID="txt_Validade" CssClass="form-control" type="date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Categoria">Categoria</label>
                <asp:TextBox runat="server" ID="txt_Categoria" CssClass="form-control" minlength="2" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Editar" CssClass="btn btn-primary" OnClick="btn_Editar_Click" Text="Editar" />
                <asp:Button runat="server" ID="btn_Voltar" CssClass="btn btn-danger" OnClick="btn_Voltar_Click" Text="Voltar" formnovalidate="true" />
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