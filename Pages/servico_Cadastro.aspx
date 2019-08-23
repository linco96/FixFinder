<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="servico_Cadastro.aspx.cs" Inherits="FixFinder.Pages.servico_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de produto</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Cadastro de serviço</h1>
            <div class="form-group">
                <label for="txt_Descrição">Descrição</label>
                <asp:TextBox runat="server" ID="txt_Descricao" CssClass="form-control" minlength="4" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Valor">Valor</label>
                <asp:TextBox runat="server" ID="txt_Valor" CssClass="form-control" onkeypress="$(this).mask('#.##0,00', {reverse: true});" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastro" CssClass="btn btn-primary" OnClick="btn_Cadastro_Click" Text="Cadastrar" />
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