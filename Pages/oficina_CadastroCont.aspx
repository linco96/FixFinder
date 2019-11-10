<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="oficina_CadastroCont.aspx.cs" Inherits="FixFinder.Pages.oficina_CadastroCont" %>

<%@ Register TagPrefix="uc" TagName="Header_Control" Src="~/Controls/Header_Control.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de oficina</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/popper.min.js"></script>
    <script src="../Scripts/jquery.mask.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:Header_Control runat="server" ID="Header_Control"></uc:Header_Control>
        <div class="container mt-5">
            <asp:Panel runat="server" ID="pnl_Alert" Visible="false" CssClass="alert alert-danger" role="alert">
                <asp:Label ID="lbl_Alert" runat="server"></asp:Label>
            </asp:Panel>

            <h2 class="text-primary" style="text-align: center">Assinatura</h2>
            <h3 runat="server" id="lbl_Valor" style="text-align: center">Valor da assinatura: R$ 49,90/mês</h3>
            <h6 style="text-align: center" class="text-muted">Com uma assinatura FixFinder a sua oficina pode desfrutar das nossas funcionalidades de gestão, assim como aparecer nas pesquisas de clientes</h6>

            <hr />

            <div runat="server" id="div_Quick" class="form-group">
                <label for="txt_Cartao">Cartões cadastrados</label>
                <asp:DropDownList runat="server" ID="txt_Cartao" ReadOnly="true" CssClass="form-control custom-select width100" OnSelectedIndexChanged="txt_Cartao_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <div class="form-inline">
                    <span class="w-50 text-left pr-3">
                        <label for="txt_NumeroCartao" style="display: block">Número do Cartão</label>
                        <asp:TextBox runat="server" ID="txt_NumeroCartao" ReadOnly="true" CssClass="form-control w-100" minlength="15" onkeypress="$(this).mask('0000 0000 0000 0000 000');" required="required" autocomplete="off"></asp:TextBox>
                    </span>
                    <span class="w-25 text-left pr-3">
                        <label for="txt_Vencimento" style="display: block">Data de Vencimento</label>
                        <asp:TextBox runat="server" ID="txt_Vencimento" ReadOnly="true" CssClass="form-control w-100" required="required" minlength="7" onkeypress="$(this).mask('00/0000');" autocomplete="off"></asp:TextBox>
                    </span>
                    <span class="w-25 text-left">
                        <label for="txt_Cvv" style="display: block">CVV</label>
                        <asp:TextBox runat="server" ID="txt_Cvv" ReadOnly="true" CssClass="form-control w-100" onkeypress="$(this).mask('0000');" minlength="3" required="required" autocomplete="off"></asp:TextBox>
                    </span>
                </div>
            </div>

            <div class="form-group">
                <div class="form-inline">
                    <span class="w-100 text-left">
                        <label for="txt_Titular" style="display: block">Nome do Titular</label>
                        <asp:TextBox runat="server" ID="txt_Titular" ReadOnly="true" CssClass="form-control w-100" required="required" minlength="6" onkeyup="this.value = this.value.toUpperCase();" autocomplete="off"></asp:TextBox>
                    </span>
                </div>
            </div>

            <div runat="server" id="div_Check" class="form-group">
                <asp:CheckBox ID="chk_Salvar" runat="server" TextAlign="Right" />
                <label class="form-check-label" for="chk_Salvar">
                    Salvar cartão para pagamentos futuros
                </label>
            </div>

            <div class="form-group">
                <asp:Button runat="server" ID="btn_CriarAssinatura" Text="Realizar pagamento" CssClass="btn btn-success" />
            </div>
        </div>
    </form>
</body>
</html>