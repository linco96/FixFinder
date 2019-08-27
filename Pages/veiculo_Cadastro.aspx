<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="veiculo_Cadastro.aspx.cs" Inherits="FixFinder.Pages.veiculo_Cadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro Veículo</title>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form_CadastroVeiculo" runat="server">
        <div class="container mt-5">
            <h1 class="display-4 text-primary" style="text-align: center">Cadastro de Veículo</h1>
            <div class="form-group">
                <label for="txt_Marca">Marca</label>
                <asp:TextBox runat="server" ID="txt_Marca" CssClass="form-control" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Modelo">Modelo</label>
                <asp:TextBox runat="server" ID="txt_Modelo" CssClass="form-control" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_Ano">Ano</label>
                <asp:TextBox runat="server" ID="txt_Ano" CssClass="form-control" minlength="4" onkeypress="$(this).mask('0000');" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Selecione o modelo da placa</label>
                <asp:RadioButtonList runat="server" ID="radio_ModeloPlaca" OnSelectedIndexChanged="radio_ModeloPlaca_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Placa nova" Value="nova"></asp:ListItem>
                    <asp:ListItem Text="Placa antiga" Value="antiga"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:TextBox runat="server" ID="txt_PlacaAntiga" CssClass="form-control" onkeypress="$(this).mask('SSS-0000');" minlength="8" placeholder="Digite sua placa..." Visible="false"></asp:TextBox>
                <asp:TextBox runat="server" ID="txt_PlacaNova" CssClass="form-control" onkeypress="$(this).mask('SSS 0S00');" minlength="8" placeholder="Digite sua placa..." Visible="false"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btn_Cadastrar" CssClass="btn btn-primary m-1" OnClick="btn_Cadastrar_Click" Text="Cadastrar" />
                <asp:Button runat="server" ID="btn_Voltar" CssClass="btn btn-primary m-1" OnClick="btn_Voltar_Click" Text="Voltar" formnovalidate />
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