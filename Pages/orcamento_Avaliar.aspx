<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orcamento_Avaliar.aspx.cs" Inherits="FixFinder.Pages.orcamento_Avaliar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Avaliar Orçamento</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_Avaliar" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Avaliar Orçamento</h1>

            <%--AVALIACAOD DA OFICINA--%>
            <div class="form-group">
                <h2 style="text-align: center"><span runat="server" id="lbl_Oficina" class="align-middle mr-2">Paje Motors</span>
                    <img class="align-middle ml-2" src="../Content/star_32.png" />
                    <asp:Label runat="server" ID="lbl_Reputacao" CssClass="align-middle">7/10</asp:Label></h2>
                <h5 runat="server" id="lbl_Endereco" class="text-muted" style="text-align: center">Rua DAS na Torre, 2062<br />
                    NteInteressa, Curitiba</h5>
            </div>

            <%--FIM--%>
        </div>
    </form>
</body>
</html>