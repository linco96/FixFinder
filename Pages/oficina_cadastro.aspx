<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="oficina_cadastro.aspx.cs" Inherits="FixFinder.Pages.oficina_cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de oficina</title>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="form-group">
                <label for="txt_cnpj">CNPJ</label>
                <asp:TextBox runat="server" ID="txt_cnpj" CssClass="form-control" onkeypress="$(this).mask('00.000.000/0000-00');" required="required"></asp:TextBox>
            </div>
        </div>
    </form>
</body>
</html>