<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agendamento_ListaOficina.aspx.cs" Inherits="FixFinder.Pages.agendamento_ListaOficina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agendamentos</title>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/dashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
            </asp:Panel>
            <div>
                <asp:Button runat="server" ID="btn_MostrarEsconderPassados" Text="Mostrar agendamentos passados" CssClass="btn btn-outline-primary btn-sm mb-2" aria-pressed="true" OnClick="btn_MostrarEsconderPassados_Click" formnovalidate="true" />
            </div>
            <div class="table-responsive-xl">
                <asp:Table runat="server" ID="tbl_Agendamentos" CssClass="table border rounded-lg mt-3">
                </asp:Table>
            </div>
        </div>
    </form>
</body>
</html>