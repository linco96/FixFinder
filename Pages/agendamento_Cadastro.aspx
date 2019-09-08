<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="agendamento_Cadastro.aspx.cs" Inherits="FixFinder.Pages.agendamento_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Novo agendamento</title>
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
            <div class="form-group">
                <label for="txt_Data">Data</label>
                <asp:TextBox runat="server" ID="txt_Data" CssClass="form-control" type="date" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Hora">Hora</label>
                <asp:DropDownList runat="server" ID="ddl_Banco" CssClass="custom-select custom-select-sm">
                    <asp:ListItem Text="07:00" Value="07:00"></asp:ListItem>
                    <asp:ListItem Text="08:00" Value="08:00"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastrar" CssClass="btn btn-primary" Text="Agendar" OnClick="btn_Cadastrar_Click" />
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