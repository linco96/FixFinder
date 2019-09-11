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
            <hr class="border border-primary border-bottom-0 mt-1 mb-1" />

            <%--DADOS ORCAMENTO--%>
            <div class="form-group">
                <label for="txt_Veiculo">Veiculo</label>
                <asp:TextBox runat="server" ID="txt_Veiculo" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txt_PrecoTotal">Preço Total</label>
                <asp:TextBox runat="server" ID="txt_PrecoTotal" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <%--AVALIAR--%>
            <div class="form-group">
                <div class="form-inline">
                    <h4>Faça sua avaliação</h4>
                </div>
                <div class="form-inline">
                    <asp:RadioButtonList runat="server" ID="radio_AvaliacaoServico" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="false" sytle="display: block" CssClass="w-25 text-center" required>
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-inline">
                    <asp:RadioButtonList runat="server" ID="RadioButtonList1" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="false" sytle="display: block" CssClass="w-25 text-center" required>
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-inline">
                    <small id="horario_Help" class="form-text text-muted">Selecione 1 para muito ruim ou 10 para muito bom</small>
                </div>
                <div class="form-inline">
                    <h4>Escreva seu comentário</h4>
                </div>
                <div class="form-inline mt-1">
                    <span class="w-100 text-left">
                        <asp:TextBox runat="server" ID="txt_Descrição" CssClass="form-control w-100" TextMode="MultiLine" autocomplete="off" Columns="100" Rows="3" MaxLength="200" required></asp:TextBox>
                    </span>
                </div>
            </div>
            <div class="form-inline mt-1">
                <asp:Panel ID="pnl_Alert" runat="server" Visible="false" CssClass="alert alert-success w-100" role="alert">
                    <asp:Label runat="server" ID="lbl_Alert" CssClass="form-text text-muted"></asp:Label>
                </asp:Panel>
            </div>
            <div class="form-inline mt-1">
                <asp:Button runat="server" ID="btn_Avaliar" Text="Avaliar" OnClick="btn_Avaliar_Click" CssClass="btn btn-success mr-1" />
                <asp:Button runat="server" ID="btn_Cancelar" Text="Cancelar" OnClick="btn_Cancelar_Click" CssClass="btn btn-danger" formnovalidate />
            </div>
        </div>
    </form>
</body>
</html>